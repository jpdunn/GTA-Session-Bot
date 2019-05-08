using System;
using System.Collections.Generic;
using System.IO;

namespace GTASessionBot {
    public class Common {

        /// <summary>
        /// Gets the temp directory path to store player list screenshots.
        /// </summary>
        public static string TempDirectoryPath {
            get {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%/.gta"), "GTA Session Bot");
            }
        }


        /// <summary>
        /// Gets the path to the first player list screenshot.
        /// </summary>
        public static string PlayerListFirstScreenshotPath {
            get {
                return Path.Combine(TempDirectoryPath, "list1.jpg");
            }
        }


        /// <summary>
        /// Gets the path to the second player list screenshot.
        /// </summary>
        public static string PlayerListSecondScreenshotPath {
            get {
                return Path.Combine(TempDirectoryPath, "list2.jpg");
            }
        }


        /// <summary>
        /// Gets the list of gifs to use for the keyword 'Denied'.
        /// </summary>
        public static List<string> DeniedGifs {
            get {
                return new List<string> {
                    "https://media.giphy.com/media/l3V0px8dfZmmfwize/giphy.gif",
                    "https://media.giphy.com/media/3HAYjf9agsx7U3aXKXm/giphy.gif",
                    "https://media.giphy.com/media/bTmU4fa3xUyqY/giphy.gif",
                    "https://media.giphy.com/media/ydJOf0YQaK4tW/giphy.gif",
                    "https://media.giphy.com/media/26gBjplxRU5EUhvlS/giphy.gif",
                    "https://media.giphy.com/media/d49xKuZ49wP8RUpq/giphy.gif",
                    "https://media.giphy.com/media/l0OXWXUHdp4K9nitq/giphy.gif",
                    "https://media.giphy.com/media/7NcBFRq8cxksM/giphy.gif",
                    "https://media.giphy.com/media/nn2kmb1lRtpkY/giphy.gif",
                    "https://media.giphy.com/media/26u4m6DWqw4Zef38Y/giphy.gif",
                    "https://media0.giphy.com/media/Zw133sEVc0WXK/giphy.gif",
                    "https://media.giphy.com/media/1zlj55brLYCYmJqujn/giphy.gif",
                    "https://media.giphy.com/media/1Agk6OMNgBNW8ycXKl/giphy.gif",
                    "https://media.giphy.com/media/YVscawov2P6bUNkGLC/giphy.gif",
                    "https://media.giphy.com/media/pVR9FBvnp055xcLTId/giphy.gif"
                };
            }
        }


        public static List<string> FailedCommandMessages {
            get {
                return new List<string> {
                    "Are you seriously still trying?",
                    "No...just no.",
                    "Can you kindly scurry on.",
                    "I heard HALP is looking for new members :FeelsCringeMan:",
                    "**GO AWAY YOU'RE NOT ALLOWED TO USE THIS COMMAND.** Jeeze, why is that so hard to understand.",
                    "I don't like you, please stop.",
                    "Your mother should have swallowed you.",
                    "You are the weakest link...goodbye."
                };
            }
        }

    }
}
