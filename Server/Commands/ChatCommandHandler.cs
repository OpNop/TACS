using System.Collections.Generic;
using System.Threading.Tasks;
using TACS_Server.Commands;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server
{
    public class ChatCommandHandler
    {
        public delegate Task CommandHandler(UserSession user, UserSessionList userList, string args);
        public int UserCommandCount() => UserCommands.Count;
        public int AdminCommandCount() => AdminCommands.Count;

        private readonly UserSessionList UserList;
        private readonly Dictionary<string, CommandHandler> UserCommands = new Dictionary<string, CommandHandler>();
        private readonly Dictionary<string, CommandHandler> AdminCommands = new Dictionary<string, CommandHandler>();

        public ChatCommandHandler(UserSessionList userList)
        {
            UserList = userList;

            //Load user commands
            UserCommands.Add("afk", new CommandHandler(UserCommand.Afk));
            UserCommands.Add("admin", new CommandHandler(UserCommand.Admin));
            UserCommands.Add("list", new CommandHandler(UserCommand.List));
            //Load emotes
            UserCommands.Add("beckon", new CommandHandler(EmoteCommand.Beckon));
            UserCommands.Add("bow", new CommandHandler(EmoteCommand.Bow));
            UserCommands.Add("cheer", new CommandHandler(EmoteCommand.Cheer));
            UserCommands.Add("cower", new CommandHandler(EmoteCommand.Cower));
            UserCommands.Add("crossarms", new CommandHandler(EmoteCommand.Crossarms));
            UserCommands.Add("cry", new CommandHandler(EmoteCommand.Cry));
            UserCommands.Add("dance", new CommandHandler(EmoteCommand.Dance));
            UserCommands.Add("facepalm", new CommandHandler(EmoteCommand.Facepalm));
            UserCommands.Add("upset", new CommandHandler(EmoteCommand.Facepalm));
            UserCommands.Add("geargrind", new CommandHandler(EmoteCommand.Geargrind));
            UserCommands.Add("gg", new CommandHandler(EmoteCommand.Gg));
            UserCommands.Add("kneel", new CommandHandler(EmoteCommand.Kneel));
            UserCommands.Add("laugh", new CommandHandler(EmoteCommand.Laugh));
            UserCommands.Add("no", new CommandHandler(EmoteCommand.No));
            UserCommands.Add("playdead", new CommandHandler(EmoteCommand.Playdead));
            UserCommands.Add("point", new CommandHandler(EmoteCommand.Point));
            UserCommands.Add("ponder", new CommandHandler(EmoteCommand.Ponder));
            UserCommands.Add("rockout", new CommandHandler(EmoteCommand.Rockout));
            UserCommands.Add("sad", new CommandHandler(EmoteCommand.Sad));
            UserCommands.Add("salute", new CommandHandler(EmoteCommand.Salute));
            UserCommands.Add("shiver", new CommandHandler(EmoteCommand.Shiver));
            UserCommands.Add("shrug", new CommandHandler(EmoteCommand.Shrug));
            UserCommands.Add("shuffle", new CommandHandler(EmoteCommand.Shuffle));
            UserCommands.Add("sit", new CommandHandler(EmoteCommand.Sit));
            UserCommands.Add("sleep", new CommandHandler(EmoteCommand.Sleep));
            UserCommands.Add("nap", new CommandHandler(EmoteCommand.Sleep));
            UserCommands.Add("step", new CommandHandler(EmoteCommand.Step));
            UserCommands.Add("surprised", new CommandHandler(EmoteCommand.Surprised));
            UserCommands.Add("talk", new CommandHandler(EmoteCommand.Talk));
            UserCommands.Add("thanks", new CommandHandler(EmoteCommand.Thanks));
            UserCommands.Add("thank", new CommandHandler(EmoteCommand.Thanks));
            UserCommands.Add("thk", new CommandHandler(EmoteCommand.Thanks));
            UserCommands.Add("ty", new CommandHandler(EmoteCommand.Thanks));
            UserCommands.Add("threaten", new CommandHandler(EmoteCommand.Threaten));
            UserCommands.Add("wave", new CommandHandler(EmoteCommand.Wave));
            UserCommands.Add("yes", new CommandHandler(EmoteCommand.Yes));
            UserCommands.Add("me", new CommandHandler(EmoteCommand.Me));
            UserCommands.Add("bite", new CommandHandler(EmoteCommand.Bite));
            UserCommands.Add("biteankle", new CommandHandler(EmoteCommand.Bite));
            //Load admin commands
            AdminCommands.Add("announce", new CommandHandler(AdminCommand.Announce));
            AdminCommands.Add("kick", new CommandHandler(AdminCommand.Kick));
            AdminCommands.Add("mute", new CommandHandler(AdminCommand.Mute));
            AdminCommands.Add("unmute", new CommandHandler(AdminCommand.Unmute));
        }

        public async Task ExecuteUserCommand(UserSession user, string command, string args)
        {
            if (UserCommands.ContainsKey(command))
            {
                await UserCommands[command](user, UserList, args);
            }
            else
            {
                user.Send(new ServerSendMessage("Command not found"));
            }
        }

        public async Task ExecuteAdminCommand(UserSession user, string command, string args)
        {
            //strip leading .
            command = command[1..];

            if (user.IsOfficer && AdminCommands.ContainsKey(command))
            {
                await AdminCommands[command](user, UserList, args);
            }
            else
            {
                user.Send(new ServerSendMessage("Command not found"));
            }
        }

        /*

        public Dictionary<string, MemberInfo> UserCommands = new Dictionary<string, MemberInfo>();
        public Dictionary<string, MemberInfo> AdminCommands = new Dictionary<string, MemberInfo>();

        public ChatCommandHandler() { }

        public async Task LoadUserCommands()
        {
            await Task.Run(() =>
            {
                //Get all classes that extend IChatCommand
                var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(m => m.IsClass && m.GetInterface("IChatCommandHandler") != null);

                //Parse Attributes into Dictonary
                foreach (var commandHandlers in commandClasses)
                {
                    //Get the commands from that class
                    var commands = commandHandlers.GetMethods().Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0).ToArray();
                    foreach (var command in commands)
                    {
                        var isAdmin = (IsAdminAttribute)command.GetCustomAttribute(typeof(IsAdminAttribute));
                        if (isAdmin != null)
                        {
                            AdminCommands.Add(command.Name, command);
                        }
                        else
                        {
                            UserCommands.Add(command.Name, command);
                        }
                    }
                }
            });
        }

        public async Task ExecuteUserCommand(UserSession user, string command, string[] args)
        {
            if (UserCommands.ContainsKey(command))
            {
                await UserCommands[command].;
            }
            else
            {
                user.Send(new ServerSendMessage("Command not found"));
            }
        }
    }

    public interface IChatCommandHandler
    {

    }*/
    }
}


