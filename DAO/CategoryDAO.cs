using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;

        public CategoryDAO()
        {

        }
        public static CategoryDAO getInstance()
        {
            if (instance == null)
            {
                instance = new CategoryDAO();
            }
            return instance;
        }
        public List<Category> GetAllCategory()
        {
            var listCategories = new List<Category>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                listCategories = context.Categories.Include(c => c.ParentCategory).Include(c => c.InverseParentCategory).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listCategories;
        }

        public void AddCategory(Category category)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                if (category.ParentCategoryId.HasValue)
                {
                    var parentCategory = db.Categories.Find(category.ParentCategoryId.Value);
                    if (parentCategory != null)
                    {
                        category.ParentCategory = parentCategory;
                    }
                }
                db.Categories.Add(category);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                if (category.ParentCategoryId.HasValue)
                {
                    var parentCategory = db.Categories.Find(category.ParentCategoryId.Value);
                    if (parentCategory != null)
                    {
                        category.ParentCategory = parentCategory;
                    }
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCategory(Category category)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();

                // Check if the category is associated with any news articles
                bool isLinkedToArticles = db.NewsArticles.Any(na => na.CategoryId == category.CategoryId);
                if (isLinkedToArticles)
                {
                    throw new InvalidOperationException("This category is linked to one or more news articles and cannot be deleted.");
                }

                var categoryToDelete = db.Categories.Include(c => c.InverseParentCategory)
                                                    .SingleOrDefault(c => c.CategoryId == category.CategoryId);

                if (categoryToDelete != null)
                {
                    // Optionally handle the InverseParentCategory (child categories) here if needed
                    db.Categories.Remove(categoryToDelete);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public Category? GetCategoryById(short categoryId)
        {
            using var db = new FunewsManagementFall2024Context();
            Category? category = db.Categories
                                     .FirstOrDefault(x => x.CategoryId.Equals(categoryId));
            return category;
        }
    }
}