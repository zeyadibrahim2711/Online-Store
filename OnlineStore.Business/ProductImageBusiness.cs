using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class ProductImageBusiness
    {
        private readonly ProductImageDataAccess _productImageDataAccess;
        private readonly ProductCatalogDataAccess _productCatalogDataAccess;

        public ProductImageBusiness()
        {
            _productImageDataAccess = new ProductImageDataAccess();
            _productCatalogDataAccess = new ProductCatalogDataAccess();
        }
        private bool IsValidImage(ProductImage image)
        {
            return image != null &&
                   !string.IsNullOrWhiteSpace(image.ImageURL) &&
                   image.Order >= 0;
        }

        public int Add(ProductImage image)
        {
            if (!IsValidImage(image) ||
                image.ProductID <= 0 ||
                !_productCatalogDataAccess.Exists(image.ProductID))
                return -1;

            return _productImageDataAccess.Add(image);
        }
        public bool Update(ProductImage image)
        {
            if (!IsValidImage(image) ||
                image.ImageID <= 0 ||
                !_productImageDataAccess.Exists(image.ImageID))
                return false;

            return _productImageDataAccess.Update(image);
        }
        public bool Delete(int imageId)
        {
            if (imageId <= 0 ||!_productImageDataAccess.Exists(imageId))
                return false;
            
            return _productImageDataAccess.Delete(imageId);
        }
        public ProductImage GetByImageId(int imageId)
        {
            if (imageId <= 0)
                return null;

            return _productImageDataAccess.GetByImageId(imageId);
        }
        public List<ProductImage> GetByProductId(int productId)
        {
            if (!_productCatalogDataAccess.Exists(productId))
                return new List<ProductImage>();

            return _productImageDataAccess.GetByProductId(productId);
        }
    }
}