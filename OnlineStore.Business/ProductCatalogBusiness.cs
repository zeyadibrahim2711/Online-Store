using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Collections.Generic;
namespace OnlineStore.Business
{
    public class ProductCatalogBusiness
    {
        private readonly ProductCatalogDataAccess _productCatalogDataAccess;
        private readonly ProductCategoryDataAccess _productCategoryDataAccess;
        private readonly OrderItemDataAccess _orderItemDataAccess;

        public ProductCatalogBusiness()
        {
            _productCatalogDataAccess = new ProductCatalogDataAccess();
            _productCategoryDataAccess = new ProductCategoryDataAccess();
            _orderItemDataAccess = new OrderItemDataAccess();
        }

        public int Add(Product product)
        {
            if (!_productCategoryDataAccess.Exists(product.CategoryID))
                return -1;

            if (product.Price <= 0)
                return -1;

            if (product.QuantityInStock < 0)
                return -1;

            return _productCatalogDataAccess.Add(product);
        }
        public bool Update(Product product)
        {
            if (!_productCatalogDataAccess.Exists(product.ProductID))
                return false;

            if (!_productCategoryDataAccess.Exists(product.CategoryID))
                return false;

            if (product.Price <= 0)
                return false;

            if (product.QuantityInStock < 0)
                return false;

            return _productCatalogDataAccess.Update(product);
        }
        public bool Delete(int productId)
        {
            if (!_productCatalogDataAccess.Exists(productId))
                return false;

            if (_orderItemDataAccess.HasOrders(productId))
                return false;

            return _productCatalogDataAccess.Delete(productId);
        }
        public Product GetById(int productId)
        {
            if (productId <= 0)
                return null;

            return _productCatalogDataAccess.GetById(productId);
        }
        public List<Product> GetAll()
        {
            return _productCatalogDataAccess.GetAll();
        }
        
    }
}