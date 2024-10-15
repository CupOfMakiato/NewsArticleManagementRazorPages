using BusinessObject.Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }

        public void DeleteCategory(Category category)
        {
            _categoryRepository.DeleteCategory(category);
        }

        public List<Category> GetAllCategory() => _categoryRepository.GetAllCategory();

        public Category? GetCategoryById(short id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.UpdateCategory(category);
        }
    }
}
