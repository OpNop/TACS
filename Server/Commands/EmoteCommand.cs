using System.Threading.Tasks;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
    public static class EmoteCommand
    {
        public static async Task Beckon(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} beckons to {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} beckons."));
        }

        public static async Task Bow(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} bow for {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} bow."));
        }

        public static async Task Cheer(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} cheers for {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} cheers."));
        }

        public static async Task Cower(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} cowers in fear from {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} cowers."));
        }

        public static async Task Crossarms(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} crosses their arms."));
        }

        public static async Task Cry(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is crying."));
        }

        public static async Task Dance(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is busting out some moves, some sweet dance moves."));
        }

        public static async Task Facepalm(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is upset."));
        }

        public static async Task Geargrind(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} does the Gear Grind."));
        }

        public static async Task Gg(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} says \"Good Game\"."));
        }

        public static async Task Kneel(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} kneels."));
        }

        public static async Task Laugh(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} laughs at {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} laughs."));
        }

        public static async Task No(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} disagrees with {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} disagrees."));
        }

        public static async Task Playdead(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is probably dead."));
        }

        public static async Task Point(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} points at {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} points."));
        }

        public static async Task Ponder(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} ponders."));
        }

        public static async Task Rockout(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is rocking out!"));
        }

        public static async Task Sad(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is sad."));
        }

        public static async Task Salute(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} salutes {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} salutes."));
        }

        public static async Task Shiver(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} shivers."));
        }

        public static async Task Shrug(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} shrugs at {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} shrugs."));
        }

        public static async Task Shuffle(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} does the Inventory Shuffle."));
        }

        public static async Task Sit(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} sits."));
        }

        public static async Task Sleep(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} goes to sleep."));
        }

        public static async Task Step(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} does the Dodge Step."));
        }

        public static async Task Surprised(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is surprised by {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is surprised."));
        }

        public static async Task Talk(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is talking to {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is talking."));
        }

        public static async Task Thanks(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} thanks {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is grateful."));
        }

        public static async Task Threaten(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} threatens {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} is threatening."));
        }

        public static async Task Wave(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} waves at {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} waves."));
        }

        public static async Task Yes(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} agrees with {args}."));
            else
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} agrees."));
        }

        public static async Task Me(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} {args}."));
            else
                user.Send(new ServerSendMessage($"Usage: /me <whatever you like>"));
        }

        public static async Task Bite(UserSession user, UserSessionList userList, string args)
        {
            if (!string.IsNullOrEmpty(args))
                await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} bites {args}'s ankles."));
            else
                await userList.Broadcast(new ServerSendMessage($"Watch out! {user.CharacterName} is going to start biting the nearest ankle."));
        }
    }
}

/*private readonly string[] _commands =
{
"beckon",
"bow",
"cheer",
"cower",
"crossarms",
"cry",
"dance",
"facepalm",
----- "upset",
"geargrind",
"gg",
"kneel",
"laugh",
"no",
"playdead",
"point",
"ponder",
"rockout",
"sad",
"salute",
"shiver",
"shrug",
"shuffle",
"sit",
"sleep",
------"nap",
"step",
"surprised",
"talk",
"thanks",
------"thank",
------"thk",
------"ty",
"threaten",
"wave",
"yes",
"me",
"bite",
------"biteankle",
"list",
"help",

};

[Command("Test")]
public string[] GetCommands()
{
return _commands;
}

public bool Run(string command, string[] args)
{
throw new NotImplementedException();
}*/
