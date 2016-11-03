using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Retailizer.Common.DTO;
using Retailizer.Common.Services;
using ADevice = Microsoft.Azure.Devices;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Retailizer.Controllers
{
    //[Route("api/[controller]")]
    public class DeviceController : Controller
    {
        private readonly IotHubService _iotHubService;

        public DeviceController(IotHubService iotHubService)
        {
            _iotHubService = iotHubService;
        }

        [Route("api/device")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Device device)
        {
            ADevice.Device registeredDevice = await _iotHubService.RegisterDevice(device);

            HttpContext.Response.StatusCode = 201;
            return Json(new
            {
                deviceKey = registeredDevice.Authentication.SymmetricKey.PrimaryKey,
                deviceId = registeredDevice.Id
            });
        }
    }
}