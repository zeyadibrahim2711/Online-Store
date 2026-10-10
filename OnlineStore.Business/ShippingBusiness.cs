using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class ShippingBusiness
    {
        private readonly ShippingDataAccess _shippingDataAccess;
        private readonly OrderDataAccess _orderDataAccess;
        private readonly PaymentDataAccess _paymentDataAccess;

        public ShippingBusiness()
        {
            _shippingDataAccess = new ShippingDataAccess();
            _orderDataAccess = new OrderDataAccess();
            _paymentDataAccess = new PaymentDataAccess();
        }
        private bool IsValidShipping(Shipping shipping)
        {
            return shipping != null &&
                   shipping.OrderID > 0;
        }
        private bool IsValidStatusTransition(
            Shipping.ShippingStatus currentStatus,
            Shipping.ShippingStatus newStatus)
        {
            switch (currentStatus)
            {
                case Shipping.ShippingStatus.Pending:
                    return newStatus == Shipping.ShippingStatus.Shipped ||
                           newStatus == Shipping.ShippingStatus.Cancelled;

                case Shipping.ShippingStatus.Shipped:
                    return newStatus == Shipping.ShippingStatus.InTransit ||
                           newStatus == Shipping.ShippingStatus.Cancelled;

                case Shipping.ShippingStatus.InTransit:
                    return newStatus == Shipping.ShippingStatus.Delivered ||
                           newStatus == Shipping.ShippingStatus.Cancelled;

                case Shipping.ShippingStatus.Delivered:
                case Shipping.ShippingStatus.Cancelled:
                default:
                    return false;
            }
        }
    
        public int Add(Shipping shipping)
        {
            if (!IsValidShipping(shipping) ||
                !_orderDataAccess.Exists(shipping.OrderID))
            {
                return -1;
            }
            
            if (!_paymentDataAccess.HasSuccessfulPayment(shipping.OrderID)||
                _shippingDataAccess.ExistsActiveByOrderId(shipping.OrderID))
                
                return -1;
            
            return _shippingDataAccess.Add(shipping);
        }
        public bool Update(Shipping newShipping)
        {
            if (newShipping == null ||
                newShipping.ShippingID <= 0 ||
                !_shippingDataAccess.Exists(newShipping.ShippingID))
            {
                return false;
            }

            Shipping existingShipping =
                _shippingDataAccess.GetById(newShipping.ShippingID);

            if (existingShipping == null ||
                !IsValidStatusTransition(existingShipping.Status, newShipping.Status))
            {
                return false;
            }
            
            if (newShipping.Status == Shipping.ShippingStatus.Delivered &&
                !newShipping.ActualDeliveryDate.HasValue)
            {
                return false;
            }

            if (newShipping.Status != Shipping.ShippingStatus.Delivered)
                newShipping.ActualDeliveryDate = null;

            return _shippingDataAccess.Update(newShipping);
        }
        public bool Cancel(int shippingId)
        {
            if (shippingId <= 0 ||
                !_shippingDataAccess.Exists(shippingId))
            {
                return false;
            }

            Shipping shipping = _shippingDataAccess.GetById(shippingId);

            if (shipping.Status == Shipping.ShippingStatus.Delivered ||
                shipping.Status == Shipping.ShippingStatus.Cancelled)
            {
                return false;
            }

            return _shippingDataAccess.Cancel(shippingId);
        }
        public Shipping GetById(int shippingId)
        {
            if (shippingId <= 0)
                return null;

            return _shippingDataAccess.GetById(shippingId);
        }
        public Shipping GetByOrderId(int orderId)
        {
            if (orderId <= 0)
                return null;

            return _shippingDataAccess.GetByOrderId(orderId);
        }
        public List<Shipping> GetAll()
        {
            return _shippingDataAccess.GetAll();
        }
        
    }
}