using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.TagManagement
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public string? Message { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_tagService.GetTagById(Tag.TagId) != null)
            {
                Message = "Tag ID already exists";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            _tagService.AddTag(Tag);

            Message = "Create successfully!";
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
