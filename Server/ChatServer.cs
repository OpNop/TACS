using Gw2Sharp;
using Gw2Sharp.WebApi.V2;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TACS_Server.Commands;
using TACS_Server.User;
using TACSLib;
using TACSLib.Packets.Client;
using TACSLib.Packets.Server;
using TinyLogger;
using TinyTools.TinySocket;
using TinyTools.TinySocket.Server;

namespace TACS_Server
{
    public class ChatServer
    {
        public UserSessionList userSessionList;
        public IGw2WebApiV2Client gw2api;

        private readonly Logger Log;
        private readonly ChatCommandHandler commandHandler;
        internal readonly RSACryptoServiceProvider mRSASelf;

        //MQTT Client
        private IMqttClient mqttClient;

        public ChatServer()
        {
            Log = Logger.GetInstance();
            mRSASelf = new RSACryptoServiceProvider(2048);
            userSessionList = new UserSessionList(this);
            commandHandler = new ChatCommandHandler(userSessionList)
               .BuildAdminCommands()
               .BuildUserCommands()
               .BuildEmoteCommands();

            //Connect to GW2 API
            var apiClient = new Gw2Client(new Connection());
            gw2api = apiClient.WebApi.V2;

        }

        public void UserAcceptHandler(ListenerSocket ss, Socket cs)
        {
            UserSession userSession = new UserSession(this);
            userSession.BeginReceive(cs);
            userSession.OneTimeKey = new byte[8];
            RandomNumberGenerator.Create().GetBytes(userSession.OneTimeKey);
            //p.Add(20201028);
            //p.Add(userSession.OneTimeKey);
            userSession.Send(new ServerVersion(mRSASelf.ToXmlString(false)));
        }

        public async Task OnUserDisconnected(UserSession userSession)
        {
            Log.AddNotice($"{userSession.AccountName} disconnecting");
            await userSessionList.Remove(userSession);
        }

        public async Task OnUserPacket(UserSession userSession, PacketType type, Unpacker p)
        {
            //Fun MQTT publish
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("tiny/tacs")
                .WithPayload($"Incoming Packet of type {type}")
                .Build();
            await mqttClient.PublishAsync(message, CancellationToken.None); // Since 3.0.5 with CancellationToken

            try
            {
                //non logged in user packets
                if (!userSession.IsAuthenticated)
                {
                    switch (type)
                    {
                        case PacketType.C_SEND_CERT:
                            await OnUserCert(userSession, p);
                            break;
                        case PacketType.C_LOGIN:
                            await OnUserLogin(userSession, p);
                            break;
                        default:
                            Log.AddError("User trying to do something without being logged in");
                            userSession.Stop();
                            await userSessionList.Remove(userSession);
                            break;
                    }
                }
                //logged in user packets
                else
                {
                    switch (type)
                    {
                        case PacketType.C_CHANGE_CHARACTER:
                            OnUserChangeCharacter(userSession, p);
                            break;
                        case PacketType.C_SEND_MESSAGE:
                            await OnUserSendMessage(userSession, p);
                            break;
                        case PacketType.C_UPDATE:
                            await OnUserUpdate(userSession, p);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.AddError(e.Message);
                Log.AddError(e.StackTrace);
                userSession.Stop();
                await userSessionList.Remove(userSession);
            }
        }

        private async Task OnUserCert(UserSession userSession, Unpacker p)
        {
            var packet = new SendCert(p);

            userSession.InitClientKey(packet.RSAKey);
        }

        private async Task OnUserUpdate(UserSession userSession, Unpacker p)
        {
            var packet = new Update(p);

            if (packet.MumbleData.CharacterName != userSession.CharacterName)
                userSession.CharacterName = packet.MumbleData.CharacterName;

            userSession.MumbleData = packet.MumbleData;
        }

        private async Task OnUserLogin(UserSession userSession, Unpacker p)
        {
            var packet = new Login(p);
            userSession.Version = packet.ClientVersion;
            userSession.APIKey = packet.APIKey;

            if (await userSession.ValidateKey() == false)
            {
                userSession.Send(new LoginResult(0, "Invalid API Key"));
                userSession.Stop();
            }
            else if (await userSessionList.Exist(userSession.AccountName))
            {
                userSession.Send(new LoginResult(0, "Already logged in"));
                userSession.Stop();
            }
            else if (await userSession.ValidateGuild() == false)
            {
                userSession.Send(new LoginResult(0, "No supported Guild"));
                userSession.Stop();
            }
            else
            {
                userSession.CharacterName = userSession.AccountName;
                userSession.IsAuthenticated = true;
                await userSessionList.Add(userSession);
                userSession.Send(new LoginResult(1));

                //Broadcast that user has logged in
                await userSessionList.BroadcastExcludeSender(new StatusChange(userSession.AccountName, StatusType.Online), userSession);
            }
        }

        private void OnUserChangeCharacter(UserSession userSession, Unpacker p)
        {
            var packet = new ChangeCharacter(p);
            userSession.CharacterName = packet.CharacterName;
        }

        private async Task OnUserSendMessage(UserSession userSession, Unpacker p)
        {
            if (userSession.IsMuted)
            {
                userSession.Send(new ServerSendMessage("You are currently muted"));
                return;
            }

            var packet = new ClientSendMessage(p);


            //Check for chat or admin command
            if (packet.Message.StartsWith("/"))
            {
                var split = packet.Message[1..].Split(" ", 2);
                var command = split[0];
                var args = string.Empty;
                if (split.Length > 1)
                    args = split[1];

               await commandHandler.ExecuteCommand(userSession, command, args);
            }
            else
            {
                //process normal message
                await userSessionList.Broadcast(new ServerSendMessage(packet.Message, userSession.CharacterName, MessageType.NORMAL));
            }
        }

        public async Task GetAPIBuild()
        {
            //Check API connection
            var build = await gw2api.Build.GetAsync();
            Log.AddNotice($"API: {build.Id}");
        }

        private async Task SetupMQTT()
        {
            // Create a new MQTT client.
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("TINYTACSServer")
                .WithTcpServer("broker.hivemq.com")
                .WithCleanSession()
                .Build();

            await mqttClient.ConnectAsync(options);

            mqttClient.UseDisconnectedHandler(async e =>
            {
                Log.AddError("### DISCONNECTED FROM MQTT SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    Log.AddError("### RECONNECTING MQTT FAILED ###");
                }
            });

        }

        public async Task Start()
        {
            Log.AddNotice($"TACS Chat Service Start (build {Program.Config.Build})");

            await GetAPIBuild();
            Log.AddNotice($"Added {commandHandler.UserCommandCount()} User Commands, {commandHandler.AdminCommandCount()} Admin Commands");

            //Connect to MQTT
            Log.AddNotice("Connecting to MQTT server");
            await SetupMQTT();

            //Start listener
            var host = Dns.GetHostEntry(Dns.GetHostName());
            Log.AddNotice($"Server Name: {host.HostName} Server IP: {IPAddress.Any}");

            ListenerSocket userListenerSocket = new ListenerSocket();
            userListenerSocket.Accepted += new ListenerSocket.AcceptedHandler(UserAcceptHandler);
            userListenerSocket.Start(new IPEndPoint(IPAddress.Any, 8888));
        }
    }
}
