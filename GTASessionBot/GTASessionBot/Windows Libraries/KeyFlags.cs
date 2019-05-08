using System;

namespace GTASessionBot.Windows_Libraries
{
    [Flags]
    public enum KeyFlags
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        UniCode = 0x0004,
        ScanCode = 0x0008
    }
}
