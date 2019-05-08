using Discord;
using Discord.Commands;
using GTASessionBot.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASessionBot.Modules
{
    [Group("help"), Name("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;
        private readonly Configuration.Configuration _config;


        public HelpModule(IServiceProvider provider, Configuration.Configuration config)
        {
            _commands = provider.GetService<CommandService>();
            _provider = provider;
            _config = config;
        }

        [Command]
        public async Task HelpAsync()
        {
            var modules = _commands.Modules.Where(x => !string.IsNullOrWhiteSpace(x.Summary));

            var embed = new EmbedBuilder()
                .WithFooter(x => x.Text = $"Type `{_config.Prefix}help <module>` for more information");

            foreach (var module in modules)
            {
                bool success = false;
                foreach (var command in module.Commands)
                {
                    var result = await command.CheckPreconditionsAsync(Context, _provider);
                    if (result.IsSuccess)
                    {
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

        [Command]
        public async Task HelpAsync(string moduleName)
        {
            var module = _commands.Modules.FirstOrDefault(x => x.Name.ToLower() == moduleName.ToLower());

            if (moduleName[0].ToString() == _config.Prefix)
            {
                var commandName = moduleName.Substring(1);
                var command = _commands.Commands.FirstOrDefault(x => x.Aliases.Any(z => z.ToLower() == commandName.ToLower()));
                await HelpAsync(command.Module.Name, moduleName.Substring(1));
                return;
            }

            if (module == null)
            {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }


            var commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary))
                                 .GroupBy(x => x.Name)
                                 .Select(x => x.First());

            if (!commands.Any())
            {
                await ReplyAsync($"The module `{module.Name}` has no available commands :(");
                return;
            }

            var embed = new EmbedBuilder();

            foreach (var command in commands)
            {
                var result = await command.CheckPreconditionsAsync(Context, _provider);
                if (result.IsSuccess)
                {
                    string field;


                    field = command.Aliases.First();

                    if (command.Aliases.Count > 1)
                    {
                        List<string> skipped;
                        string aliases;


                        skipped = command.Aliases.Skip(1).ToList();
                        aliases = "";

                        // Iterate over all the aliases skipping the first one (as that is the main command)
                        // and if we have any then build a string to display them.
                        for (var i = 0; i < skipped.Count; i++)
                        {
                            if (i == skipped.Count - 1)
                            {
                                aliases += $"{skipped[i]}";
                            }
                            else
                            {
                                aliases += $"{skipped[i]}, ";
                            }
                        }

                        // If we had any aliases, then surround them with brackets for formatting sake.
                        if (!string.IsNullOrEmpty(aliases))
                        {
                            field = $"{field} ({aliases})";
                        }

                    }

                    embed.AddField($"{_config.Prefix}{field}", command.Summary);
                }
            }

            embed.WithColor(EmbedColors.GetSuccessColor());

            await ReplyAsync("", embed: embed.Build());
        }

        private async Task HelpAsync(string moduleName, string commandName)
        {
            string alias = $"{commandName}".ToLower();
            var module = _commands.Modules.FirstOrDefault(x => x.Name.ToLower() == moduleName.ToLower());

            if (module == null)
            {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }

            var commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary));

            if (commands.Count() == 0)
            {
                await ReplyAsync($"The module `{module.Name}` has no available commands :(");
                return;
            }

            var command = commands.Where(x => x.Aliases.Contains(alias));
            var embed = new EmbedBuilder();

            var aliases = new List<string>();
            foreach (var overload in command)
            {
                var result = await overload.CheckPreconditionsAsync(Context, _provider);
                if (result.IsSuccess)
                {
                    var sbuilder = new StringBuilder()
                        .Append(_config.Prefix + overload.Aliases.First());

                    foreach (var parameter in overload.Parameters)
                    {
                        string p = parameter.Name;
                        p = StringHelper.FirstCharToUpper(p);

                        if (parameter.IsRemainder)
                            p += "...";
                        if (parameter.IsOptional)
                            p = $"[{p}]";
                        else
                            p = $"<{p}>";

                        sbuilder.Append(" " + p);
                    }

                    embed.AddField(sbuilder.ToString(), overload.Remarks ?? overload.Summary);
                }
                aliases.AddRange(overload.Aliases);
            }

            embed.WithFooter(x => x.Text = $"Aliases: {string.Join(", ", aliases)}");

            embed.WithColor(EmbedColors.GetSuccessColor());

            await ReplyAsync("", embed: embed.Build());
        }
    }
}
