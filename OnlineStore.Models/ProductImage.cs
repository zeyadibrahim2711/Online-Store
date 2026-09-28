namespace OnlineStore.Models
{
    public class ProductImage
    {
        public int ImageID { get; set; }

        public int ProductID { get; set; }

        public string ImageURL { get; set; }
        
        public int Order { get; set; }
    }
}