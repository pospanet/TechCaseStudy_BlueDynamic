using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Retailizer.Common.DTO
{
    public enum VisitStatus
    {
        None,
        Entered,
        Left
    }


    public class Visit : TableEntity
    {
        public string Id
        {
            get { return RowKey; }
            set { RowKey = value; }
        }


        public string TenantId { get; set; }

        public string StoreId { get; set; }

        public string PersonId { get; set; }

        public string SuggestedPersonId { get; set; }
        public double Confidence { get; set; }
        public DateTime EnterOn { get; set; }
        public DateTime PaymentOn { get; set; }
        public DateTime LeaveOn { get; set; }
        public VisitStatus VisitStatus { get; set; }

        public Visit()
        {
            RowKey = Guid.NewGuid().ToString();
        }

        public Visit(Event _event)
        {
            Id = Guid.NewGuid().ToString();
            TenantId = _event.TenantId;
            StoreId = _event.StoreId;
            PersonId = _event.PersonId;
            SuggestedPersonId = _event.SuggestedPersonId;
            Confidence = _event.Confidence;

            switch (_event.EventType)
            {
                case Event.EVENT_TYPE_ENTER:
                    EnterOn = _event.TimeStamp;
                    VisitStatus = VisitStatus.Entered;
                    break;
                case Event.EVENT_TYPE_PAYMENT:
                    PaymentOn = _event.TimeStamp;
                    VisitStatus = VisitStatus.Entered;
                    break;
                case Event.EVENT_TYPE_LEAVE:
                    LeaveOn = _event.TimeStamp;
                    VisitStatus = VisitStatus.Left;
                    break;
                default:
                    VisitStatus = VisitStatus.None;
                    break;
            }
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
            OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            string tenantId, storeId, personId;
            DecodePartitionKey(PartitionKey, out tenantId, out storeId, out personId);
            TenantId = tenantId;
            StoreId = storeId;
            PersonId = personId;
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            PartitionKey = EncodePartitionKey(TenantId, StoreId, PersonId);
            return base.WriteEntity(operationContext);
        }

        private static string EncodePartitionKey(string tenantId, string storeId, string personId)
        {
            return $"{tenantId}#{storeId}#{personId}";
        }

        private static void DecodePartitionKey(string partitionKey, out string tenantId, out string storeId,
            out string personId)
        {
            char[] delimiterChars = {'#'};

            string[] words = partitionKey.Split(delimiterChars);
            tenantId = words[0];
            storeId = words[1];
            personId = words[2];
        }
    }
}