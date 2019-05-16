using Discord;
using Discord.Commands;
using GTASessionBot.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {

    /// <summary>
    /// Defines a module for help related commands.
    /// </summary>
    [Group("help"), Name("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext> {

        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;
        private readonly Configuration.Configuration _config;


        /// <summary>
        /// Initializes a new instance of the <see cref="HelpModule"/>.
        /// </summary>
        /// <param name="provider">The service provider to use.</param>
        /// <param name="config">The configuration to use.</param>
        public HelpModule(
            IServiceProvider provider,
            Configuration.Configuration config
        ) {
            _commands = provider.GetService<CommandService>();
            _provider = provider;
            _config = config;
        }


        /// <summary>
        /// Handles a basic help command.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [Command]
        public async Task HelpAsync() {
            List<ModuleInfo> modules;
            EmbedBuilder embed;


            modules = _commands.Modules.Where(x => !string.IsNullOrWhiteSpace(x.Summary)).ToList();

            embed = new EmbedBuilder()
                .WithFooter(x => x.Text = $"Type `{_config.Prefix}help <module>` for more information");

            foreach (var module in modules) {
                bool success;


                success = false;

                foreach (var command in module.Commands) {
                    PreconditionResult result;


                    result = await command.CheckPreconditionsAsync(Context, _provider);

                    if (result.IsSuccess) {
                        success = true;
                        break;
                    }
                }

                if (!success)
                    continue;

                embed.AddField(module.Name, module.Summary);
            }

            embed.WithColor(EmbedColors.GetSuccessColor());

            await ReplyAsync("", embed: embed.Build());
        }


        /// <summary>
        /// Handles help commands for a specific module.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <returns>An awaitable task.</returns>
        [Command]
        public async Task HelpAsync(string moduleName) {
            ModuleInfo module;
            List<CommandInfo> commands;
            EmbedBuilder builder;


            module = _commands.Modules.FirstOrDefault(x => string.Equals(x.Name.ToLower(), moduleName.ToLower()));

            if (string.Equals(moduleName[0].ToString(), _config.Prefix)) {
                string commandName;
                CommandInfo command;


                commandName = moduleName.Substring(1);
                command = _commands.Commands.FirstOrDefault(x => x.Aliases.Any(z => string.Equals(z.ToLower(), commandName.ToLower())));

                await HelpAsync(command.Module.Name, moduleName.Substring(1));
                return;
            }

            if (module == null) {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }

            commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary))
                                 .GroupBy(x => x.Name)
                                 .FirstOrDefault()
                                 .ToList();

            if (!commands.Any()) {
                await ReplyAsync($"The module `{module.Name}` has no available commands :(");
                return;
            }



            builder = new EmbedBuilder();

            foreach (var command in commands) {
                PreconditionResult result;


                result = await command.CheckPreconditionsAsync(Context, _provider);

                if (result.IsSuccess) {
                    string field;


                    field = command.Aliases.First();

                    if (command.Aliases.Count > 1) {
                        List<string> skipped;
                        string aliases;


                        skipped = command.Aliases.Skip(1).ToList();
                        aliases = "";

                        // Iterate over all the aliases skipping the first one (as that is the main command)
                        // and if we have any then build a string to display them.
                        for (var i = 0; i < skipped.Count; i++) {
                            if (i == skipped.Count - 1) {
                                aliases += $"{skipped[i]}";
                            } else {
                                aliases += $"{skipped[i]}, ";
                            }
                        }

                        // If we had any aliases, then surround them with brackets for formatting sake.
                        if (!string.IsNullOrEmpty(aliases)) {
                            field = $"{field} ({aliases})";
                        }

                    }

                    builder.AddField($"{_config.Prefix}{field}", command.Summary);
                }
            }

            builder.WithColor(EmbedColors.GetSuccessColor());

            await ReplyAsync("", embed: builder.Build());
        }


        /// <summary>
        /// Handles help commands for a specific command in a module.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <returns>An awaitable task.</returns>
        private async Task HelpAsync(string moduleName, string commandName) {
            string alias;
            ModuleInfo module;
            List<CommandInfo> commands;


            alias = $"{commandName}".ToLower();

            module = _commands.Modules.FirstOrDefault(x => string.Equals(x.Name.ToLower(), moduleName.ToLower()));

            if (module == null) {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }

            commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary)).ToList();

            if (!commands.Any()) {
                await ReplyAsync($"The module `{module.Name}` has no available commands.");
                return;
            }

            EmbedBuilder builder;
            List<string> aliases;


            builder = new EmbedBuilder();
            aliases = new List<string>();

            foreach (var overload in commands.Where(x => x.Aliases.Contains(alias))) {
                PreconditionResult result;


                result = await overload.CheckPreconditionsAsync(Context, _provider);

                if (result.IsSuccess) {
                    StringBuilder stringBuilder;


                    stringBuilder = new StringBuilder();

                    stringBuilder.Append(_config.Prefix + overload.Aliases.First());

                    foreach (var parameter in overload.Parameters) {
                        string name;


                        name = StringHelper.FirstCharToUpper(parameter.Name);

                        if (parameter.IsRemainder)
                            name += "...";
                        if (parameter.IsOptional)
                            name = $"[{name}]";
                        else
                            name = $"<{name}>";

                        stringBuilder.Append(" " + name);
                    }

                    builder.AddField(stringBuilder.ToString(), overload.Remarks ?? overload.Summary);
                }
                aliases.AddRange(overload.Aliases);
            }

            builder.WithFooter(x => x.Text = $"Aliases: {string.Join(", ", aliases)}");

            builder.WithColor(EmbedColors.GetSuccessColor());

            await ReplyAsync("", embed: builder.Build());
        }
    }
}
