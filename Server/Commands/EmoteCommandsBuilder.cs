using System.Threading.Tasks;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
   internal static class EmoteCommandsBuilder
   {
      internal static ChatCommandHandler BuildEmoteCommands(this ChatCommandHandler handler)
      {
         handler.RegisterCommand(new TargetableEmoteCommand("beckon", (UserSession user) => $"{user.CharacterName} beckons.", (UserSession user, string args) => $"{user.CharacterName} beckons to {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("bow", (UserSession user) => $"{user.CharacterName} bows.", (UserSession user, string args) => $"{user.CharacterName} bows to {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("cheer", (UserSession user) => $"{user.CharacterName} cheers.", (UserSession user, string args) => $"{user.CharacterName} cheers for {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("cower", (UserSession user) => $"{user.CharacterName} cowers.", (UserSession user, string args) => $"{user.CharacterName} cowers in fear from {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("laugh", (UserSession user) => $"{user.CharacterName} laughs.", (UserSession user, string args) => $"{user.CharacterName} laughs at {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("no", (UserSession user) => $"{user.CharacterName} disagrees.", (UserSession user, string args) => $"{user.CharacterName} disagrees with {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("point", (UserSession user) => $"{user.CharacterName} points.", (UserSession user, string args) => $"{user.CharacterName} points at {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("salute", (UserSession user) => $"{user.CharacterName} salutes.", (UserSession user, string args) => $"{user.CharacterName} salutes {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("shrugs", (UserSession user) => $"{user.CharacterName} shrugs.", (UserSession user, string args) => $"{user.CharacterName} shrugs at {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("surprised", (UserSession user) => $"{user.CharacterName} is surprised.", (UserSession user, string args) => $"{user.CharacterName} is surprised by {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("talk", (UserSession user) => $"{user.CharacterName} is talking.", (UserSession user, string args) => $"{user.CharacterName} is talking to {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand(new string[] { "thanks", "thank", "thk", "ty" }, (UserSession user) => $"{user.CharacterName} is grateful.", (UserSession user, string args) => $"{user.CharacterName} thanks {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("threaten", (UserSession user) => $"{user.CharacterName} is threatening.", (UserSession user, string args) => $"{user.CharacterName} threatens {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("wave", (UserSession user) => $"{user.CharacterName} waves.", (UserSession user, string args) => $"{user.CharacterName} waves at {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand("yes", (UserSession user) => $"{user.CharacterName} agrees.", (UserSession user, string args) => $"{user.CharacterName} agrees with {args}."));
         handler.RegisterCommand(new TargetableEmoteCommand(new string[] { "bite", "biteankle" }, (UserSession user) => $"Watch out! {user.CharacterName} is going to start biting the nearest ankle.", (UserSession user, string args) => $"{user.CharacterName} bites {args}'s ankles."));

         handler.RegisterCommand(new EmoteCommand("crossarms", (UserSession user) => $"{user.CharacterName} crosses their arms."));
         handler.RegisterCommand(new EmoteCommand("cry", (UserSession user) => $"{user.CharacterName} is crying."));
         handler.RegisterCommand(new EmoteCommand("dance", (UserSession user) => $"{user.CharacterName} is busting out some moves, some sweet dance moves."));
         handler.RegisterCommand(new EmoteCommand(new string[] { "facepalm", "upset" }, (UserSession user) => $"{user.CharacterName} is upset."));
         handler.RegisterCommand(new EmoteCommand("geargrind", (UserSession user) => $"{user.CharacterName} does the Gear Grind."));
         handler.RegisterCommand(new EmoteCommand("gg", (UserSession user) => $"{user.CharacterName} says \"Good Game\"."));
         handler.RegisterCommand(new EmoteCommand("kneel", (UserSession user) => $"{user.CharacterName} kneels."));
         handler.RegisterCommand(new EmoteCommand("playdead", (UserSession user) => $"{user.CharacterName} is probably dead."));
         handler.RegisterCommand(new EmoteCommand("ponder", (UserSession user) => $"{user.CharacterName} ponders."));
         handler.RegisterCommand(new EmoteCommand("rockout", (UserSession user) => $"{user.CharacterName} is rocking out!"));
         handler.RegisterCommand(new EmoteCommand("sad", (UserSession user) => $"{user.CharacterName} is sad."));
         handler.RegisterCommand(new EmoteCommand("shiver", (UserSession user) => $"{user.CharacterName} shivers."));
         handler.RegisterCommand(new EmoteCommand("shuffle", (UserSession user) => $"{user.CharacterName} does the Inventory Shuffle."));
         handler.RegisterCommand(new EmoteCommand("sit", (UserSession user) => $"{user.CharacterName} sits."));
         handler.RegisterCommand(new EmoteCommand(new string[] { "sleep", "nap" }, (UserSession user) => $"{user.CharacterName} goes to sleep."));
         handler.RegisterCommand(new EmoteCommand("step", (UserSession user) => $"{user.CharacterName} does the Dodge Step."));

         handler.RegisterCommand(new UserChatCommand("me", async (UserSession user, UserSessionList userList, string args) =>
         {
            if (!string.IsNullOrEmpty(args))
               await userList.Broadcast(new ServerSendMessage($"{user.CharacterName} {args}."));
            else
               user.Send(new ServerSendMessage($"Usage: /me <whatever you like>"));
         }));

         return handler;
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
