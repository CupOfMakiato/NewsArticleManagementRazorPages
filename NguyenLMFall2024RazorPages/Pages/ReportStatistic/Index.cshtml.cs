using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.ReportStatistic
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;

        public IndexModel(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        [BindProperty]
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        public IActionResult OnGet()
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (StartDate.HasValue && EndDate.HasValue && ModelState.IsValid)
            {
                NewsArticles = _newsService.GetAllNewsArticle()
                    .Where(n => n.CreatedDate >= StartDate && n.CreatedDate <= EndDate)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();

                return Page();
            }
            return Page();
        }

        public bool CheckSession()
        {
            var loginAccount = HttpContext.Session.GetString("LoginSession");
            if (loginAccount != null)
            {
                var account = JsonSerializer.Deserialize<SystemAccount>(loginAccount);
                if (account != null && account.AccountRole == -1)
                    return true;
            }
            return false;
        }
    }
}

