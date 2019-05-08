using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GTASessionBot.Utilities;
using System;
using System.Threading.Tasks;


namespace GTASessionBot.Services {
    public class CommandHandler {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly Configuration.Configuration _config;
        private readonly IServiceProvider _provider;


        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            Configuration.Configuration config,
            IServiceProvider provider
        ) {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }


        private async Task OnMessageReceivedAsync(SocketMessage s) {
            SocketUserMessage message;


            // Ensure the message is from a user/bot
            message = s as SocketUserMessage;

            // If the message is null, return.
            if (message == null) {
                return;
            }

            // Ignore self when checking commands.
            if (message.Author == _discord.CurrentUser) {
                return;
            }

            SocketCommandContext context;
            int argPos = 0;


            // Create the command context.
            context = new SocketCommandContext(_discord, message);

            // Check if the message has a valid command prefix.
            if (message.HasStringPrefix(_config.Prefix, ref argPos) || message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) {

                // Look at the second character of the command, if it's the same as the prefix then ignore it.
                // This gets around the bot treating a message like '...' as a command and trying to process it.
                if (!string.Equals(message.Content.Substring(1, 1), _config.Prefix)) {
                    IResult result;


                    Console.WriteLine(message.Content.ToString());

                    // Execute the command.
                    result = await _commands.ExecuteAsync(context, argPos, _provider);

                    if (!result.IsSuccess) {
                        if (!string.IsNullOrEmpty(result.ErrorReason)) {
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                        } else {

                            EmbedBuilder embed;


                            embed = new EmbedBuilder();
                            embed.WithColor(EmbedColors.GetErrorColor());
                            embed.AddField(":warning: An unexpected error occurred.", $"The command: '{message.Content}' is not a registered command.");

                            // If not successful, reply with the error.
                            await context.Channel.SendMessageAsync("", embed: embed.Build());
                        }
                    }
                }
            }
        }
    }
}
