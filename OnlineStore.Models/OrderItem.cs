using System;

namespace OnlineStore.Models
{
    public class OrderItem
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ReservationStatus { get; set; }

        public DateTime? ReservationExpiresAt { get; set; }
    }
}