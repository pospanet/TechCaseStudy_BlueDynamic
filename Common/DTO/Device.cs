using Newtonsoft.Json;
using System.Collections.Generic;

namespace Retailizer.Common.DTO
{
    public class Device
    {
        private string _id;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonIgnore]
        public string DeviceId
        {
            get { return _id; }
            set { _id = value; }
        }

        public string StoreId { get; set; }
        public string TenantId { get; set; }

        public List<DeviceCamera> Cameras { get; set; }

        public Device()
        {

        }

        public Device(string deviceId)
        {
            DeviceId = deviceId;
            Cameras = new List<DeviceCamera>();
        }
    }
}
