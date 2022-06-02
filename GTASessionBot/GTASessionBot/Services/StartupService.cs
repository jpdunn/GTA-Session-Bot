using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;


namespace GTASessionBot.Services {

    public class StartupService {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly Configuration.Configuration _config;


        private List<string> statusMessages = new List<string> {
            "Is this thing on?",
            "Joining IGN",
            "What is Discord?",
            ":Sweats:",
            "The waiting game",
            "Purging friends list",
            "Regretting life decisions",
            "Doing bot things",
            "Fixing Typos",
            "Visual Studio 2017",
            "Can I sleep yet?",
            "The Vespucci Job",
            "Bowling with Roman"
            };


        /// <summary>
        /// Creates a new <see cref="StartupService"/>.
        /// </summary>
        /// <param name="discord">The Discord socket client to use.</param>
        /// <param name="commands">The command service to use.</param>
        /// <param name="config">The configuration object to use.</param>
        public StartupService(
            DiscordSocketClient discord,
            CommandService commands,
            Configuration.Configuration config
        ) {
            _config = config;
            _discord = discord;
            _commands = commands;
        }


        /// <summary>
        /// Starts the bot.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        public async Task StartAsync() {
            string discordToken;
            Func<Task> clientReady;


            discordToken = _config.DiscordToken;

            if (string.IsNullOrWhiteSpace(discordToken)) {
                throw new Exception(
                    "Please enter your bot's token into the `_configuration.json` file found in the applications root directory."
                );
            }

            if (_config.DebugLogging) {
                ConfigureDebugLogging();
            }

            clientReady = async () => {
                Assembly assembly;
                FileVersionInfo fileVersionInfo;
                string version;


                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Client ready");
                Console.WriteLine("--------------------------------------------------");

                assembly = Assembly.GetExecutingAssembly();
                fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                version = fileVersionInfo.FileVersion;

                // Log the version of the assembly out as well just to give an indicator of updated versions.
                Console.WriteLine($"GTA Session Bot version {version}");
                Console.WriteLine("--------------------------------------------------");

                await _discord.SetGameAsync(GetRandomStatusMessage(), type: ActivityType.Listening);
            };

            _discord.Ready += clientReady;

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            // Load commands and modules into the command service.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }


        /// <summary>
        /// Gets a random string to be used for the status.
        /// </summary>
        /// <returns>A random string to be used for the status.</returns>
        private string GetRandomStatusMessage() {
            Random r = new Random();
            int rInt = r.Next(0, statusMessages.Count);

            return statusMessages[rInt];
        }


        /// <summary>
        /// Configures the Discord client to display debug logging information.
        /// </summary>
        private void ConfigureDebugLogging() {
            Func<Exception, Task> disconnected;
            Func<Task> connected;
            Func<Task> loggedOut;
            Func<Task> loggedIn;
            Func<int, int, Task> latencyUpdated;


            disconnected = e => {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"Gateway disconnected. Exception data: {e.Message}");
                Console.WriteLine("--------------------------------------------------");
                return Task.CompletedTask;
            };

            connected = () => {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Gateway connected");
                Console.WriteLine("--------------------------------------------------");
                return Task.CompletedTask;
            };

            loggedOut = () => {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Logged out");
                Console.WriteLine("--------------------------------------------------");
                return Task.CompletedTask;
            };

            loggedIn = () => {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Logged in");
                Console.WriteLine("--------------------------------------------------");
                return Task.CompletedTask;
            };

            latencyUpdated = (old, updated) => {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"Latency updated from {old}ms to {updated}ms");
                Console.WriteLine("--------------------------------------------------");
                return Task.CompletedTask;
            };

            _discord.Disconnected += disconnected;
            _discord.Connected += connected;
            _discord.LoggedOut += loggedOut;
            _discord.LoggedIn += loggedIn;
            _discord.LatencyUpdated += latencyUpdated;
        }

    }
}