//using System;
//using System.Collections;
//class SampleCommandApp
//    {   // any command function must match this interface (same return type and arguments)	
//        delegate void CommandHandler(string[] args);
//        public static void HelpCommandFunction(string[] args)
//        {
//            Console.WriteLine("Help: the only commands available are /help and /name");
//        }
//        public static void NameCommandFunction(string[] args)
//        {
//            if (args.Length != 3) Console.WriteLine("Invalid arguments: use /name {first} {last}"); else Console.WriteLine("Hello, {0} {1}", args[1], args[2]);
//        }
//        public static void MessageFunction(string[] args) { Console.WriteLine(String.Join(" ", args)); }
//        public static void Main(string[] args)
//        {
//            Hashtable cmds = new Hashtable();
//            cmds.Add("/help", new CommandHandler(HelpCommandFunction));
//            cmds.Add("/name", new CommandHandler(NameCommandFunction));
//            if (args[0].IndexOf("/") == 0)
//            {
//                if (cmds.ContainsKey(args[0]))
//                {               // a command handler is in the hash table				
//                                // so we cast the object to a CommandHandler				
//                                // and call it just like a function.				
//                    ((CommandHandler)cmds[args[0]])(args);
//                }
//                else Console.WriteLine("Unrecognized command.");
//            }
//            else { MessageFunction(args); }
//        }
//    }
