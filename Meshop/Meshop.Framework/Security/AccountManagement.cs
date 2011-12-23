using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Meshop.Framework.Model;

namespace Meshop.Framework.Security
{
    public class AccountManagement
    {
        private DatabaseConnection2 _db = new DatabaseConnection2();

        private MachineKeySection machineKey;
        private AuthenticationSection formsConfig;

        public AccountManagement()
        {
            // Get encryption and decryption key information from the configuration.
            Configuration cfg =
                WebConfigurationManager.OpenWebConfiguration(
                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection) cfg.GetSection("system.web/machineKey");
            formsConfig = (AuthenticationSection) cfg.GetSection("system.web/authentication");
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length/2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
            return returnBytes;
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;


            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte(machineKey.ValidationKey);
            encodedPassword =
                Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));


            return encodedPassword;
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            pass1 = EncodePassword(password);


            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        public bool ValidateUser(string userName, string password)
        {
            bool isValid = false;
            bool isEnabled = false;
            string pwd = "";

            var customer = _db.Customers.Find(userName.ToLower());

            if (customer != null)
            {
                pwd = customer.Password;
                isEnabled = customer.Enabled;
            }
            else
            {
                return false;
            }

            if (CheckPassword(password, pwd))
            {
                if (isEnabled)
                {
                    isValid = true;
                }
            }

            return isValid;
        }


        public Customer CreateUser(Customer customer, out MembershipCreateStatus status)
        {
            if (ValidatePassword(customer.Password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (GetUsernameByEmail(customer.Email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            Customer u = GetUser(customer.Username);

            if (u == null)
            {
                customer.Password = EncodePassword(customer.Password);
                customer.Username = customer.Username.ToLower();
                customer.Email = customer.Email.ToLower();
                _db.Customers.Add(customer);
                _db.SaveChanges();

                status = MembershipCreateStatus.Success;
                return GetUser(customer.Username);
            }

            status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        public Customer GetUser(string username)
        {
            return _db.Customers.Find(username.ToLower());
        }

        private string GetUsernameByEmail(string email)
        {
            var user = _db.Customers.Where(c => c.Email.Equals(email.ToLower())).FirstOrDefault();

            return user == null ? "" : user.Username;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length <= PasswordMinLength)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ChangePasswordForUser(string userName, string oldPassword, string newPassword)
        {
            if (!ValidateUser(userName, oldPassword))
                return false;


            if (ValidatePassword(newPassword))
            {
                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }

            int rowsAffected = 0;

            var customer = new Customer {Username = userName, Password = EncodePassword(newPassword)};

            _db.Entry(customer).State = EntityState.Modified;
            rowsAffected = _db.SaveChanges();

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public void EnableUser(string username)
        {
            throw new NotImplementedException();
        }

        public void DisableUser(string username)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Customer customer)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(Customer customer)
        {
            throw new NotImplementedException();
        }

        public static int PasswordMinLength
        {
            get { return 6; }
        }

        public bool IsInRole(string username, string role)
        {
            var dbrole = _db.Customers.Where(c => c.Username == username).Select(c => c.Role).SingleOrDefault();

            if (dbrole == null)
                return false;

            if (dbrole.ToLower() == role.ToLower()) 
                return true;

            return false;
        }

        public void Authorize(string userName, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
            

            //unsecure
          /*  var cookie = new HttpCookie("UserRole") {Value = GetUser(userName).Role, Expires = DateTime.Now.Add(formsConfig.Forms.Timeout),HttpOnly = true};
            HttpContext.Current.Response.Cookies.Add(cookie);
                */
            

        }
        public void SignOff()
        {
            FormsAuthentication.SignOut();
            //Response.Cookies["UserRole"].Expires = DateTime.Now.AddDays(-1);
        }
    }
}