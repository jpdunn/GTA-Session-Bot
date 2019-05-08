using Microsoft.Win32;


namespace GTASessionBot.Utilities
{
    public class RegistryHelper
    {

        /// <summary>
        /// Gets the install path for the Steam version of Grand Theft Auto from the registry.
        /// </summary>
        /// <returns>The install path for the Steam version of Grand Theft Auto.</returns>
        public static string GetGTASteamInstallPath()
        {
            RegistryKey key;


            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Rockstar Games\GTAV");

            if (key != null)
            {
                return key.GetValue("installfoldersteam").ToString();
            }

            return null;
        }


        /// <summary>
        /// Gets the install path for the Social Club version of Grand Theft Auto from the registry.
        /// </summary>
        /// <returns>The install path for the Social Club version of Grand Theft Auto.</returns>
        public static string GetGTASocialClubInstallPath()
        {
            RegistryKey key;


            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Rockstar Games\Grand Theft Auto V");

            if (key != null)
            {
                return key.GetValue("InstallFolder").ToString();
            }

            return null;
        }


        /// <summary>
        /// Gets the install path for the Steam client if it exists.
        /// </summary>
        /// <returns>The install path for the Steam client if it exists; otherwise null.</returns>
        public static string GetSteamPath()
        {
            RegistryKey key;


            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\Steam");

            if (key != null)
            {
                return key.GetValue("installPath").ToString();
            }

            return null;
        }
    }
}
