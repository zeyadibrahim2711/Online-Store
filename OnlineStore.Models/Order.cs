using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public class Order
    {
        public enum OrderStatus
        {
            Pending,
            Confirmed,
            Shipped,
            Delivered,
            Cancelled
        }
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}