# EdgyBot
The handy and all around fun bot that can be used for any kind of server!

[![Discord Bots](https://discordbots.org/api/widget/373163613390897163.svg)](https://discordbots.org/bot/373163613390897163)

## Running

 To use/run EdgyBot, create a class called LoginInfo.cs with the following variables:

 * string token
 * string prefix 
 * string invLink
 * string GJP
 * string accID
 * string ownerDiscordName
 * ulong ownerID
 * int defAddedXP
 
 ## Example
 ```cs
 namespace EdgyBot
{
    public class LoginInfo
    {
		public string token = "mytopsecrettoken";
		public string prefix = "e!";
		public string invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=373163613390897163";
		public string GJP = "topsecretGJP";
		public string accID = "gdaccID";
		public string ownerDiscordName = "MyName#1234";
		public ulong ownerID = supercoolID;
		public int startersXP = 0;
		public int defAddedXP = 275;
    }
}
 ```
