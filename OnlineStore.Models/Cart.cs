using System;

namespace OnlineStore.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}