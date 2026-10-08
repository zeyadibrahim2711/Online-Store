using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class PaymentBusiness
    {
        private readonly PaymentDataAccess _paymentDataAccess;
        private readonly OrderDataAccess _orderDataAccess;

        public PaymentBusiness()
        {
            _paymentDataAccess = new PaymentDataAccess();
            _orderDataAccess = new OrderDataAccess();
        }
        private bool IsValidPayment(Payment payment)
        {
            return payment != null &&
                   payment.OrderID > 0 &&
                   payment.Amount > 0 &&
                   _orderDataAccess.Exists(payment.OrderID);
        }
        public int Add(Payment payment)
        {
            if (!IsValidPayment(payment)||
                _paymentDataAccess.HasSuccessfulPayment(payment.OrderID))
                return -1;

            int paymentId = _paymentDataAccess.Add(payment);

            if (paymentId <= 0)
                return -1;

            if (payment.Status == Payment.PaymentStatus.Successful &&
                !_orderDataAccess.Confirm(payment.OrderID))
                return -1;

            return paymentId;
        }
        public bool Update(Payment payment)
        {
            if (payment.PaymentID <= 0||!IsValidPayment(payment))
            {
                return false;
            }

            if (_paymentDataAccess.IsSuccessful(payment.PaymentID) ||
                _orderDataAccess.IsConfirmed(payment.OrderID))
            {
                return false;
            }

            return _paymentDataAccess.Update(payment);
        }
        public Payment GetById(int paymentId)
        {
            if (paymentId <= 0)
                return null;

            return _paymentDataAccess.GetById(paymentId);
        }
        public List<Payment> GetByOrderId(int orderId)
        {
            if (orderId <= 0)
                return new List<Payment>();

            return _paymentDataAccess.GetByOrderId(orderId);
        }
        public List<Payment> GetAll()
        {
            return _paymentDataAccess.GetAll();
        }
    }
}