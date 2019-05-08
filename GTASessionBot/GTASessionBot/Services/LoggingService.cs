using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;


namespace GTASessionBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string _logDirectory { get; }
        private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");


        /// <summary>
        /// Creates a new <see cref="LoggingService"/>.
        /// </summary>
        /// <param name="discord">The Discord socket client to use.</param>
        /// <param name="commands">The command service to use.</param>
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            _discord = discord;
            _commands = commands;

            _discord.Log += OnLogAsync;
            _commands.Log += OnCommandLogAsync;
        }


        /// <summary>
        /// Logs the given log message to the console.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <returns>An awaitable task.</returns>
        private Task OnLogAsync(LogMessage msg)
        {
            string logText;


            logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

            return Console.Out.WriteLineAsync(logText);
        }


        /// <summary>
        /// Logs the given command log message to the console.
        /// </summary>
        /// <param name="log">The command log message to log.</param>
        /// <returns>An awaitable task.</returns>
        private Task OnCommandLogAsync(LogMessage log)
        {
            return Console.Out.WriteLineAsync(log.Message);
        }
    }
}
