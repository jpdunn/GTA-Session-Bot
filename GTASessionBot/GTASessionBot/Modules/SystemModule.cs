using Discord;
using Discord.Commands;
using GTASessionBot.Preconditions;
using GTASessionBot.Providers;
using GTASessionBot.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {
    [Group("System"), Name("System")]
    [Summary("Get system information about the machine that is running the session host.")]
    public class SystemModule : ModuleBase<SocketCommandContext> {

        [IsAdmin]
        [Command("cpu")]
        [Summary("Gets information about the CPU of the system.")]
        public async Task GetCpuInformationAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":mag: Gathering data", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                string cores;
                string uptime;
                string processor;
                Dictionary<string, string> temps;
                Dictionary<string, string> usage;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                cores = CpuProvider.GetCoreCount();
                uptime = CpuProvider.GetUpTime();
                processor = CpuProvider.GetProcessorInformation();
                temps = CpuProvider.GetCpuTemperatures();
                usage = CpuProvider.GetCpuLoad();

                embed.AddField("Processor", processor);
                embed.AddField("Processor Cores", cores);
                embed.AddField("System Up Time", uptime);

                foreach (var kvp in usage) {
                    embed.AddField(kvp.Key, kvp.Value);
                }

                foreach (var kvp in temps) {
                    embed.AddField(kvp.Key, kvp.Value);
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        [IsAdmin]
        [Command("gpu")]
        [Summary("Gets information about the GPU of the system.")]
        public async Task GetGpuInformationAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":mag: Gathering data", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Dictionary<string, string> gpuUsage;
                Dictionary<string, string> gpuTemps;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                gpuUsage = GpuProvider.GetGpuUsage();
                gpuTemps = GpuProvider.GetGpuTemperatures();

                foreach (var kvp in gpuUsage) {
                    embed.AddField(kvp.Key, kvp.Value);
                }

                foreach (var kvp in gpuTemps) {
                    embed.AddField(kvp.Key, kvp.Value);
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        [IsAdmin]
        [Command("ram")]
        [Summary("Gets information about the system RAM.")]
        public async Task GetMemoryInformationAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":mag: Gathering data", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Dictionary<string, string> ram;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                ram = MemoryProvider.GetRamUsage();

                foreach (var kvp in ram) {
                    embed.AddField(kvp.Key, kvp.Value);
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }

        }


        [IsAdmin]
        [Command("shutdown")]
        [Summary("Shuts down the host machine.")]
        public async Task ShutdownSystem() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":zzz: Preparing to shutdown", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            using (var proc = Process.Start("shutdown", "/s /t 0")) {
                // Nothing to do here as the process will be disposed of.
            }
        }


        [IsAdmin]
        [Command("restart")]
        [Summary("Restarts the host machine.")]
        public async Task RestartSystem() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":zzz: Preparing to restart", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            using (var proc = Process.Start("shutdown.exe", "/r /t 0")) {
                // Nothing to do here as the process will be disposed of.
            }

        }


        [Command("latency")]
        [Alias("ping")]
        [Summary("Gets the estimated round-trip latency, in milliseconds, to the gateway server.")]
        public async Task GetLatencyAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":earth_asia: Calculating latency", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                embed.AddField(":earth_asia: Latency", $"{Context.Client.Latency}ms");

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }


        [IsAdmin]
        [Command("network")]
        [Alias("net")]
        [Summary("Displays the UDP network information of the machine.")]
        public async Task GetNetworkStatsAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":computer: Gathering Network Statistics", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                IPGlobalProperties properties;
                UdpStatistics udpStat = null;


                embed = EmbedHelper.GetDefaultEmbed(Context);
                properties = IPGlobalProperties.GetIPGlobalProperties();
                udpStat = properties.GetUdpIPv4Statistics();


                embed.AddField("Datagrams Received", udpStat.DatagramsReceived.ToString("#,##0"), inline: true);
                embed.AddField("Datagrams Sent", udpStat.DatagramsSent.ToString("#,##0"), inline: true);

                embed.AddField("Datagrams Discarded", udpStat.IncomingDatagramsDiscarded.ToString("#,##0"));
                embed.AddField("Datagrams With Errors", udpStat.IncomingDatagramsWithErrors.ToString("#,##0"));
                embed.AddField("UDP Listeners", udpStat.UdpListeners.ToString("#,##0"));

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });

            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }

    }
}
