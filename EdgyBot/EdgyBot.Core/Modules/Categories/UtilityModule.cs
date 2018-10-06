using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.Commands;
using EdgyBot.Database;
using EdgyBot.Core.Lib;

namespace EdgyBot.Modules
{
    [Name("Utility Commands"), Summary("Commands that help manage the server!")]
    public class UtilityCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("enablecommand", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Name("enablecommand"), Summary("Enables a command that was previously disabled in the Guild/Server")]
        public async Task EnableCommandCmd ([Remainder]string query)
        {
            CommandInfo cmd = HelpCommand._service.Commands.FirstOrDefault(x => x.Name == query);
            if (cmd == null) {
                await ReplyAsync("I could not find this command! Please make sure you spelled the command correctly.");
                return;
            }

            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.EnableCommand(Context.Guild.Id, cmd.Name);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Utility Commands", $"Successfully enabled command ``{cmd.Name}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not enable this command. Error: " + e.Message);
                }
                return;
            }

            await ReplyAsync("Could not open a connection to the Database.");
        }

        //[Command("disabledcommands", RunMode = RunMode.Async)]
        //[Name("disabledcommands"), Summary("The disabled commands for the guild")]
        public async Task DisabledCommandsCmd () 
        {
            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection()) 
            {
                try {
                    Guild guild = new Guild(Context.Guild.Id);
                    string[] split  = await guild.GetDisabledCommands();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("```");
                    foreach (string entry in split) {
                        sb.Append($"{entry}\n");
                    }
                    sb.Append("```");

                    await ReplyAsync(sb.ToString());
                    
                } catch (Exception e) {
                    await ReplyAsync("Error: " + e.Message);
                }
            }
        }

        [Command("disablecommand", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Name("disablecommand"), Summary("Disables a command from being used in the Guild/Server")]
        public async Task DisableCommandCmd ([Remainder]string query)
        {
            CommandInfo cmd = HelpCommand._service.Commands.FirstOrDefault(x => x.Name == query);
            if (cmd == null) {
                await ReplyAsync("This is not a valid command!\n*It is possible that you entered the alias for a command with a different name. Please check the help command to see the official command name.*    ");
                return;
            } else if (cmd.Name == "disablecommand") {
                await ReplyAsync("You can not disable this command.");
                return;
            }

            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.DisableCommand(Context.Guild.Id, cmd.Name);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Utility Commands", $"Successfully disabled command ``{cmd.Name}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not disable this command. Error: " + e.Message);
                }

                return;
            }

            await ReplyAsync("Could not open a connection to the Database.");
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

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Utility Commands", $"Guild Prefix set to ``{newPrefix}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not change the server prefix. Error: " + e.Message);
                }

                return;
            }

            await ReplyAsync("Could not open a connection to the Database.");
        }

        [Command("purge", RunMode = RunMode.Async), Name("purge"), Summary("Deletes messages from said channel (Provide a number on how much messages to delete)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeCmd (int input) 
        {

            if (input > 100) {
                await ReplyAsync("You can not delete more than 100 messages at once.");
                return;
            }
            if (input <= 0) {
                await ReplyAsync("What? No.");
                return;
            }

            try 
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(input + 1).FlattenAsync();

                ITextChannel channel = (ITextChannel)Context.Channel;
                ChannelPermissions channelPermissions = Context.Guild.CurrentUser.GetPermissions(channel);

                if (channelPermissions.ManageMessages) {
                    await channel.DeleteMessagesAsync(messages);
                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Purge", "Successfully deleted " + (input + 1) + " messages! :ok_hand:"));
                } else {
                    await ReplyAsync("", embed: _lib.CreateEmbedWithError("Purge Error", "I don't seem to have permissions to delete messages.\nTo Delete messages, i must have the **Manage Messages** permission."));
                }

            } catch (Exception e)
            {
                Embed err = _lib.CreateEmbedWithError("Purge Error", $"**Error**: {e.Message}");
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
                e = _lib.CreateEmbedWithText("EdgyBot Administrative Commands", "Kicked user " + usr.Username + " for reason " + reason);
            } catch
            {
                e = _lib.CreateEmbedWithError("EdgyBot Administrative Commands Error", ":exclamation: *Could not kick this user.*");
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
                e = _lib.CreateEmbedWithText("EdgyBot Administrative Commands", $"Successfully banned user {usr.Username}!");
            } catch
            {
                e = _lib.CreateEmbedWithError("EdgyBot Administrative Commands Error", ":exclamation: *Could not ban this user.*");
            }
            await ReplyAsync("", embed: e);
        }
    }
}
