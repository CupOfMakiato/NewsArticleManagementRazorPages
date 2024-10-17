using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.TagManagement
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public string? Message { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (id == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            var tag = _tagService.GetAllTag().FirstOrDefault(m => m.TagId == id);

            if (tag == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            else
            {
                Tag = tag;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (!CheckSession())
                return RedirectToPage("/Authentication/Login");

            if (id == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            var tag = _tagService.GetAllTag().FirstOrDefault(t => t.TagId == id);
            if (tag == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            if (tag.NewsArticles.Count > 0)
            {
                Message = "Cannot delete tag because it is being used in news articles";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Tag = tag;
            _tagService.DeleteTag(Tag);

            Message = "Delete successfully!";
            ModelState.AddModelError(string.Empty, Message);

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
