using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.Commands;
using EdgyBot.Database;
using EdgyBot.Core.Lib;
using EdgyBot.Core;

namespace EdgyBot.Modules
{
    [Name("Utility Commands"), Summary("Commands that help manage the server!")]
    public class UtilityCommands : ModuleBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("enablecommand", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Name("enablecommand"), Summary("Enables a command that was previously disabled in the Guild/Server")]
        public async Task EnableCommandCmd ([Remainder]string query)
        {
            CommandInfo cmd = HelpCommand._service.Commands.FirstOrDefault(x => x.Name == query);
            if (cmd == null) {
                await ReplyAsync((string)Context.Language["util"]["cmdNotFound"]);
                return;
            }

            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.EnableCommand(cmd.Name);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["util"]["utilCmds"], $"{Context.Language["util"]["enableCmdSuccess"]} ``{cmd.Name}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync(Context.Language["util"]["enableCmdFail"] + e.Message);
                }
                return;
            }

            await ReplyAsync((string)Context.Language["util"]["openConnFail"]);
        }

        //[Command("disabledcommands", RunMode = RunMode.Async)]
        //[Name("disabledcommands"), Summary("The disabled commands for the guild")]
        //public async Task DisabledCommandsCmd () 
        //{
        //    DatabaseConnection connection = new DatabaseConnection();
        //    await connection.ConnectAsync();
        //
        //    if (connection.OpenConnection()) 
        //    {
        //        try {
        //            Guild guild = new Guild(Context.Guild.Id);
        //            string[] split  = await guild.GetDisabledCommands();
        //
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("```");
        //            foreach (string entry in split) {
        //                sb.Append($"{entry}\n");
        //            }
        //            sb.Append("```");
        //
        //            await ReplyAsync(sb.ToString());
        //            
        //        } catch (Exception e) {
        //            await ReplyAsync("Error: " + e.Message);
        //        }
        //    }
        //}
        //Put on hold

        [Command("disablecommand", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Name("disablecommand"), Summary("Disables a command from being used in the Guild/Server")]
        public async Task DisableCommandCmd ([Remainder]string query)
        {
            CommandInfo cmd = HelpCommand._service.Commands.FirstOrDefault(x => x.Name == query);
            if (cmd == null) {
                await ReplyAsync((string)Context.Language["util"]["invalidCmd"]);
                return;
            } else if (cmd.Name == "disablecommand") {
                await ReplyAsync((string)Context.Language["util"]["disableFail"]);
                return;
            }

            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.DisableCommand(cmd.Name);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["util"]["disableSuccess"], $"{Context.Language["util"]["disableSuccess"]} ``{cmd.Name}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync((string)Context.Language["util"]["disableFail"] + e.Message);
                }

                return;
            }

            await ReplyAsync((string)Context.Language["util"]["openConnFail"]);
        }

        [Command("setprefix")]
        [Name("setprefix"), Summary("Sets the prefix used for the server.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefixCmd([Remainder]string newPrefix)
        {
            DatabaseConnection connection = new DatabaseConnection("EdgyBot.db");
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.ChangePrefix(newPrefix);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["util"]["utilCmds"], $"{Context.Language["util"]["prefixSet"]} ``{newPrefix}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync((string)Context.Language["util"]["prefixError"] + e.Message);
                }

                return;
            }

            await ReplyAsync((string)Context.Language["util"]["openConnFail"]);
        }

        [Command("purge", RunMode = RunMode.Async), Name("purge"), Summary("Deletes messages from said channel (Provide a number on how much messages to delete)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeCmd (int input) 
        {

            if (input > 100) {
                await ReplyAsync((string)Context.Language["util"]["purgeMax"]);
                return;
            }
            if (input <= 0) {
                await ReplyAsync((string)Context.Language["util"]["purgeInvalid"]);
                return;
            }

            try 
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(input + 1).FlattenAsync();

                ITextChannel channel = (ITextChannel)Context.Channel;
                ChannelPermissions channelPermissions = Context.Guild.CurrentUser.GetPermissions(channel);

                if (channelPermissions.ManageMessages) {
                    await channel.DeleteMessagesAsync(messages);
                    await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["util"]["purge"], (string)Context.Language["util"]["successPurge1"] + (input + 1) + (string)Context.Language["util"]["successPurge2"]));
                } else {
                    await ReplyAsync("", embed: _lib.CreateEmbedWithError((string)Context.Language["util"]["purgeError"], (string)Context.Language["util"]["purgePermFail"]));
                }

            } catch (Exception e)
            {
                Embed err = _lib.CreateEmbedWithError((string)Context.Language["util"]["purgeError"], $"**Error**: {e.Message}");
                await ReplyAsync("", embed: err);
            }
        }

        [Command("kick")][Name("kick")][Summary("Kicks a user from the guild")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickCmd (IGuildUser usr, [Remainder]string reason = null)
        {
            Embed e = null;
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    await usr.KickAsync();
                } else
                {
                    await usr.KickAsync(reason);
                }
                e = _lib.CreateEmbedWithText((string)Context.Language["util"]["ebAdminCmds"], Context.Language["util"]["kickedUser"] + usr.Username + ": " + reason);
            } catch
            {
                e = _lib.CreateEmbedWithError((string)Context.Language["util"]["ebAdminCmdsError"], $":exclamation: *{Context.Language["util"]["ebKickFail"]}*");
            }
            await ReplyAsync("", embed: e);
        }

        [Command("ban")][Name("ban")][Summary("Bans a user from the Guild. Optionally, You can provide a reason.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanCmd (IGuildUser usr, [Remainder]string reason = null)
        {
            bool hasReason = true;
            if (reason == null) hasReason = false;
            Embed e = null;
            try
            {
                if (hasReason)
                {
                    await usr.Guild.AddBanAsync(usr, reason: reason);
                } else await usr.Guild.AddBanAsync(usr);
                e = _lib.CreateEmbedWithText((string)Context.Language["util"]["ebAdminCmds"], $"{(string)Context.Language["util"]["banSuccess"]} {usr.Username}!");
            } catch
            {
                e = _lib.CreateEmbedWithError((string)Context.Language["util"]["ebAdminCmdsError"], $":exclamation: *{(string)Context.Language["util"]["banFail"]}*");
            }
            await ReplyAsync("", embed: e);
        }
    }
}
