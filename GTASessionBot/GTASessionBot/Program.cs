using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GTASessionBot.Providers;
using GTASessionBot.Services;
using GTASessionBot.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GTASessionBot {
    class Program {
        private CommandService _commands;
        private DiscordSocketClient _client;
        private Configuration.Configuration _config;


        public static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();


        public async Task StartAsync() {

            IServiceCollection services;
            IServiceProvider provider;

            try {
                _config = Configuration.ConfigurationReader.Load();
            } catch (Exception e) {
                // Catch the error and show a dialog. The error will be logged when trying to load the config
                // file anyway, so all we need to do here is to ensure that the user knows why the application crashed.
                MessageBox.Show(
                    $"An error occurred whilst trying to load the configuration file. Please ensure the configuration file exists and is named correctly. {e.Message}",
                    "Invalid Configuration File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }


            _commands = new CommandService(
                new CommandServiceConfig {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose
                }
            );

            _client = new DiscordSocketClient(
                new DiscordSocketConfig {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }
            );

            // Ensure that the event log source we are going
            // to write to exists.
            EventLogLogger.EnsureExists();

            // Add singletons of all the services we will need.
            services = new ServiceCollection()
                 .AddSingleton(_client)
                 .AddSingleton(_commands)
                 .AddSingleton<CommandHandler>()
                 .AddSingleton<LoggingService>()
                 .AddSingleton<StartupService>()
                 .AddSingleton<ScreenshotManager>()
                 .AddSingleton<ScreenshotProvider>()
                 .AddSingleton<PermissionManager>()
                 .AddSingleton<Random>()
                 .AddSingleton(_config);

            // Create the service provider.
            provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);

            // Initialize all the services.
            provider.GetRequiredService<LoggingService>();
            await provider.GetRequiredService<StartupService>().StartAsync();
            provider.GetRequiredService<CommandHandler>();
            provider.GetRequiredService<ScreenshotManager>();

            // Prevent the application from closing.
            await Task.Delay(-1);
        }
    }
}
