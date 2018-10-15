using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.Web.Models.WaterSystem
{
    public static class Water
    {
        public enum SoilStatus
        {
            Wet,
            Dry
        }
        public enum AutoWateringStatus
        {
            AlreadyRunning,
            Running,
            Stopped,
            None
        }

    }
}
