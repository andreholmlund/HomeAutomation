using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAutomation.Web.Models.WaterSystem;
using HomeAutomation.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Unosquare.Swan.Formatters;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace HomeAutomation.Web.Controllers
{
    [Route("api/[controller]")]
    public class WaterController : Controller
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


        private readonly IRaspberryPiService _raspberryPiService;
        private bool OnRas;

        public WaterController(IRaspberryPiService raspberryPiService, IHostingEnvironment environment)
        {
            _raspberryPiService = raspberryPiService;
            OnRas = environment.IsProduction();
        }
        // GET api/values

        [HttpGet]
        [Route("SoilStatus")]
        public IActionResult SoilStatus()
        {
            if (OnRas)
            {
                var result = _raspberryPiService.GetSoilStatus(0);
                return Json(new { message = result.ToString() });
            }
            return Json(new { message = "In dev" });
        }

        // GET api/values/5
        [HttpGet("ToggleAutoWater")]
        public async Task<ActionResult> Get(bool value)
        {
            if (OnRas)
            {
                if (value)
                {
                    return Json(new { message = _raspberryPiService.TurnOnAutoWater().ToString() });
                }
                return Json(new { message = _raspberryPiService.TurnOffAutoWater().ToString() });
            }

            return Json(new { message = "In Dev" });

        }


        [Route("TurnOnPump")]
        [HttpGet]
        public IActionResult TurnOnPump(int time)
        {
            if (time == 0)
            {
                return BadRequest("Invalid time parameter");
            }
            if (OnRas)
            {
                _raspberryPiService.TurnOnPump(time);
                return Json(new { message = $"The pump was running for {time} seconds" });
            }

            return Json(new { message = "In dev" });
        }
    }
}
