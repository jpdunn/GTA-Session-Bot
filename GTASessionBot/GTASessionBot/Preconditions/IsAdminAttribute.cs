using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTASessionBot.Preconditions {

    /// <summary>
    /// Defines a precondition attribute to check if the user is classed as an administrator.
    /// </summary>
    public class IsAdmin : PreconditionAttribute {


        public async override Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context,
            CommandInfo command,
            IServiceProvider services
        ) {
            ulong ownerID;
            Configuration.Configuration config;
            SocketGuildUser user;


            ownerID = (await services.GetService<DiscordSocketClient>().GetApplicationInfoAsync()).Owner.Id;

            if (ownerID == context.User.Id) {
                return PreconditionResult.FromSuccess();
            }

            if (!(context.User is SocketGuildUser)) {
                return PreconditionResult.FromError("Not invoked in a Guild");
            }

            config = services.GetService<Configuration.Configuration>();
            user = (SocketGuildUser)context.User;

            return user.Roles.Any(role => config.AdminRoleList.Contains(role.Id))
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(GetDeniedGif());
        }


        /// <summary>
        /// Gets a random gif to use for the denied message.
        /// </summary>
        /// <returns>A gif URL.</returns>
        private string GetDeniedGif() {
            Random r = new Random();
            int rInt = r.Next(0, Common.DeniedGifs.Count);

            return Common.DeniedGifs[rInt];
        }
    }
}
