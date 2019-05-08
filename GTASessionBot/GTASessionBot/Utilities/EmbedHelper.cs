using Discord;
using Discord.Commands;

namespace GTASessionBot.Utilities {
    public class EmbedHelper {

        public static EmbedBuilder GetDefaultEmbed(SocketCommandContext context) {
            EmbedBuilder rc;


            rc = new EmbedBuilder();
            rc.WithColor(EmbedColors.GetSuccessColor());
            rc.Author = new EmbedAuthorBuilder() {
                Name = context.Client.CurrentUser.Username,
                IconUrl = context.Client.CurrentUser.GetAvatarUrl()
            };

            return rc;
        }
    }
}
