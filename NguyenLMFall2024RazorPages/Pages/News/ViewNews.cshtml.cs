using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Collections.Generic;
using System.Linq;

namespace NguyenLMFall2024RazorPages.Pages.News
{
    public class ViewNewsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ViewNewsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        // Property to hold the search query
        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public IList<NewsArticle> NewsArticle { get; set; } = default!;

        public IActionResult OnGet()
        {
            var allNewsArticles = _newsArticleService.GetAllNewsArticle()
                .Where(a => a.NewsStatus == true);

            // Apply search filter if search query is not null or empty
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                allNewsArticles = allNewsArticles.Where(a =>
                    a.NewsTitle.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.Headline.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.NewsContent.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.Tags.Any(t => t.TagName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));
            }

            NewsArticle = allNewsArticles.ToList();
            return Page();
        }
    }
}
