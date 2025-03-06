using System;

namespace YVR.AndroidDevice.Core
{
    [Flags]
    public enum AJCWrapMode
    {
        None = 0,
        Log = 1 << 1,
        Timer = 1 << 2,
    }
}