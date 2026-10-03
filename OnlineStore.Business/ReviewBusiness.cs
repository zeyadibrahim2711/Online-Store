using System.Collections.Generic;
using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Configuration;

namespace OnlineStore.Business
{
    public class ReviewBusiness
    {
        private readonly ReviewDataAccess _reviewDataAccess;
        private readonly CustomerDataAccess _customerDataAccess;
        private readonly ProductCatalogDataAccess _productDataAccess;
        private readonly OrderDataAccess _orderDataAccess;

        public ReviewBusiness()
        {
            
            _reviewDataAccess = new ReviewDataAccess();
            _customerDataAccess = new CustomerDataAccess();
            _productDataAccess = new ProductCatalogDataAccess();
            _orderDataAccess = new OrderDataAccess();
        }

        public int Add(Review review)
        {
            if (review == null ||
                review.CustomerID <= 0 ||
                review.ProductID <= 0 ||
                review.Rating < 1 ||
                review.Rating > 5)
            {
                return -1;
            }

            if (!_customerDataAccess.Exists(review.CustomerID) ||
                !_productDataAccess.Exists(review.ProductID))
            {
                return -1;
            }

            if (_reviewDataAccess.ExistsByCustomerAndProduct(review.CustomerID, review.ProductID) ||
                !_orderDataAccess.HasPurchasedProduct(review.CustomerID, review.ProductID))
            {
                return -1;
            }

            return _reviewDataAccess.Add(review);
        }
        public bool Update(Review review)
        {
            if (review == null ||
                review.ReviewID <= 0 ||
                review.Rating < 1 ||
                review.Rating > 5)
            {
                return false;
            }

            if (!_reviewDataAccess.Exists(review.ReviewID))
                return false;

            return _reviewDataAccess.Update(review);
        }
        
        public bool Delete(int reviewId)
        {
            if (reviewId <= 0 ||
                !_reviewDataAccess.Exists(reviewId))
            {
                return false;
            }

            return _reviewDataAccess.Delete(reviewId);
        }
        public Review GetById(int reviewId)
        {
            if (reviewId <= 0)
                return null;

            return _reviewDataAccess.GetById(reviewId);
        }
        public List<Review> GetByProductId(int productId)
        {
            if (productId <= 0)
                return new List<Review>();

            return _reviewDataAccess.GetByProductId(productId);
        }
        public List<Review> GetByCustomerId(int customerId)
        {
            if (customerId <= 0)
                return new List<Review>();

            return _reviewDataAccess.GetByCustomerId(customerId);
        }
        
    }
}