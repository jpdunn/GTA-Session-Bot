using Discord;
using Discord.Commands;
using GTASessionBot.Preconditions;
using GTASessionBot.Providers;
using GTASessionBot.Utilities;
using GTASessionBot.Windows_Libraries;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {

    /// <summary>
    /// Defines a module for keyboard related actions.
    /// </summary>
    [Group("keyboard"), Name("Keyboard"), Alias("kb")]
    [Summary("Everything keyboard related.")]
    public class KeyboardModule : ModuleBase<SocketCommandContext> {


        /// <summary>
        /// Uses an AutoHotKey script to press the `Enter` key.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [IsAdmin]
        [Command("enter")]
        [Summary("Simulates a press of the 'enter' key on the keyboard.")]
        public async Task PressEnterAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":keyboard: Pressing buttons", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            try {
                EmbedBuilder embed;
                string path;
                Process process;
                ProcessStartInfo startInfo;


                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                embed = EmbedHelper.GetDefaultEmbed(Context);

                startInfo = new ProcessStartInfo();
                startInfo.FileName = $"{path}\\enter.exe";

                process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                embed.AddField("Buttons Pressed", "IDK what to put here, I literally just pressed 1 key.");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });
            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        /// <summary>
        /// Simulates the key presses needed to enter GTA Online via the in game menu,
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [IsAdmin]
        [Command("online")]
        [Summary("Simulates the key presses needed to enter GTA Online via the game menu.")]
        public async Task EnterGTAOnlineAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;
            ProcessProvider provider;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":keyboard: Pressing buttons", "Please wait...");
            provider = new ProcessProvider();

            response = await ReplyAsync("", embed: initialBuilder.Build());

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            try {
                EmbedBuilder embed;
                string path;
                Process process;
                ProcessStartInfo startInfo;
                IntPtr windowHandle;


                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                embed = EmbedHelper.GetDefaultEmbed(Context);

                startInfo = new ProcessStartInfo();
                startInfo.FileName = $"{path}\\enterOnline.exe";

                process = new Process();
                process.StartInfo = startInfo;

                windowHandle = provider.GetGrandTheftAutoProcessPointer();

                User32.SetForegroundWindow(windowHandle);

                process.Start();

                embed.AddField("Buttons Pressed", "IDK what to put here, things are happening I guess?");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }
    }
}
