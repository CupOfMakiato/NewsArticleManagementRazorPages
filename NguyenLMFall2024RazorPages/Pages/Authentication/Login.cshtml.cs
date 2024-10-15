using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Text.Json;

namespace NguyenLMFall2024RazorPages.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        [BindProperty]
        public SystemAccount LoginAccount { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public LoginModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }



        public IActionResult OnPostLogin()
        {
            var adminInfo = GetAdminInfo();

            SystemAccount? systemAccount = _systemAccountService.GetAccountByEmailAndPassword(LoginAccount.AccountEmail, LoginAccount.AccountPassword);

            string jsonLoginAccount;

            if (adminInfo.adminEmail.Equals(LoginAccount.AccountEmail) && adminInfo.adminPassword.Equals(LoginAccount.AccountPassword))
            {
                LoginAccount.AccountRole = -1;
                jsonLoginAccount = JsonSerializer.Serialize(LoginAccount, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true
                });
                HttpContext.Session.SetString("LoginSession", jsonLoginAccount);
                return RedirectToPage("/UserManagement/Index");
            }
            if (systemAccount is not null)
            {
                jsonLoginAccount = JsonSerializer.Serialize(systemAccount, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true
                });
                if (systemAccount.AccountRole == 1)
                {
                    HttpContext.Session.SetString("LoginSession", jsonLoginAccount);
                    return RedirectToPage("/NewsArticleManagement/Index");
                }
                else
                {
                    ErrorMessage = "You do not have permission";
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    return Page();
                }
            }
            ErrorMessage = "Email or Password is incorrect";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }

        private (string adminEmail, string adminPassword) GetAdminInfo()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            string adminEmail = configuration["AdminCredentials:Email"];
            string adminPassword = configuration["AdminCredentials:Password"];
            return (adminEmail, adminPassword);
        }
    }
}
