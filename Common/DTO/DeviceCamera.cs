namespace Retailizer.Common.DTO
{
    public class DeviceCamera
    {
        public string Id { get; set; }
        public string EventType { get; set; }

        /// <summary>
        ///     If confidence is lower than ConfidenceLimit, new person is created
        /// </summary>
        public double ConfidenceLimit { get; set; }

        public DeviceCamera()
        {
        }

        public DeviceCamera(string id)
        {
            Id = id;
        }

        public DeviceCamera(string id, string eventType)
        {
            Id = id;
            EventType = eventType;
        }
    }
}