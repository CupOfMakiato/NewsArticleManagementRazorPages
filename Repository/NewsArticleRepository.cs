using BusinessObject.Entity;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public void DeleteNewsArticle(NewsArticle newsArticle)
        {
            NewsArticleDAO.getInstance().DeleteNewsArticle(newsArticle);
        }

        public NewsArticle? GetNewsArticleById(string id) => NewsArticleDAO.getInstance().GetNewsArticleById(id);

        public List<NewsArticle> GetAllNewsArticle() => NewsArticleDAO.getInstance().GetAllNewsArticle();

        public void AddNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds)
        {
            NewsArticleDAO.getInstance().AddNewsArticle(newsArticle, selectedTagIds);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds)
        {
            NewsArticleDAO.getInstance().UpdateNewsArticle(newsArticle, selectedTagIds);
        }

        public void SoftDeleteNewsArticle(NewsArticle newsArticle)
        {
            NewsArticleDAO.getInstance().SoftDeleteNewsArticle(newsArticle);
        }
    }
}
