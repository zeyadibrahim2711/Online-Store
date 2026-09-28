using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class OrderItemBusiness
    {
        private readonly OrderItemDataAccess _orderItemDataAccess;
        private readonly OrderDataAccess _orderDataAccess;
        private readonly ProductCatalogDataAccess _productCatalogDataAccess;

        public OrderItemBusiness()
        {
            _orderItemDataAccess = new OrderItemDataAccess();
            _orderDataAccess = new OrderDataAccess();
            _productCatalogDataAccess = new ProductCatalogDataAccess();
        }
        
        public bool Add(OrderItem item)
        {
            if (item.Quantity <= 0)
                return false;

            if (!_orderDataAccess.Exists(item.OrderID) || !_productCatalogDataAccess.Exists(item.ProductID))
                return false;

            Product product = _productCatalogDataAccess.GetById(item.ProductID);

            if (item.Quantity > product.QuantityInStock)
                return false;

            return _orderItemDataAccess.Add(item);
        }
        public bool Update(OrderItem item)
        {
            if (item.Quantity <= 0)
                return false;

            if (!_orderItemDataAccess.Exists(item.OrderID, item.ProductID))
                return false;
            

            Product product = _productCatalogDataAccess.GetById(item.ProductID);

            if (item.Quantity > product.QuantityInStock)
                return false;

            return _orderItemDataAccess.Update(item);
        }
        public bool Delete(int orderId, int productId)
        {
            if (!_orderItemDataAccess.Exists(orderId, productId))
                return false;

            return _orderItemDataAccess.Delete(orderId, productId);
        }
        public OrderItem GetById(int orderId, int productId)
        {
            if (orderId <= 0 || productId <= 0)
                return null;

            return _orderItemDataAccess.GetById(orderId, productId);
        }
        public List<OrderItem> GetAll()
        {
            return _orderItemDataAccess.GetAll();
        }
    }
}