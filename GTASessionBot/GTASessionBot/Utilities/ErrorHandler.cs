using Discord;
using Discord.WebSocket;
using System;

namespace GTASessionBot.Utilities {
    public class ErrorHandler {

        public static MessageProperties GetDefaultErrorMessageEmbed(
            Exception e,
            MessageProperties message,
            SocketUserMessage userMessage
        ) {
            EmbedBuilder embed;


            embed = new EmbedBuilder();
            embed.WithColor(EmbedColors.GetErrorColor());

            embed.AddField(":warning: An unexpected error occured.", e.Message);

            message.Content = "";
            message.Embed = embed.Build();

            // Also ensure that we are logging the exception to the event log.
            EventLogLogger.LogError(e, userMessage.Content);

            return message;
        }

    }
}
