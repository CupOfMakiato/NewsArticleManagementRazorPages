using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.UserManagement
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public CreateModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public string? Message { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a Role")]
        public string Role { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!CheckSession())
                return RedirectToPage("/Authentication/Login");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var account = _systemAccountService.GetAllAccount().FirstOrDefault(a => a.AccountId == SystemAccount.AccountId);
                if (account != null)
                {
                    Message = "Account ID already exists";
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

                _systemAccountService.AddAccount(SystemAccount);

                Message = "Account created successfully";
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