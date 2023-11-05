using System;
using System.Collections;

namespace DeviceConfiguration
{
    public class LedConfiguration
    {
        public string Type { get; set; }
        public bool IsDimmable { get; set; }
        public LedSetting[] Settings { get; set; }
    }

    public class LedSetting
    {
        public DateTime MinRange { get; set; }
        public DateTime MaxRange { get; set; }
        public int[] RGB { get; set; }
        public double Brightness { get; set; } = 0.5;
    }
}
