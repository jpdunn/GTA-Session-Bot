using System;
using System.Diagnostics;
using System.Linq;

namespace GTASessionBot.Providers {
    public class ProcessProvider {


        /// <summary>
        /// Attempts to find the process for Grand Theft Auto V and if found, returns the window handle for the process.
        /// </summary>
        /// <returns>The window handle for the process.</returns>
        public IntPtr GetGrandTheftAutoProcessPointer() {
            Process process;
            IntPtr windowHandle;


            process = Process.GetProcessesByName("gta5").FirstOrDefault();

            if (process == null) {
                throw new ApplicationException("Attempted to get the GTA V process but was unable to find it.");
            }

            windowHandle = process.MainWindowHandle;

            return windowHandle;
        }


        /// <summary>
        /// Attempts to find the process for Grand Theft Auto V and returns it.
        /// </summary>
        /// <returns>The process for Grand Theft Auto V if it is running; otherwise null.</returns>
        public Process GetGrandTheftAutoProcess() {
            return Process.GetProcessesByName("gta5").FirstOrDefault();
        }


        /// <summary>
        /// Attempts to find the process for Grand Theft Auto V launcher and returns it.
        /// </summary>
        /// <returns>The process for Grand Theft Auto V launcher if it is running; otherwise null.</returns>
        public Process GetGrandTheftAutoLauncherProcess() {
            return Process.GetProcessesByName("GTAVLauncher").FirstOrDefault();
        }


        /// <summary>
        /// Attempts to find the process for Windows error reporting and returns it.
        /// </summary>
        /// <returns>The process for Windows error reporting if it is running; otherwise null.</returns>
        public Process GetWindowsErrorReporterProcess() {
            return Process.GetProcessesByName("WerFault").FirstOrDefault();
        }


        /// <summary>
        /// Attempts to find the process for the AHK bot script and returns it.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        /// <returns>The process.</returns>
        public Process GetBotScriptProcess(Configuration.Configuration config) {
            Process process;


            process = Process.GetProcessesByName(config.BotProcessName).FirstOrDefault();

            return process;
        }


        /// <summary>
        /// Starts the bot process.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public void StartBotProcess(Configuration.Configuration config) {
            ProcessStartInfo info;


            info = new ProcessStartInfo {
                UseShellExecute = false,
                FileName = config.ScriptLocation
            };

            using (Process.Start(info)) {
                // Do nothing, we don't need the process anymore and it is disposable.
            }
        }
    }
}
