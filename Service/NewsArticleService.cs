using BusinessObject.Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        public NewsArticleService(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public void DeleteNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.DeleteNewsArticle(newsArticle);
        }

        public List<NewsArticle> GetAllNewsArticle()
        {
            return _newsArticleRepository.GetAllNewsArticle();
        }

        public NewsArticle? GetNewsArticleById(string id)
        {
            return _newsArticleRepository.GetNewsArticleById(id);
        }

        public void AddNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds)
        {
            _newsArticleRepository.AddNewsArticle(newsArticle, selectedTagIds);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle, List<int> selectedTagIds)
        {
            _newsArticleRepository.UpdateNewsArticle(newsArticle, selectedTagIds);
        }

        public void SoftDeleteNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.SoftDeleteNewsArticle(newsArticle);
        }
    }
}
