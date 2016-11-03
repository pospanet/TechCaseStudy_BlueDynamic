using System.Threading.Tasks;
using Microsoft.Azure.Devices.Common.Exceptions;
using Retailizer.Common.DTO;
using AzureDevices = Microsoft.Azure.Devices;

namespace Retailizer.Common.Services
{
    public class IotHubService
    {
        private readonly AzureDevices.RegistryManager _registryManager;

        public IotHubService(IAppConfiguration configuration)
        {
            _registryManager = AzureDevices.RegistryManager.CreateFromConnectionString(configuration.IotHubConnectionString);
        }

        public async Task<AzureDevices.Device> RegisterDevice(Device device)
        {
            AzureDevices.Device azureDevice;
            try
            {
                azureDevice = await _registryManager.AddDeviceAsync(new AzureDevices.Device(device.DeviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                azureDevice = await _registryManager.GetDeviceAsync(device.DeviceId);
            }

            return azureDevice;
        }
    }
}