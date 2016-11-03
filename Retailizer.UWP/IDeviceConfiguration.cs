using System;

namespace Retailizer.UWP
{
    internal interface IDeviceConfiguration
    {
        Guid DeviceId { get; }
        string DeviceIdString { get; }
        string DeviceKey { get; }
    }
}