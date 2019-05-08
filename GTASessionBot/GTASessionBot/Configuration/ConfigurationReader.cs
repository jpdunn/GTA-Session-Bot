using Newtonsoft.Json;
using System;
using System.IO;

namespace GTASessionBot.Configuration {
    public class ConfigurationReader {

        public static string _filename { get; private set; } = "_configurations.json";


        /// <summary>
        /// Creates a new <see cref="ConfigurationReader"/>.
        /// </summary>
        /// <param name="filename">The filename of the configuration file.</param>
        public ConfigurationReader(string filename) {
            _filename = filename;
        }


        /// <summary>
        /// Ensures that the bot configuration file exists.
        /// </summary>
        public static void EnsureExists() {
            string file;


            file = Path.Combine(AppContext.BaseDirectory, _filename);

            if (!File.Exists(file)) {
                throw new ApplicationException("Unable to locate the _configurations.json file.");
            }
        }


        /// <summary>
        /// Loads the configuration file.
        /// </summary>
        /// <returns>The configuration object.</returns>
        public static Configuration Load() {

            string file;


            // Ensure the configuration file exists before attempting to parse it.
            EnsureExists();

            file = Path.Combine(AppContext.BaseDirectory, _filename);

            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }

    }
}
