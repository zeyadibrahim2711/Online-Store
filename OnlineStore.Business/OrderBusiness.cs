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
        private bool IsValidOrder(Order order)
        {
            return order != null &&
                   order.CustomerID > 0 &&
                   _customerDataAccess.Exists(order.CustomerID) &&
                   order.OrderItems != null &&
                   order.OrderItems.Count > 0;
        }
        private bool IsValidStatusTransition(
            Order.OrderStatus currentStatus,
            Order.OrderStatus newStatus)
        {
            switch (currentStatus)
            {
                case Order.OrderStatus.Pending:
                    return newStatus == Order.OrderStatus.Confirmed ||
                           newStatus == Order.OrderStatus.Cancelled;

                case Order.OrderStatus.Confirmed:
                    return newStatus == Order.OrderStatus.Shipped ||
                           newStatus == Order.OrderStatus.Cancelled;

                case Order.OrderStatus.Shipped:
                    return newStatus == Order.OrderStatus.Delivered ||
                           newStatus == Order.OrderStatus.Cancelled;

                case Order.OrderStatus.Delivered:
                case Order.OrderStatus.Cancelled:
                default:
                    return false;
            }
        }
        public int Add(Order order)
        {
            if (!IsValidOrder(order))
                return -1;

            return _orderDataAccess.Add(order);
        }
        
        public bool Update(Order order)
        {
            if (order == null ||
                order.OrderID <= 0 ||!_orderDataAccess.Exists(order.OrderID))
                return false;
            
            Order existingOrder = _orderDataAccess.GetById(order.OrderID);
            if (!IsValidStatusTransition(existingOrder.Status, order.Status))
                return false;
            
            return _orderDataAccess.Update(order);
        }
        public bool Cancel(int orderId)
        {
            if (orderId <= 0 ||
                !_orderDataAccess.Exists(orderId))
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