using GTASessionBot.Windows_Libraries;
using System;
using System.IO;
using System.Threading;
using System.Timers;

namespace GTASessionBot.Providers {
    public class ScreenshotManager {

        private static System.Timers.Timer _screenshotTimer;
        private static int _ScreenshotAge;
        private String _tempDirectoryPath;
        private static Object syncLock = new Object();
        private Configuration.Configuration _config { get; }


        public ScreenshotManager(Configuration.Configuration config) {
            _ScreenshotAge = 0;

            // Set up the timer for automatically capturing screenshots once all our commands are hooked up.
            _screenshotTimer = new System.Timers.Timer();
            _screenshotTimer.Interval = 1000;
            _screenshotTimer.Elapsed += new ElapsedEventHandler(TimerTick);
            _tempDirectoryPath = Common.TempDirectoryPath;

            if (!Directory.Exists(_tempDirectoryPath)) {
                Directory.CreateDirectory(_tempDirectoryPath);
            }

            _config = config;
        }


        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer() {
            _screenshotTimer.Stop();
        }


        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer() {
            _screenshotTimer.Start();
        }


        private void TimerTick(object sender, ElapsedEventArgs e) {
            lock (syncLock) {
                _ScreenshotAge += 1;

                if (_ScreenshotAge > 20) {
                    FetchPlayerList();
                    _ScreenshotAge = 0;
                } else {
                    if (_config.DebugLogging) {
                        Console.WriteLine($"---------------Sleeping for { 20 - _ScreenshotAge} seconds...--------------");
                    }
                }
            }
        }


        /// <summary>
        /// Fetches both pages of the player list and saves the images to disk.
        /// </summary>
        private void FetchPlayerList() {
            ProcessProvider provider;
            IntPtr windowHandle;
            ScreenCaptureProvider captureProvider;


            Console.WriteLine("---------------Capturing player list.--------------");
            provider = new ProcessProvider();
            windowHandle = provider.GetGrandTheftAutoProcessPointer();
            captureProvider = new ScreenCaptureProvider(_config);

            User32.SetForegroundWindow(windowHandle);
            Thread.Sleep(1000);

            // Send the keypress to open the player list.
            KeyPressProvider.SendKeyEvent(System.Windows.Forms.Keys.Z);
            Thread.Sleep(500);

            captureProvider.CapturePlayerList(windowHandle, Path.Combine(_tempDirectoryPath, "list1.jpg"));

            // Sleep for a second so that we can given the game time to update.
            Thread.Sleep(1000);

            // Send the keypress to open the second page of the playerlist.
            KeyPressProvider.SendKeyEvent(System.Windows.Forms.Keys.Z);
            Thread.Sleep(500);

            captureProvider.CapturePlayerList(windowHandle, Path.Combine(_tempDirectoryPath, "list2.jpg"));
        }
    }
}
