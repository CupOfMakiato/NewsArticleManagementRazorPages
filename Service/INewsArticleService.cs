using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface INewsArticleService
    {
        void AddNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds);
        void DeleteNewsArticle(NewsArticle newsArticle);
        void SoftDeleteNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds);
        List<NewsArticle> GetAllNewsArticle();
        NewsArticle? GetNewsArticleById(string id);
    }
}
