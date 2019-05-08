using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GTASessionBot.Providers;
using GTASessionBot.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTASessionBot.Modules {

    //[Name("Permissions"), Alias("perms")]
    //[Summary("Handles permission based things.")]
    public class PermissionsModule : ModuleBase<SocketCommandContext> {
        private readonly PermissionManager _permissionManager;

        public PermissionsModule(PermissionManager manager) {
            _permissionManager = manager;
        }


        //[IsAdmin]
        //[Command("naughty")]
        //[Summary("Gets the list of muppets who have tried to access commands that they don't have permissions for.")]
        public async Task GetNaughtyListAsync() {
            IUserMessage response;
            EmbedBuilder initialBuilder;


            initialBuilder = EmbedHelper.GetDefaultEmbed(Context);
            initialBuilder.AddField(":mag: Gathering naughty list", "Please wait...");

            response = await ReplyAsync("", embed: initialBuilder.Build());

            try {
                EmbedBuilder embed;
                Dictionary<ulong, int> failedUsers;


                embed = EmbedHelper.GetDefaultEmbed(Context);

                failedUsers = _permissionManager.getNaughtyList();

                foreach (var failedUser in failedUsers) {
                    SocketGuildUser user;


                    user = Context.Guild.GetUser(failedUser.Key);

                    embed.AddField(":face_palm:", $"User **{user.Nickname}** has failed {failedUser.Value} permission checks.");
                }

                await response.ModifyAsync(msg => { msg.Embed = embed.Build(); msg.Content = ""; });
            } catch (Exception e) {
                await response.ModifyAsync(msg => msg = ErrorHandler.GetDefaultErrorMessageEmbed(e, msg, Context.Message));
            }
        }
    }
}
