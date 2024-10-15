using BusinessObject.Entity;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public void AddCategory(Category category)
        {
            CategoryDAO.getInstance().AddCategory(category);
        }

        public void DeleteCategory(Category category)
        {
            CategoryDAO.getInstance().DeleteCategory(category);
        }

        public List<Category> GetAllCategory() => CategoryDAO.getInstance().GetAllCategory();

        public Category? GetCategoryById(short id)
        {
            return CategoryDAO.getInstance().GetCategoryById(id);
        }

        public void UpdateCategory(Category category)
        {
            CategoryDAO.getInstance().UpdateCategory(category);
        }
    }
}
