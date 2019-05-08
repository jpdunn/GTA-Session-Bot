using System;
using System.Runtime.InteropServices;

namespace GTASessionBot.Windows_Libraries {
    class User32 {

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window and, 
        /// optionally, the identifier of the process that created the window.
        /// </summary>
        /// <param name="handle">A handle to the window.</param>
        /// <param name="processId">
        /// A pointer to a variable that receives the process identifier. If this parameter is not NULL, 
        /// GetWindowThreadProcessId copies the identifier of the process to the variable; 
        /// otherwise, it does not.
        /// </param>
        /// <returns>The identifier of the thread that created the window.</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);


        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window. 
        /// Keyboard input is directed to the window, and various visual cues are changed for the user. 
        /// The system assigns a slightly higher priority to the thread that created the foreground window 
        /// than it does to other threads.
        /// </summary>
        /// <param name="point">A handle to the window that should be activated and brought to the foreground.</param>
        /// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);


        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. 
        /// The dimensions are given in screen coordinates that are relative 
        /// to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="rect">
        /// A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. 
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);


        /// <summary>
        /// Retrieves a handle to the desktop window. The desktop window covers the entire screen. 
        /// The desktop window is the area on top of which other windows are painted.
        /// </summary>
        /// <returns>The handle to the desktop window.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();


        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="nInputs">The number of structures in the pInputs array.</param>
        /// <param name="pInputs">
        /// An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.
        /// </param>
        /// <param name="cbSize">
        /// The size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.
        /// </param>
        /// <returns>
        /// The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. 
        /// If the function returns zero, the input was already blocked by another thread.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] Input[] pInputs, Int32 cbSize);


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
