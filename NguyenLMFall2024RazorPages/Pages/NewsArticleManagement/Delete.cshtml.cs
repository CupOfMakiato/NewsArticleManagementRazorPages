using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.NewsArticleManagement
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IHubContext<NewsHub> _hubContext;

        public string? Message { get; set; }

        public DeleteModel(INewsArticleService newsArticleService, IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public IActionResult OnGet(string id)
        {
            if (!CheckSession())
                return RedirectToPage("/Authentication/Login");

            if (id == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            var newsArticle = _newsArticleService.GetNewsArticleById(id);

            if (newsArticle == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            else
            {
                NewsArticle = newsArticle;
            }
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            string? id = Request.Form["id"];
            if (!CheckSession())
                return RedirectToPage("/Authentication/Login");

            if (string.IsNullOrEmpty(id))
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            try
            {
                var loginSession = HttpContext.Session.GetString("LoginSession");
                var systemAccountId = JsonSerializer.Deserialize<SystemAccount>(loginSession)?.AccountId;
                var newsArticle = _newsArticleService.GetNewsArticleById(id);

                if (newsArticle == null)
                {
                    Message = "Not Found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                NewsArticle = newsArticle;

                if ((bool)!newsArticle.NewsStatus)
                {
                    Message = "This article has already been deleted";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                // Soft delete by setting the NewsStatus to false
                NewsArticle.NewsStatus = false;

                // Assuming Tags is a collection of Tags, get the selected tag IDs
                List<int> selectedTagIds = NewsArticle.Tags.Select(t => t.TagId).ToList();

                _newsArticleService.UpdateNewsArticle(NewsArticle, selectedTagIds); // Pass selectedTagIds here

                Message = "Delete successfully!";
                ModelState.AddModelError(string.Empty, Message);

                _hubContext.Clients.All.SendAsync("ArticleModified", systemAccountId).Wait();

                return Page();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }

        public bool CheckSession()
        {
            var loginAccount = HttpContext.Session.GetString("LoginSession");
            if (loginAccount != null)
            {
                var account = JsonSerializer.Deserialize<SystemAccount>(loginAccount);
                return account != null && account.AccountRole == 1;
            }
            return false;
        }
    }
}
