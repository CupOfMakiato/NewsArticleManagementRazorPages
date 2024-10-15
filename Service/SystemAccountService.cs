using BusinessObject.Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public SystemAccountService(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public List<SystemAccount> GetAllAccount()
        {
            return _systemAccountRepository.GetAllAccount();
        }
        public SystemAccount GetAccountById(short accountID)
        {
            return _systemAccountRepository.GetAccountById(accountID);
        }
        public void UpdateAccount(SystemAccount account)
        {
            _systemAccountRepository.UpdateAccount(account);
        }
        public void DeleteAccount(SystemAccount account)
        {
            _systemAccountRepository.DeleteAccount(account);
        }
        public void AddAccount(SystemAccount account)
        {
            _systemAccountRepository.AddAccount(account);
        }
        public SystemAccount? GetAccountByEmailAndPassword(string email, string password)
        {
            return _systemAccountRepository.GetAccountByEmailAndPassword(email, password);
        }

        public bool IsAdmin(SystemAccount account)
        {
            return _systemAccountRepository.IsAdmin(account);
        }

        public SystemAccount VerifyAccount(SystemAccount account)
        {
            return _systemAccountRepository.VerifyAccount(account);
        }
    }
}
