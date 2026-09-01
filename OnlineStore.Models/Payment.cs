using System;

namespace OnlineStore.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }

        public int OrderID { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}