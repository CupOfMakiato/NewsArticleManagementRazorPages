using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.CategoryManagement
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
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
            if (id != null)
            {
                var category = _categoryService.GetAllCategory().FirstOrDefault(m => m.CategoryId == id);
                if (category == null)
                {
                    Message = "Not Found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }
                Category = category;
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            var id = Request.Form["id"];
            if (!CheckSession())
                return RedirectToPage("/Authentication/Login");

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _categoryService.AddCategory(Category);


                    Message = "Create successfully!";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                var category = _categoryService.GetAllCategory().FirstOrDefault(c => c.CategoryId.ToString() == id);
                if (category == null)
                {
                    Message = "Not Found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();

                }

                _categoryService.UpdateCategory(Category);

                Message = "Update successfully!";
                ModelState.AddModelError(string.Empty, Message);

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

