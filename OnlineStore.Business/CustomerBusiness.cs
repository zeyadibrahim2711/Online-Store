using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Net.Mail;
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
        private bool IsValidCustomer(Customer customer)
        {
            if (customer == null ||
                string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                string.IsNullOrWhiteSpace(customer.Username) ||
                string.IsNullOrWhiteSpace(customer.Password))
                return false;

            try
            {
                new MailAddress(customer.Email);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public int Add(Customer customer)
        {
            if (!IsValidCustomer(customer))
                return -1;
            
            Customer existingCustomer = _customerDataAccess.GetByEmail(customer.Email);

            if (existingCustomer != null ||
                _customerDataAccess.GetByUsername(customer.Username) != null)
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
            if (!IsValidCustomer(customer))
                return false;
            if (!_customerDataAccess.Exists(customer.CustomerID) ||
                _customerDataAccess.EmailExistsForAnotherCustomer(
                    customer.Email, customer.CustomerID) ||
                _customerDataAccess.UsernameExistsForAnotherCustomer(
                    customer.Username, customer.CustomerID))
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