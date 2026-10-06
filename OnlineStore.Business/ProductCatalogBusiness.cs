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
        private bool IsValidProduct(Product product)
        {
            return product != null &&
                   !string.IsNullOrWhiteSpace(product.ProductName) &&
                   product.Price > 0 &&
                   product.QuantityInStock >= 0 &&
                   _productCategoryDataAccess.Exists(product.CategoryID);
        }

        public int Add(Product product)
        {
            if (!IsValidProduct(product))
                return -1;

            return _productCatalogDataAccess.Add(product);
        }

        public bool Update(Product product)
        {
            if (!_productCatalogDataAccess.Exists(product.ProductID) ||
                !IsValidProduct(product))
                return false;

            return _productCatalogDataAccess.Update(product);
        }

        public bool Delete(int productId)
        {
            if (!_productCatalogDataAccess.Exists(productId) ||
                _orderItemDataAccess.HasOrders(productId))
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