using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.UserManagement
{
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public DeleteModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public string? Message { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your role")]
        public string Role { get; set; } = default!;

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

            var systemAccount = _systemAccountService.GetAllAccount().FirstOrDefault(m => m.AccountId == id);

            if (systemAccount == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            if (systemAccount.AccountRole == 1)
                Role = "Staff";
            else if (systemAccount.AccountRole == 2)
                Role = "Lecturer";
            else
            {
                Message = "Account has invalid role!";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            SystemAccount = systemAccount;
            return Page();
        }

        public IActionResult OnPost(short? id)
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (id == null)
            {
                Message = "Not Found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            try
            {
                var systemAccount = _systemAccountService.GetAllAccount().FirstOrDefault(m => m.AccountId == id);
                if (systemAccount == null)
                {
                    Message = "Not Found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (systemAccount.NewsArticles.Count > 0)
                {
                    Message = "Cannot delete account because this account has at least 1 news articles";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                _systemAccountService.DeleteAccount(systemAccount);

                Message = "Delete successfully!";
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
                if (account != null && account.AccountRole == -1)
                    return true;
            }
            return false;
        }
    }
}