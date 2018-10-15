using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAutomation.Web.Models.WaterSystem;

namespace HomeAutomation.Web.Services
{
    public interface IRaspberryPiService
    {
        Water.SoilStatus GetSoilStatus(int pin);
        void TurnOnPump(int time);
        Water.AutoWateringStatus IsAutoWateringRunning();
        Water.AutoWateringStatus TurnOffAutoWater();
        Water.AutoWateringStatus TurnOnAutoWater();
    }
}
