using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.NewsArticleManagement
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public string? Message { get; set; }

        public EditModel(INewsArticleService newsArticleService, ICategoryService categoryService,
                         ISystemAccountService systemAccountService, ITagService tagService,
                         IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        [BindProperty]
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        public IActionResult OnGet(string id)
        {
            if (!CheckSession())
                return RedirectToPage("Authentication/Login");

            if (id != null)
            {
                var newsarticle = _newsArticleService.GetAllNewsArticle().FirstOrDefault(m => m.NewsArticleId == id);
                if (newsarticle != null)
                {
                    NewsArticle = newsarticle;
                    Status = NewsArticle.NewsStatus is true ? "Active" : "Inactive";
                    SelectedTagIds = NewsArticle.Tags.Select(t => t.TagId).ToList();
                }
                else
                {
                    Message = "Article does not exist";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }
            }

            var activeCategories = _categoryService.GetAllCategory().Where(c => (bool)c.IsActive == true).ToList();
            ViewData["CategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryDesciption");
            ViewData["CreatedById"] = new SelectList(_systemAccountService.GetAllAccount(), "AccountId", "AccountName");
            ViewData["Tags"] = new MultiSelectList(_tagService.GetAllTag(), "TagId", "TagName", SelectedTagIds);

            return Page();
        }

        public IActionResult OnPost()
        {
            string? id = Request.Form["id"];
            if (!CheckSession())
                return RedirectToPage("Authentication/Login");

            try
            {
                var systemAccountId = JsonSerializer.Deserialize<SystemAccount>(HttpContext.Session.GetString("LoginSession")).AccountId;
                if (string.IsNullOrEmpty(id))
                {
                    NewsArticle.NewsArticleId = Guid.NewGuid().ToString().Substring(0, 20);
                    NewsArticle.NewsStatus = Status.Trim().ToLower() == "active";
                    NewsArticle.CreatedDate = DateTime.Now;
                    NewsArticle.CreatedById = systemAccountId;

                    _newsArticleService.AddNewsArticle(NewsArticle, SelectedTagIds);
                    _hubContext.Clients.All.SendAsync("ArticleModified", systemAccountId);

                    Message = "Create successfully!";
                    ModelState.AddModelError(string.Empty, Message);
                }
                else
                {
                    var newsarticle = _newsArticleService.GetNewsArticleById(id);
                    if (newsarticle == null)
                    {
                        Message = "Not Found";
                        ModelState.AddModelError(string.Empty, Message);
                        return Page();
                    }

                    NewsArticle.CreatedDate = newsarticle.CreatedDate;
                    NewsArticle.ModifiedDate = DateTime.Now;
                    NewsArticle.CreatedById = newsarticle.CreatedById;
                    NewsArticle.NewsStatus = Status.Trim().ToLower() == "active";

                    // Ensure SelectedTagIds are passed here
                    _newsArticleService.UpdateNewsArticle(NewsArticle, SelectedTagIds);
                    _hubContext.Clients.All.SendAsync("ArticleModified", systemAccountId);

                    Message = "Update successfully!";
                    ModelState.AddModelError(string.Empty, Message);
                }

                var activeCategories = _categoryService.GetAllCategory().Where(c => (bool)c.IsActive == true).ToList();
                ViewData["CategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryDesciption");
                ViewData["CreatedById"] = new SelectList(_systemAccountService.GetAllAccount(), "AccountId", "AccountName");
                ViewData["Tags"] = new MultiSelectList(_tagService.GetAllTag(), "TagId", "TagName", SelectedTagIds);

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
                if (account != null && account.AccountRole == 1)
                    return true;
            }
            return false;
        }
    }
}
