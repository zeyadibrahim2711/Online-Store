using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;

namespace OnlineStore.Business
{
    public class CartItemBusiness
    {
        private readonly CartItemDataAccess _cartItemDataAccess;
        private readonly CartDataAccess _cartDataAccess;
        private readonly ProductCatalogDataAccess _productDataAccess;

        public CartItemBusiness()
        {
            _cartItemDataAccess = new CartItemDataAccess();
            _cartDataAccess = new CartDataAccess();
            _productDataAccess = new ProductCatalogDataAccess();
        }

        public bool Add(CartItem cartItem)
        {
            if (cartItem == null ||
                cartItem.CartID <= 0 ||
                cartItem.ProductID <= 0 ||
                cartItem.Quantity <= 0)
            {
                return false;
            }

            if (!_cartDataAccess.IsCartExists(cartItem.CartID))
            {
                return false;
            }
            
            Product product = _productDataAccess.GetById(cartItem.ProductID);

            if (product == null ||
                cartItem.Quantity > product.QuantityInStock)
            {
                return false;
            }

            if (_cartItemDataAccess.IsProductExistsInCart(
                    cartItem.CartID,
                    cartItem.ProductID))
            {
                return false;
            }

            return _cartItemDataAccess.Add(cartItem);
        }
        public bool Update(CartItem cartItem)
        {
            if (cartItem == null ||
                cartItem.CartID <= 0 ||
                cartItem.ProductID <= 0 ||
                cartItem.Quantity <= 0)
            {
                return false;
            }

            Product product = _productDataAccess.GetById(cartItem.ProductID);

            if (product == null ||
                cartItem.Quantity > product.QuantityInStock ||
                !_cartItemDataAccess.IsProductExistsInCart(
                    cartItem.CartID,
                    cartItem.ProductID))
            {
                return false;
            }

            return _cartItemDataAccess.Update(cartItem);
        }
        public bool Delete(int cartId, int productId)
        {
            if (cartId <= 0 ||
                productId <= 0 ||
                !_cartItemDataAccess.IsProductExistsInCart(
                    cartId,
                    productId))
            {
                return false;
            }

            return _cartItemDataAccess.Delete(cartId, productId);
        }

        public CartItem GetById(int cartId, int productId)
        {
            if (cartId <= 0 || productId <= 0)
            {
                return null;
            }

            return _cartItemDataAccess.GetById(cartId, productId);
        }
        public List<CartItem> GetByCartId(int cartId)
        {
            if (cartId <= 0)
                return new List<CartItem>();

            return _cartItemDataAccess.GetByCartId(cartId);
        }
        public List<CartItem> GetByProductId(int productId)
        {
            if (productId <= 0)
                return new List<CartItem>();

            return _cartItemDataAccess.GetByProductId(productId);
        }
    }
}