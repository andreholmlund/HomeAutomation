using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using HomeAutomation.Web.Helpers;
using HomeAutomation.Web.Models.WaterSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace HomeAutomation.Web.Services
{
    /// <inheritdoc />
    public class RaspberryPiService : IRaspberryPiService
    {
        private bool _autoWateringRunning;
        private string _jobId;

        public RaspberryPiService()
        {
        }
        /// <inheritdoc />
        public void TurnOnPump(int time = 1)
        {
            var pin = Helpers.Enums.GetPinFromIndex(7);
            pin.PinMode = GpioPinDriveMode.Output;
            pin.Write(GpioPinValue.Low);
            Thread.Sleep(TimeSpan.FromSeconds(time));
            pin.Write(GpioPinValue.High);
        }

        /// <inheritdoc />
        public Water.AutoWateringStatus IsAutoWateringRunning()
        {
            return _autoWateringRunning ? Water.AutoWateringStatus.Running : Water.AutoWateringStatus.Stopped;
        }

        /// <inheritdoc />
        public Water.AutoWateringStatus TurnOffAutoWater()
        {
            if (!string.IsNullOrEmpty(Enums.JobId))
            {
                BackgroundJob.Delete(Enums.JobId);
                Enums.JobId = null;
                return Water.AutoWateringStatus.Stopped;
            }

            return Water.AutoWateringStatus.None;
        }

        /// <inheritdoc />
        public Water.AutoWateringStatus TurnOnAutoWater()
        {
            try
            {
                if (string.IsNullOrEmpty(Enums.JobId))
                {

                    if (JobStorage.Current.GetMonitoringApi().JobDetails(Enums.JobId ?? "") == null)
                    {
                        var token = new JobCancellationToken(false);

                        Enums.JobId = BackgroundJob.Enqueue(() => RunAutoStart(token));
                        return Water.AutoWateringStatus.Running;
                    }

                    return Water.AutoWateringStatus.AlreadyRunning;
                }

                return Water.AutoWateringStatus.AlreadyRunning;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Water.AutoWateringStatus.Stopped;
            }

        }

        public void RunAutoStart(IJobCancellationToken cancellationToken)
        {
            var isRunning = true;
            while (isRunning)
            {
                try
                {

                    if (GetSoilStatus(0) == Water.SoilStatus.Dry)
                    {
                        TurnOnPump(3);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(6));
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    isRunning = false;
                }
            }
        }

        /// <inheritdoc />
        public Water.SoilStatus GetSoilStatus(int pin = 0)
        {

            var raspin = Helpers.Enums.GetPinFromIndex(pin);

            raspin.PinMode = GpioPinDriveMode.Input;

            var isOn = raspin.Read();

            return isOn ? Water.SoilStatus.Dry : Water.SoilStatus.Wet;
        }


    }
}
