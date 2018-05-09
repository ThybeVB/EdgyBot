# EdgyBot
[![Discord Bots](https://discordbots.org/api/widget/status/373163613390897163.svg)](https://discordbots.org/bot/373163613390897163)
[![Discord Bots](https://discordbots.org/api/widget/servers/373163613390897163.svg)](https://discordbots.org/bot/373163613390897163)

The handy and all around fun bot that can be used for any kind of server!

## Running

 To use/run EdgyBot, create a class called LoginInfo.cs with the following variables:

 * string token, The token for your bot used to connect to Discord.
 * string dblToken, (Optional) Your API token for the Discord Bot List.
 * string prefix, The prefix used for the bot.
 * string invLink, The invite link to invite the bot to other guilds.
 * string GJP, (Optional) Geometry Jump Password, used for the Geometry Dash Commands.
 * string accID, (Optional) Geometry Dash Account ID used for the Geometry Dash Commands.
 * ulong ownerID, The Discord ID of the bot owner.
 
 ## Example
 ```cs
 namespace EdgyCore
{
    public class LoginInfo
    {
		public string token = "mytopsecrettoken";
		public string dblToken = "discord bot list token"; (OPTIONAL)
		public string prefix = "e!";
		public ulong clientID = 123456789;
		public string invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=123456789"; (OPTIONAL)
		public string accID = "gdaccID"; (Optional)
		public ulong ownerID = supercoolID;
    }
}
 ```