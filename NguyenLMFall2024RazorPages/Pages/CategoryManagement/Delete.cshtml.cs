using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.CategoryManagement
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public string? Message { get; set; }

        public IActionResult OnGet(short? id)
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (id == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            var category = _categoryService.GetAllCategory().FirstOrDefault(m => m.CategoryId == id);

            if (category == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            else
            {
                Category = category;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            string? id = Request.Form["id"];

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var category = _categoryService.GetAllCategory().FirstOrDefault(c => c.CategoryId.ToString() == id);
            if (category == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            if (category.NewsArticles.Count > 0)
            {
                Message = "Cannot delete category which belongs to at least 1 article";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Category = category;
            _categoryService.DeleteCategory(Category);

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
