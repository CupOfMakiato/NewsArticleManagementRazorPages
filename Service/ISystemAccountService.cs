using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISystemAccountService
    {
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(SystemAccount account);
        SystemAccount GetAccountById(short accountID);
        SystemAccount? GetAccountByEmailAndPassword(string email, string password);
        List<SystemAccount> GetAllAccount();
        bool IsAdmin(SystemAccount account);
        SystemAccount VerifyAccount(SystemAccount account);
    }
}
