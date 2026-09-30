using OnlineStore.DataAccess;
using OnlineStore.Models;
using System.Collections.Generic;

namespace OnlineStore.Business
{
    public class ProductCategoryBusiness
    {
        private readonly ProductCategoryDataAccess _productCategoryDataAccess;

        public ProductCategoryBusiness()
        {
            _productCategoryDataAccess = new ProductCategoryDataAccess();
        }

        public int Add(ProductCategory category)
        {
            if (_productCategoryDataAccess.ExistsByName(category.CategoryName))
                return -1;

            return _productCategoryDataAccess.Add(category);
        }

        public bool Update(ProductCategory category)
        {
            if (!_productCategoryDataAccess.Exists(category.CategoryID) ||
                _productCategoryDataAccess.NameExistsForAnotherCategory(
                    category.CategoryName, category.CategoryID))
                return false;

            return _productCategoryDataAccess.Update(category);
        }

        public bool Delete(int categoryId)
        {
            if (!_productCategoryDataAccess.Exists(categoryId) ||
                _productCategoryDataAccess.HasProducts(categoryId))
                return false;

            return _productCategoryDataAccess.Delete(categoryId);
        }

        public ProductCategory GetById(int categoryId)
        {
            if (categoryId <= 0)
                return null;

            return _productCategoryDataAccess.GetById(categoryId);
        }

        public List<ProductCategory> GetAll()
        {
            return _productCategoryDataAccess.GetAll();
        }
    }
}