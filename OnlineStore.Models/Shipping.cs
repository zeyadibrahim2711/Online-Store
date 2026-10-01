using System;

namespace OnlineStore.Models
{
    public class Shipping
    {
        public enum ShippingStatus
        {
            Pending,
            Shipped,
            InTransit,
            Delivered,
            Cancelled
        }

        public int ShippingID { get; set; }

        public int OrderID { get; set; }

        public string CarrierName { get; set; }

        public string TrackingNumber { get; set; }

        public ShippingStatus Status { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

    }
}