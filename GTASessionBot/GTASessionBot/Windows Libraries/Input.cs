using System.Runtime.InteropServices;

namespace GTASessionBot.Windows_Libraries
{
    [StructLayout(LayoutKind.Explicit)]
    struct Input
    {

        /// <summary>
        /// The type of the input event. This member can be one of the following values.
        /// <list type="bullet">
        /// <item>
        /// <description>0: The event is a mouse event. Use the mi structure of the union.</description>
        /// </item>
        /// <item>
        /// <description>1: The event is a keyboard event. Use the ki structure of the union.</description>
        /// </item>
        /// <item>
        /// <description>2: The event is a hardware event. Use the hi structure of the union.</description>
        /// </item>
        /// </list>
        /// </summary>
        [FieldOffset(0)]
        public int type;


        /// <summary>
        /// The information about a simulated mouse event.
        /// </summary>
        [FieldOffset(4)]
        public MouseInput mouseInput;


        /// <summary>
        /// The information about a simulated keyboard event.
        /// </summary>
        [FieldOffset(4)]
        public KeyboardInput keyboardInput;


        /// <summary>
        /// The information about a simulated hardware event.
        /// </summary>
        [FieldOffset(4)]
        public HardwareInput hardwareInput;
    }
}
