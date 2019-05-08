using Discord;
using Discord.Commands;
using GTASessionBot.Preconditions;
using GTASessionBot.Providers;
using GTASessionBot.Utilities;
using GTASessionBot.Windows_Libraries;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {

    [Name("Management")]
    [Summary("Manage a session host.")]
    public class ManagementModule : ModuleBase<SocketCommandContext> {

        public Configuration.Configuration _config { get; }
        private readonly ScreenshotManager _screenshotManager;
        private readonly ScreenshotProvider _screenshotProvider;


        public ManagementModule(
            Configuration.Configuration config,
            ScreenshotManager screenshotManager,
            ScreenshotProvider screenshotProvider
            ) {
            _config = config;
            _screenshotManager = screenshotManager;
            _screenshotProvider = screenshotProvider;
        }


        [IsAdmin]
        [Command("newsession")]
        [Alias("split", "solo")]
        [Summary("Splits the session host into a new solo session.")]
        public async Task FindNewSessionAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":mag: Finding new session", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                IntPtr windowHandle;
                ProcessProvider provider;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ProcessProvider();

                embed.AddField(":robot: Stopping bot process", "Please wait...");

                _screenshotProvider.Stop();

                embed.AddField(":white_check_mark: Success", "The bot process has been stopped.");
                embed.AddField(":mag: Finding new session", "Please wait...");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

                windowHandle = provider.GetGrandTheftAutoProcessPointer();

                User32.GetWindowThreadProcessId(windowHandle, out uint processID);

                if (processID != 0) {
                    Process process;


                    process = Process.GetProcessById((int)processID);

                    process.Suspend();
                    Thread.Sleep(8000);
                    process.Resume();
                }


                embed.AddField(":white_check_mark: Success", "A new session has been found.");
                embed.AddField(":robot: Starting bot script", "Please wait");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

                _screenshotProvider.Start();

                embed.AddField(":white_check_mark: Success", "Bot script started ");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });
            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        [IsAdmin]
        [Command("foreground")]
        [Alias("front")]
        [Summary("Brings the GTA process to the foreground.")]
        public async Task BringWindowToForegroundAsync() {

            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField("Bringing GTA Process to foreground", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                IntPtr windowHandle;
                ProcessProvider provider;
                EmbedBuilder embed;
                Stopwatch watch;


                watch = new Stopwatch();
                watch.Start();

                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ProcessProvider();

                windowHandle = provider.GetGrandTheftAutoProcessPointer();

                User32.SetForegroundWindow(windowHandle);
                watch.Stop();

                embed.AddField(":white_check_mark: Success", "GTA Process is in foreground.");
                embed.AddField(":alarm_clock: Time taken to complete action", watch.Elapsed.ToString("mm'm 'ss's 'fff'ms'"));

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });
            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("stop idle")]
        [Summary("Stops the idle script.")]
        public async Task StopScriptAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":robot: Stopping idle script", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Stopwatch watch;


                watch = new Stopwatch();
                watch.Start();

                embed = EmbedHelper.GetDefaultEmbed(Context);
                _screenshotProvider.Stop();

                watch.Stop();

                embed.AddField(":white_check_mark: Success", "The idle script has been stopped.");
                embed.AddField(":alarm_clock: Time taken to complete action", watch.Elapsed.ToString("mm'm 'ss's 'fff'ms'"));

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("start idle")]
        [Summary("Starts the idle script.")]
        public async Task StartScriptAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":robot: Starting idle script", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Stopwatch watch;


                watch = new Stopwatch();
                watch.Start();

                embed = EmbedHelper.GetDefaultEmbed(Context);

                _screenshotProvider.Start();

                watch.Stop();

                embed.AddField(":white_check_mark: Success", "The idle script has been started.");
                embed.AddField(":alarm_clock: Time taken to complete action", watch.Elapsed.ToString("mm'm 'ss's 'fff'ms'"));

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        [IsAdmin]
        [Command("status")]
        [Summary("Uploads a screenshot of the host machine. Useful for determining if the GTA process is not in the foreground, or has an error.")]
        public async Task GetScreenStatusAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":camera_with_flash: Uploading screenshot", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                ScreenCaptureProvider provider;
                IntPtr windowHandle;
                EmbedBuilder embed;
                Stopwatch watch;
                Stopwatch uploadWatch;
                ISupportedImageFormat format;
                TextLayer watermark;
                Byte[] imageBytes;


                watch = new Stopwatch();
                uploadWatch = new Stopwatch();

                watch.Start();

                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ScreenCaptureProvider(_config);

                // Get the desktop window rather than the GTA window, if there is an application
                // blocking the GTA window then taking a screenshot of the GTA window won't really
                // show any problems. Getting the desktop window will grab the currently active desktop.
                windowHandle = User32.GetDesktopWindow();

                format = new JpegFormat { Quality = 50 };
                imageBytes = provider.CaptureWindow(windowHandle);

                watermark = BuildWatermark(windowHandle);

                using (var stream = new MemoryStream(imageBytes)) {
                    using (MemoryStream outStream = new MemoryStream()) {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true)) {
                            imageFactory.Load(stream)
                                        .Format(format)
                                        .Watermark(watermark)
                                        .Save(outStream);
                        }

                        watch.Stop();

                        embed.AddField(":camera_with_flash: Success", "Current status of the host machine is shown below.");
                        embed.AddField(":alarm_clock: Action time", watch.Elapsed.ToShortForm(), inline: true);

                        uploadWatch.Start();

                        await Context.Channel.SendFileAsync(outStream, "window.jpg");
                    }
                }

                uploadWatch.Stop();
                embed.AddField(":alarm_clock: Upload time", uploadWatch.Elapsed.ToShortForm(), inline: true);

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [Command("version")]
        [Summary("Gets the version number of the bot.")]
        public async Task GetVersionAsync() {

            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":robot: Starting bot script", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Assembly assembly;
                FileVersionInfo fileVersionInfo;
                string version;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                assembly = Assembly.GetExecutingAssembly();
                fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                version = fileVersionInfo.FileVersion;

                embed.AddField(":cheese: Session Bot Version", version);

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("desktop")]
        [Summary("Sets the desktop to be the active window.")]
        public async Task SwitchToDesktopAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":desktop: Switching to desktop", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                ScreenCaptureProvider provider;
                IntPtr windowHandle;
                EmbedBuilder embed;
                const int WM_COMMAND = 0x111;
                const int MIN_ALL = 419;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                provider = new ScreenCaptureProvider(_config);
                windowHandle = User32.GetDesktopWindow();

                IntPtr lHwnd = User32.FindWindow("Shell_TrayWnd", null);
                User32.SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);

                User32.SetForegroundWindow(windowHandle);

                embed.AddField(":white_check_mark: Success", "The desktop should now be in focus.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        private TextLayer BuildWatermark(IntPtr windowHandle) {
            TextLayer watermark;
            Rect rect;
            int width;
            int height;


            rect = new Rect();

            User32.GetWindowRect(windowHandle, ref rect);

            width = rect.right - rect.left;
            height = rect.bottom - rect.top;

            watermark = new TextLayer {
                Text = "© GTA Session Bot",
                Position = new System.Drawing.Point(width - 480, height - 60),
                FontColor = System.Drawing.Color.White,
                FontSize = 50,
                Opacity = 75
            };


            return watermark;
        }


        /// <summary>
        /// Gives a cookie to the current user.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [Command("cookie")]
        public async Task GiveCookie() {
            await ReplyAsync($"Here {Context.User.Mention}, have a cookie :cookie:");
        }


        [Command("sleezy")]
        public async Task MentionSleezy() {
            await ReplyAsync($"What the fuck do you want from me? {Context.User.Mention}");
        }


        /// <summary>
        /// Gives a cookie to the tagged user.
        /// </summary>
        /// <param name="username">The user to give a cookie to.</param>
        /// <returns>An awaitable task.</returns>
        [Command("cookie")]
        public async Task GiveCookie(string username) {
            await ReplyAsync($"Here {username}, have a cookie :cookie:");
        }
    }
}
