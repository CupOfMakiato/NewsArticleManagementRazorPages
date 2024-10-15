using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public partial class NewsArticleDAO
    {
        private static NewsArticleDAO instance = null;
        public NewsArticleDAO()
        {

        }
        public static NewsArticleDAO getInstance()
        {
            if (instance == null)
            {
                instance = new NewsArticleDAO();
            }
            return instance;
        }
        public List<NewsArticle> GetAllNewsArticle()
        {
            var listNewsArticles = new List<NewsArticle>();
            try
            {
                using var db = new FunewsManagementFall2024Context();
                listNewsArticles = db.NewsArticles
                                        .Include(a => a.Tags)
                                        .Include(a => a.Category)
                                        .Include(a => a.CreatedBy)
                                        .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listNewsArticles;
        }

        public NewsArticle? GetNewsArticleById(string id)
        {
            using var db = new FunewsManagementFall2024Context();
            NewsArticle? article = db.NewsArticles
                                     .Include(a => a.Tags)
                                     .Include(a => a.Category)
                                     .Include(a => a.CreatedBy)
                                     .FirstOrDefault(x => x.NewsArticleId.Equals(id));
            return article;
        }

        public void AddNewsArticle(NewsArticle p, List<int> selectedTagIds)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();


                var tags = db.Tags.Where(tag => selectedTagIds.Contains(tag.TagId)).ToList();

                // Assign the fetched tags to the NewsArticle's Tags collection
                p.Tags = tags;
                db.NewsArticles.Add(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateNewsArticle(NewsArticle p, List<int> selectedTagIds)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();

                // Retrieve the existing article including its tags
                var existingArticle = db.NewsArticles
                                        .Include(a => a.Tags) // Include tags to update the association
                                        .SingleOrDefault(x => x.NewsArticleId == p.NewsArticleId);

                if (existingArticle == null)
                {
                    throw new Exception("News Article not found!");
                }

                // Update the article's properties
                existingArticle.NewsTitle = p.NewsTitle;
                existingArticle.NewsSource = p.NewsSource;
                existingArticle.Headline = p.Headline;
                existingArticle.NewsContent = p.NewsContent;
                existingArticle.NewsStatus = p.NewsStatus;
                existingArticle.CategoryId = p.CategoryId;
                existingArticle.CreatedById = p.CreatedById;
                existingArticle.CreatedDate = p.CreatedDate;
                existingArticle.ModifiedDate = DateTime.Now;

                // Clear existing tags
                existingArticle.Tags.Clear();

                // Fetch and assign new tags based on selectedTagIds
                var newTags = db.Tags.Where(tag => selectedTagIds.Contains(tag.TagId)).ToList();
                existingArticle.Tags = newTags;

                // Mark the entity as modified
                db.Entry(existingArticle).State = EntityState.Modified;

                // Save changes
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void SoftDeleteNewsArticle(NewsArticle p)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                db.Entry<NewsArticle>(p).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteNewsArticle(NewsArticle p)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                var p1 = db.NewsArticles.SingleOrDefault(x => x.NewsArticleId == p.NewsArticleId);
                db.NewsArticles.Remove(p1);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
