using GTASessionBot.Utilities;
using GTASessionBot.Windows_Libraries;
using System;
using System.IO;
using System.Threading;

namespace GTASessionBot.Providers {
    public class ScreenshotProvider : IDisposable {

        private int _StopProcessing;
        private Thread cgExecutingThread;
        private String _tempDirectoryPath;
        private static Object syncLock = new Object();
        private Configuration.Configuration _config { get; }

        private readonly IGate cgGate;


        private const int ErrorDelay = 5000;

        public ScreenshotProvider(Configuration.Configuration config) {
            _StopProcessing = 0;
            cgGate = new Gate();

            _tempDirectoryPath = Common.TempDirectoryPath;

            if (!Directory.Exists(_tempDirectoryPath)) {
                Directory.CreateDirectory(_tempDirectoryPath);
            }

            _config = config;

        }

        public void Dispose() {
            Stop();
        }


        public void Start() {
            StopProcessing = false;
            cgGate.Close();

            cgExecutingThread = new Thread(ThreadProc);
            cgExecutingThread.Name = "Screenshot Provider";
            cgExecutingThread.Start();
        }


        /// <summary>
        /// Stops the application's processor.
        /// </summary>
        public void Stop() {
            StopProcessing = true;

            if (cgExecutingThread != null) {
                cgGate.Open();

                cgExecutingThread.Join();
                cgExecutingThread = null;
            }
        }


        /// <summary>
        /// Determines whether processing should stop.
        /// </summary>
        /// <returns>True if processing should stop; otherwise, False.</returns>
        private bool StopProcessing {
            get {
                return (Interlocked.CompareExchange(ref _StopProcessing, 0, 0) != 0);
            }

            set {
                Interlocked.Exchange(ref _StopProcessing, value ? 1 : 0);
            }
        }


        /// <summary>
        /// Processes required handovers over time.
        /// </summary>
        private void ThreadProc() {
            do {

                cgGate.Close();

                if (StopProcessing) {
                    break;
                }


                FetchPlayerList();

                // Wait 20 seconds until we open the gate.
                Console.WriteLine("---------------Sleeping for 20 seconds.--------------");

                cgGate.WaitUntilOpen(20000);
            }
            while (true);
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
