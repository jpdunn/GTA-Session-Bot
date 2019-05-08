using System;
using System.Runtime.InteropServices;

namespace GTASessionBot.Windows_Libraries {

    /// <summary>
    /// Contains information about a simulated mouse event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput {
        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, 
        /// depending on the value of the dwFlags member. Absolute data is specified as the x coordinate of the mouse; 
        /// relative data is specified as the number of pixels moved.
        /// </summary>
        public int dx;


        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, 
        /// depending on the value of the dwFlags member. Absolute data is specified as the y coordinate of the mouse; 
        /// relative data is specified as the number of pixels moved.
        /// </summary>
        public int dy;


        /// <summary>
        /// If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. 
        /// A positive value indicates that the wheel was rotated forward, away from the user; 
        /// a negative value indicates that the wheel was rotated backward, toward the user. 
        /// One wheel click is defined as WHEEL_DELTA, which is 120.
        /// </summary>
        public int mouseData;

        /// <summary>
        /// A set of bit flags that specify various aspects of mouse motion and button clicks. 
        /// The bits in this member can be any reasonable combination of the following values.
        /// The bit flags that specify mouse button status are set to indicate changes in status, 
        /// not ongoing conditions.For example, if the left mouse button is pressed and held down, 
        /// MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions.
        /// Similarly, MOUSEEVENTF_LEFTUP is set only when the button is first released.
        /// </summary>
        public int dwFlags;


        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public int time;


        /// <summary>
        /// An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public IntPtr dwExtraInfo;
    }
}
