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

    
        public int Add(Shipping shipping)
        {
            if (shipping == null ||
                shipping.OrderID <= 0 ||
                !_orderDataAccess.Exists(shipping.OrderID))
            {
                return -1;
            }
            
            if (!_paymentDataAccess.HasSuccessfulPayment(shipping.OrderID)||
                _shippingDataAccess.ExistsActiveByOrderId(shipping.OrderID))
                
                return -1;
            
            return _shippingDataAccess.Add(shipping);
        }
        public bool Update(Shipping shipping)
        {
            if (shipping == null ||
                shipping.ShippingID <= 0 ||
                !_shippingDataAccess.Exists(shipping.ShippingID))
            {
                return false;
            }

            Shipping existingShipping =
                _shippingDataAccess.GetById(shipping.ShippingID);

            if (existingShipping.Status == Shipping.ShippingStatus.Pending &&
                shipping.Status != Shipping.ShippingStatus.Shipped &&
                shipping.Status != Shipping.ShippingStatus.Cancelled)
            {
                return false;
            }

            if (existingShipping.Status == Shipping.ShippingStatus.Shipped &&
                shipping.Status != Shipping.ShippingStatus.InTransit &&
                shipping.Status != Shipping.ShippingStatus.Cancelled)
            {
                return false;
            }

            if (existingShipping.Status == Shipping.ShippingStatus.InTransit &&
                shipping.Status != Shipping.ShippingStatus.Delivered &&
                shipping.Status != Shipping.ShippingStatus.Cancelled)
            {
                return false;
            }

            if (existingShipping.Status == Shipping.ShippingStatus.Delivered ||
                existingShipping.Status == Shipping.ShippingStatus.Cancelled)
            {
                return false;
            }

            if (shipping.Status == Shipping.ShippingStatus.Delivered &&
                !shipping.ActualDeliveryDate.HasValue)
            {
                return false;
            }

            if (shipping.Status != Shipping.ShippingStatus.Delivered)
                shipping.ActualDeliveryDate = null;

            return _shippingDataAccess.Update(shipping);
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