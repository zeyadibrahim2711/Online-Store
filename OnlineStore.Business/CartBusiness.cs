using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Collections.Generic;

namespace OnlineStore.Business
{
    public class CartBusiness
    {
        private readonly CartDataAccess _cartDataAccess;
        private readonly CustomerDataAccess _customerDataAccess;

        public CartBusiness()
        {
            _cartDataAccess = new CartDataAccess();
            _customerDataAccess = new CustomerDataAccess();
        }

        public int Add(Cart cart)
        {
            if (cart == null ||
                cart.CustomerID <= 0 ||
                !_customerDataAccess.Exists(cart.CustomerID) ||//not exist
                _cartDataAccess.GetByCustomerId(cart.CustomerID) != null)//exist
            {
                return -1;
            }

            return _cartDataAccess.Add(cart);
        }
        public Cart GetById(int cartId)
        {
            if (cartId <= 0)
                return null;

            return _cartDataAccess.GetById(cartId);
        }
        public Cart GetByCustomerId(int customerId)
        {
            if (customerId <= 0)
                return null;

            return _cartDataAccess.GetByCustomerId(customerId);
        }
        public List<Cart> GetAll()
        {
            return _cartDataAccess.GetAll();
        }
        
    }
}