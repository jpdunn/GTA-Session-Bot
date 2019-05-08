using Discord.Commands;
using Discord.WebSocket;
using GTASessionBot.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTASessionBot.Preconditions {

    public class IsAdmin : PreconditionAttribute {

        public async override Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context,
            CommandInfo command,
            IServiceProvider services
        ) {
            ulong ownerID;
            Configuration.Configuration config;
            SocketGuildUser user;
            bool isAllowed;
            PermissionManager permissionManager;


            ownerID = (await services.GetService<DiscordSocketClient>().GetApplicationInfoAsync()).Owner.Id;
            permissionManager = services.GetService<PermissionManager>();

            if (ownerID == context.User.Id) {
                return PreconditionResult.FromSuccess();
            }

            if (!(context.User is SocketGuildUser)) {
                return PreconditionResult.FromError("Not invoked in a Guild");
            }

            config = services.GetService<Configuration.Configuration>();
            user = (SocketGuildUser)context.User;

            isAllowed = user.Roles.Any(role => config.AdminRoleList.Contains(role.Id));

            // TODO: REMOVE AFTER TESTING.
            //isAllowed = false;

            if (!isAllowed) {
                permissionManager.AddOrIncrementUserFailure(user.Id);
            }

            //return PreconditionResult.FromError(CheckUserFailures(services, user));
            return user.Roles.Any(role => config.AdminRoleList.Contains(role.Id))
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(CheckUserFailures(services, user));
        }



        private string CheckUserFailures(IServiceProvider services, SocketGuildUser user) {
            PermissionManager permissionManager;


            permissionManager = services.GetService<PermissionManager>();

            if (permissionManager.UserCommandFailures(user.Id) % 5 == 0) {
                return GetPermissionDeniedMessage();
            } else {
                return GetDeniedGif();
            }
        }


        private string GetDeniedGif() {
            Random r = new Random();
            int rInt = r.Next(0, Common.DeniedGifs.Count);

            return Common.DeniedGifs[rInt];
        }


        private string GetPermissionDeniedMessage() {
            Random r = new Random();
            int rInt = r.Next(0, Common.FailedCommandMessages.Count);

            return Common.FailedCommandMessages[rInt];
        }

    }
}
