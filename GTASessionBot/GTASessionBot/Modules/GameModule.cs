using Discord;
using Discord.Commands;
using GTASessionBot.Preconditions;
using GTASessionBot.Providers;
using GTASessionBot.Utilities;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {
    [Group("game"), Name("Game")]
    [Summary("Controls for the GTA game itself.")]
    public class GameModule : ModuleBase<SocketCommandContext> {

        private readonly Configuration.Configuration _config;
        private readonly ScreenshotManager _screenshotManager;
        private readonly ScreenshotProvider _screenshotProvider;


        public GameModule(
            Configuration.Configuration config,
            ScreenshotManager screenshotManager,
            ScreenshotProvider screenshotProvider
        ) {
            _config = config;
            _screenshotManager = screenshotManager;
            _screenshotProvider = screenshotProvider;
        }

        [IsAdmin]
        [Command("exit")]
        [Summary("Exits GTA via brute force.")]
        public async Task ShutdownAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":video_game: Exiting game", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                ProcessProvider provider;


                provider = new ProcessProvider();
                embed = EmbedHelper.GetDefaultEmbed(Context);
                embed.AddField(":robot: Stopping bot process", "Please wait...");

                //_screenshotManager.StopTimer();
                _screenshotProvider.Stop();

                embed.AddField(":white_check_mark: Success", "The bot process has been stopped.");

                // Forcibly kill the GTA process.
                KillGTAProcess();

                embed.AddField("Process Killed", "The GTA 5 process is no longer running.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }

        [IsAdmin]
        [Command("start")]
        [Summary("Starts GTA via Steam or Social Club depending on what is installed.")]
        public async Task StartGameAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":video_game: Starting game", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                ProcessProvider provider;


                provider = new ProcessProvider();
                embed = EmbedHelper.GetDefaultEmbed(Context);

                StartGTAProcess();

                embed.AddField("Game started", "The GTA 5 game has been started successfully.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("restart")]
        [Summary("Restarts GTA in a very ungraceful way.")]
        public async Task RestartGameAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":video_game: Restarting game", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                embed.AddField(":robot: Stopping idle script", "Please wait...");

                _screenshotProvider.Stop();

                embed.AddField(":white_check_mark: Success", "The bot process has been stopped.");
                embed.AddField("Stopping GTA", "Please wait...");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

                KillGTAProcess();

                embed = EmbedHelper.GetDefaultEmbed(Context);
                embed.AddField("Starting GTA", "Please wait...");

                if (_config.IsSteamGame) {
                    // TODO: This feels nasty, and really is nasty. Wait for 20 seconds until Steam is able to sync
                    // the game data. Eventually this should be done by hooking into Steam so that we can know for sure
                    // when the game sync has been completed.
                    Thread.Sleep(20000);
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

                StartGTAProcess();

                embed = EmbedHelper.GetDefaultEmbed(Context);
                embed.AddField("Game started", "The GTA 5 game has been restarted successfully.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("kill launcher")]
        [Summary("Kills the GTA Launcher forcefully.")]
        public async Task KillLauncherAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":gun: Killing GTA Launcher", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                Process launcherProcess;
                ProcessProvider provider;
                EmbedBuilder embed;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ProcessProvider();
                launcherProcess = provider.GetGrandTheftAutoLauncherProcess();

                if (launcherProcess != null) {
                    launcherProcess.Kill();
                } else {
                    EventLogLogger.LogWarning("Attempted to kill the GTA Launcher process but the process could not be found.");
                }

                embed.AddField(":white_check_mark: Success", "The launcher process has been killed.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("kill error")]
        [Summary("Kills the Windows error dialog forcefully.")]
        public async Task KillErrorReporterAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":gun: Killing Error Reporter", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                Process errorReporter;
                ProcessProvider provider;
                EmbedBuilder embed;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ProcessProvider();
                errorReporter = provider.GetWindowsErrorReporterProcess();

                if (errorReporter != null) {
                    errorReporter.Kill();
                } else {
                    EventLogLogger.LogWarning("Attempted to kill the Windows error reporter process but the process could not be found.");
                }

                embed.AddField(":white_check_mark: Success", "The Windows error reporter process has been killed.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [Command("list")]
        [Summary("Grabs the player list.")]
        public async Task GrabPlayerList() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":camera_with_flash: Capturing Player List", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                Byte[] imageBytes;
                ScreenCaptureProvider provider;
                EmbedBuilder embed;
                ISupportedImageFormat format;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ScreenCaptureProvider(_config);

                format = new JpegFormat { Quality = 50 };
                imageBytes = provider.MergePlayerList();

                using (var stream = new MemoryStream(imageBytes)) {
                    using (MemoryStream outStream = new MemoryStream()) {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true)) {
                            imageFactory.Load(stream)
                                        .Format(format)
                                        .Save(outStream);
                        }
                        TimeSpan span;
                        string dateString;


                        span = (DateTime.Now - File.GetLastWriteTimeUtc(Common.PlayerListFirstScreenshotPath));
                        dateString = "";

                        if (span.Days > 0) {
                            dateString += $"{span.Days} days, ";
                        }

                        if (span.Hours > 0) {
                            dateString += $"{span.Hours} hours, ";
                        }

                        if (span.Minutes > 0) {
                            dateString += $"{span.Minutes} minutes, ";
                        }

                        dateString += $"{span.Seconds} seconds";
                        embed.AddField(":pencil: Current Player List", $"List age: {dateString} old");

                        await Context.Channel.SendFileAsync(outStream, "players.jpg");
                    }
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });
            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        /// <summary>
        /// Forcibly kills the GTA process and the associated launcher process. 
        /// Throws argument exceptions if the process are unable to be found, 
        /// or if they are still alive after being killed.
        /// </summary>
        private void KillGTAProcess() {
            Process process;
            Process launcherProcess;
            ProcessProvider provider;
            int timeout;


            provider = new ProcessProvider();
            process = provider.GetGrandTheftAutoProcess();
            timeout = 0;

            if (process != null) {
                process.Kill();
            } else {
                throw new ApplicationException("Unable to locate GTA V process.");
            }

            do {
                // While the process has not exited, wait for a bit and then check again. 
                // If we wait for a period of 30 seconds then exit out of the loop and 
                // throw an exception because we do not want to end up in an infinite loop.
                Thread.Sleep(1000);
                timeout++;
            } while (!process.HasExited && (timeout < 30));

            if (timeout >= 30) {
                // If we hit the timeout, throw an exception and log the error.
                throw new ApplicationException("Timed out trying to exit the GTA V process");
            }

            // TODO: This is nasty, really nasty, but Rockstar can't program 
            // a game properly so I guess this is what we're left with. The launcher process
            // acts as a giant try-catch statement around the entire of GTA, so if GTA crashes
            // or is killed by force, the launcher process will pop up with the usual 'safe mode' 
            // message.
            //
            // The problem with this is that the launcher needs time to start up, as it seems as if 
            // it sets a flag of some kind to tell itself that it has already prompted the user with 
            // the 'safe mode' warning message. If the launcher process never has time to initialize when
            // the game is killed, then it never gets to set the flag to say it warned the user. 
            // As a result of this, the next time the game is started, because it relies on the launcher
            // process to instance the game, it will show up the 'safe mode' dialog and stop us from 
            // launching the game until the dialog is dealt with.
            Thread.Sleep(5000);

            // Get the launcher process, this is needed because GTA is special and will try to 
            // catch all exceptions that occur, and pop up an annoying 'Safe Mode' dialog. This is
            // annoying and useless, we have no other way to stop GTA quickly other than a force 
            // quit so this dialog is expected. If we find it, kill it. It serves no purpose for us.
            launcherProcess = provider.GetGrandTheftAutoLauncherProcess();

            if (launcherProcess != null) {
                launcherProcess.Kill();
            }
        }


        /// <summary>
        /// Starts the GTA process.
        /// </summary>
        private void StartGTAProcess() {
            Process process;
            ProcessProvider provider;


            provider = new ProcessProvider();

            if (provider.GetGrandTheftAutoProcess() != null) {
                EventLogLogger.LogError("Unable to start the GTA V process as it is already running.");
                throw new ApplicationException("Unable to start the GTA V process as it is already running.");
            }

            if (_config.IsSteamGame) {
                string steamPath;


                steamPath = RegistryHelper.GetSteamPath();

                if (!string.IsNullOrEmpty(steamPath)) {
                    process = new Process();
                    process.StartInfo.FileName = $"{steamPath}\\Steam.exe";
                    process.StartInfo.Arguments = "-applaunch 271590 -StraightIntoFreemode";
                    process.Start();

                } else {
                    throw new ArgumentException("Unable to find the Steam installation path in the registry.");
                }
            } else {
                string installPath;


                installPath = RegistryHelper.GetGTASocialClubInstallPath();

                if (!string.IsNullOrEmpty(installPath)) {
                    process = new Process();
                    process.StartInfo.FileName = $"{installPath}\\PlayGTAV.exe";
                    process.StartInfo.Arguments = "-StraightIntoFreemode";
                    process.Start();
                } else {
                    throw new ArgumentException("Unable to find the PlayGTAV installation path in the registry.");
                }
            }

        }

    }
}
