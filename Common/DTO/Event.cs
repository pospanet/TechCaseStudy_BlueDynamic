using System;

namespace Retailizer.Common.DTO
{
    public class Event
    {
        public const string EVENT_TYPE_ENTER = "enter";
        public const string EVENT_TYPE_PAYMENT = "payment";
        public const string EVENT_TYPE_LEAVE = "leave";

        public string id { get; set; }


        public string VisitId { get; set; }

        public string EventType { get; set; }

        public string DeviceId { get; set; }

        public string CameraId { get; set; }

        public string TenantId { get; set; }

        public string StoreId { get; set; }

        public string PersonId { get; set; }

        public DateTime TimeStamp { get; set; }

        public string SuggestedPersonId { get; set; }

        public double Confidence { get; set; }
        public VisitPerson Person { get; set; }
    }

    public class VisitPerson
    {
        public double Age { get; set; }
        public string Gender { get; set; }
        public double Smile { get; set; }
    }
}