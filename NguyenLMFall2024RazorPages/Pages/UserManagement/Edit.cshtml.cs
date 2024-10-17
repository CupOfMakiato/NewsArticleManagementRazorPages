using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.UserManagement
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public EditModel(ISystemAccountService systemAccountService)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!CheckSession())
                return RedirectToPage("/LoginPage");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var account = _systemAccountService.GetAllAccount().FirstOrDefault(a => a.AccountId == SystemAccount.AccountId);
                if (account == null)
                {
                    Message = "Not Found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (Role.Trim().ToLower() == "staff")
                    SystemAccount.AccountRole = 1;
                else if (Role.Trim().ToLower() == "lecturer")
                    SystemAccount.AccountRole = 2;
                else
                {
                    Message = "Role must be either 'Staff' or 'Lecturer'";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                _systemAccountService.UpdateAccount(SystemAccount);

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
                if (account != null && account.AccountRole == -1)
                    return true;
            }
            return false;
        }
    }
}
