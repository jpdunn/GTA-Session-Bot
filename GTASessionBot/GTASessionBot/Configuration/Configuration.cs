using System.Collections.Generic;

namespace GTASessionBot.Configuration {

    public class Configuration {

        /// <summary>
        /// The Discord API token.
        /// </summary>
        public string DiscordToken { get; set; }


        /// <summary>
        /// Returns true if the installed GTA game is a Steam game, 
        /// otherwise false if the game is installed via Social Club.
        /// </summary>
        public bool IsSteamGame { get; set; }


        /// <summary>
        /// The command prefix to use.
        /// </summary>
        public string Prefix { get; set; }


        /// <summary>
        /// The location of the bot script.
        /// </summary>
        public string ScriptLocation { get; set; }


        /// <summary>
        /// The name of the bot process.
        /// </summary>
        public string BotProcessName { get; set; }


        /// <summary>
        /// Whether or not debug logging is on.
        /// </summary>
        public bool DebugLogging { get; set; }


        /// <summary>
        /// Gets or sets the width of the player list.
        /// </summary>
        public int ListWidth { get; set; }


        /// <summary>
        /// Gets or sets the height of the player list.
        /// </summary>
        public int ListHeight { get; set; }


        /// <summary>
        /// Gets or sets the x offset of the player list.
        /// </summary>
        public int ListXOffset { get; set; }


        /// <summary>
        /// Gets or sets the y offset of the player list.
        /// </summary>
        public int ListYOffset { get; set; }


        /// <summary>
        /// Gets or sets the admin role IDs.
        /// </summary>
        public List<ulong> AdminRoleList { get; set; }

    }
}
