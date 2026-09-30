using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class CustomerBusiness
    {
        private readonly CustomerDataAccess _customerDataAccess;
        private readonly OrderDataAccess _orderDataAccess;

        public CustomerBusiness()
        {
            _customerDataAccess = new CustomerDataAccess();
            _orderDataAccess = new OrderDataAccess();
        }

        public int Add(Customer customer)
        {
            Customer existingCustomer = _customerDataAccess.GetByEmail(customer.Email);

            if (existingCustomer != null)
                return -1;

            return _customerDataAccess.Add(customer);
        }

        public bool Delete(int customerId)
        {
            if (!_customerDataAccess.Exists(customerId) ||
                _orderDataAccess.HasOrdersByCustomerId(customerId))
                return false;

            return _customerDataAccess.Delete(customerId);
        }

        public bool Update(Customer customer)
        {
            if (!_customerDataAccess.Exists(customer.CustomerID) ||
                _customerDataAccess.EmailExistsForAnotherCustomer(customer.Email, customer.CustomerID))
                return false;

            return _customerDataAccess.Update(customer);
        }

        public Customer GetById(int customerId)
        {
            if (customerId <= 0)
                return null;

            return _customerDataAccess.GetById(customerId);
        }

        public List<Customer> GetAll()
        {
            return _customerDataAccess.GetAll();
        }
    }
}