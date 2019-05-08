using GTASessionBot.Windows_Libraries;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GTASessionBot.Providers
{
    public class ScreenCaptureProvider
    {

        private int PlayerListWidth = 280; // Default 280
        private int PlayerListHeight = 601; // Default 601
        private int PlayerListXOffset = 50; // Default 50
        private int PlayerListYOffset = 40; // Default 40

        private Configuration.Configuration _config;

        public ScreenCaptureProvider(Configuration.Configuration config)
        {
            _config = config;

            PlayerListHeight = _config.ListHeight;
            PlayerListWidth = _config.ListWidth;
            PlayerListXOffset = _config.ListXOffset;
            PlayerListYOffset = _config.ListYOffset;
        }



        /// <summary>
        /// Captures a screenshot of the given window handle.
        /// </summary>
        /// <param name="windowHandle">The window handle to get the screenshot from.</param>
        /// <returns>The <see cref="MemoryStream"/> of the captured screenshot.</returns>
        public byte[] CaptureWindow(IntPtr windowHandle)
        {

            Rect rect;
            int width;
            int height;
            Bitmap bmp;
            Graphics graphics;
            MemoryStream rc;


            rect = new Rect();

            // Get the entire bounds of the window.
            User32.GetWindowRect(windowHandle, ref rect);

            width = rect.right - rect.left;
            height = rect.bottom - rect.top;

            // Create a bitmap from the size of the window.
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bmp);

            graphics.CopyFromScreen(
                rect.left,
                rect.top,
                0,
                0,
                new Size(width, height),
                CopyPixelOperation.SourceCopy
            );

            rc = new MemoryStream();

            bmp.Save(rc, ImageFormat.Jpeg);

            return rc.ToArray();
        }


        /// <summary>
        /// Captures a screenshot of the given window handle.
        /// </summary>
        /// <param name="windowHandle">The window handle to get the screenshot from.</param>
        /// <param name="filePath">The file path to save the image to.</param>
        /// <returns>The <see cref="Bitmap"/> of the captured screenshot.</returns>
        public void CapturePlayerList(IntPtr windowHandle, string filePath)
        {

            Rect rect;
            int width;
            int height;
            Bitmap bmp;
            Graphics graphics;


            rect = new Rect();

            // Get the entire bounds of the window.
            User32.GetWindowRect(windowHandle, ref rect);

            // Set the width and the height of the player list.
            width = PlayerListWidth;
            height = PlayerListHeight;

            // Create a bitmap from the size of the window.
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bmp);

            graphics.CopyFromScreen(
                rect.left + PlayerListXOffset,
                rect.top + PlayerListYOffset,
                0,
                0,
                new Size(width, height),
                CopyPixelOperation.SourceCopy
            );

            bmp.Save(filePath, ImageFormat.Jpeg);
        }


        /// <summary>
        /// Merges the player list screenshots into a single image.
        /// </summary>
        /// <returns>A byte array of the merged image.</returns>
        public byte[] MergePlayerList()
        {
            MemoryStream rc;
            Image list1;
            Image list2;


            if (!File.Exists(Common.PlayerListFirstScreenshotPath) || !File.Exists(Common.PlayerListSecondScreenshotPath))
            {
                throw new Exception(
                    "Unable to locate the player list screenshots, please ensure the bot has had adequate time to capture the player list."
                );
            }

            list1 = Image.FromFile(Common.PlayerListFirstScreenshotPath);
            list2 = Image.FromFile(Common.PlayerListSecondScreenshotPath);
            rc = new MemoryStream();

            using (Bitmap bmp = new Bitmap(list1.Width + list2.Width, Math.Max(list1.Height, list2.Height)))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(list1, new Point(0, 0));
                    g.DrawImage(list2, new Point(list1.Width, 0));
                }

                bmp.Save(rc, ImageFormat.Jpeg);
            }

            return rc.ToArray();
        }
    }
}
