using System;

namespace OnlineStore.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        public int ProductID { get; set; }

        public int CustomerID { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}