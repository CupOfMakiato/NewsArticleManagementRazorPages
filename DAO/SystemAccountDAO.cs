using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public partial class SystemAccountDAO
    {
        private static SystemAccountDAO instance = null;
        public SystemAccountDAO()
        {

        }
        public static SystemAccountDAO getInstance()
        {
            if (instance == null)
            {
                instance = new SystemAccountDAO();
            }
            return instance;
        }
        public bool IsAdmin(SystemAccount acc)
        {
            try
            {
                var config = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json").Build();

                var defaultAdminAccount = config.GetSection("AdminCredentials");

                if (acc.AccountEmail == defaultAdminAccount["Email"] && acc.AccountPassword == defaultAdminAccount["Password"])
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public SystemAccount VerifyAccount(SystemAccount acc)
        {
            SystemAccount mb;
            try
            {
                var db = new FunewsManagementFall2024Context();
                mb = db.SystemAccounts.FirstOrDefault(m => m.AccountEmail == acc.AccountEmail && m.AccountPassword == acc.AccountPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mb;
        }
        public List<SystemAccount> GetAllAccount()
        {
            var listCatagories = new List<SystemAccount>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                listCatagories = context.SystemAccounts.ToList();
            }
            catch (Exception ex)
            {
                return listCatagories;
            }
            return listCatagories;
        }

        public SystemAccount GetAccountById(short accountId)
        {
            using var db = new FunewsManagementFall2024Context();
            return db.SystemAccounts.FirstOrDefault(x => x.AccountId.Equals(accountId));
        }

        public SystemAccount? GetAccountByEmailAndPassword(string email, string password)
        {
            using var db = new FunewsManagementFall2024Context();
            return db.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(email) && x.AccountPassword.Equals(password));
        }

        public void UpdateAccount(SystemAccount acc)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                db.Entry<SystemAccount>(acc).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteAccount(SystemAccount acc)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                var p1 = db.SystemAccounts.SingleOrDefault(x => x.AccountId == acc.AccountId);
                db.SystemAccounts.Remove(p1);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddAccount(SystemAccount acc)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                db.SystemAccounts.Add(acc);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
