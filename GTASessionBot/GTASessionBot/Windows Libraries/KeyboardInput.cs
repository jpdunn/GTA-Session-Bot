using System;
using System.Runtime.InteropServices;

namespace GTASessionBot.Windows_Libraries {

    /// <summary>
    /// Contains information about a simulated keyboard event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput {

        /// <summary>
        /// A virtual-key code. The code must be a value in the range 1 to 254. 
        /// If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.
        /// </summary>
        public short wVk;


        /// <summary>
        /// A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, 
        /// wScan specifies a Unicode character which is to be sent to the foreground application.
        /// </summary>
        public short wScan;


        /// <summary>
        /// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
        /// <list type="bullet">
        /// <item>
        /// <description>0x0001: If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).</description>
        /// </item>
        /// <item>
        /// <description>0x0002: If specified, the key is being released. If not specified, the key is being pressed.</description>
        /// </item>
        /// <item>
        /// <description>0x0008: If specified, wScan identifies the key and wVk is ignored.</description>
        /// </item>
        /// <item>
        /// <description>0x0004: If specified, the system synthesizes a VK_PACKET keystroke. 
        /// The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. 
        /// For more information, see the Remarks section.</description>
        /// </item>
        /// </list>
        /// </summary>
        public int dwFlags;


        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.
        /// </summary>
        public int time;


        /// <summary>
        /// An additional value associated with the keystroke.
        /// </summary>
        public IntPtr dwExtraInfo;
    }
}
