﻿using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class EdgyBot
    {
        private static void Main(string[] args)
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose});
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        private async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, _lib.GetToken());
            await Client.StartAsync();
            await _handler.InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}