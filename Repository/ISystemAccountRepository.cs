using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISystemAccountRepository
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
