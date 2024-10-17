using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.NewsArticleManagement
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        public string? SearchQuery { get; set; }

        public IActionResult OnGet(string? searchQuery)
        {
            if (!CheckSession())
                return RedirectToPage("Authentication/Login");

            // Store the search query in the model for the Razor Page
            SearchQuery = searchQuery;

            // Fetch all news articles
            var articles = _newsArticleService.GetAllNewsArticle();

            // Filter based on search query if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                articles = articles.Where(a =>
                    a.NewsTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.Headline.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.NewsContent.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.Tags.Any(t => t.TagName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    a.CreatedBy.AccountName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Assign filtered list to the model property
            NewsArticle = articles;
            return Page();
        }

        public bool CheckSession()
        {
            var loginAccount = HttpContext.Session.GetString("LoginSession");
            if (loginAccount != null)
            {
                var account = JsonSerializer.Deserialize<SystemAccount>(loginAccount);
                if (account != null && account.AccountRole == 1)
                    return true;
            }
            return false;
        }
    }
}
