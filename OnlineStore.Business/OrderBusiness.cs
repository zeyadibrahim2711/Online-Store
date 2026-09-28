using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Collections.Generic;
namespace OnlineStore.Business
{
    public class OrderBusiness
    {
        private readonly OrderDataAccess _orderDataAccess;
        private readonly CustomerDataAccess _customerDataAccess;

        public OrderBusiness()
        {
            _orderDataAccess = new OrderDataAccess();
            _customerDataAccess = new CustomerDataAccess();
        }

        public int Add(Order order)
        {
            if (!_customerDataAccess.Exists(order.CustomerID))
                return -1;
            if (order.OrderItems == null || order.OrderItems.Count == 0)
                return -1;
            return _orderDataAccess.Add(order);
        }
        
        public bool Update(Order order)
        {
            if (!_orderDataAccess.Exists(order.OrderID))
                return false;

            return _orderDataAccess.Update(order);
        }
        public bool Cancel(int orderId)
        {
            if (!_orderDataAccess.Exists(orderId))
                return false;

            return _orderDataAccess.Cancel(orderId);
        }
        public Order GetById(int orderId)
        {
            if (orderId <= 0)
                return null;

            return _orderDataAccess.GetById(orderId);
        }
        public List<Order> GetAll()
        {
            return _orderDataAccess.GetAll();
        }
        
    }
}