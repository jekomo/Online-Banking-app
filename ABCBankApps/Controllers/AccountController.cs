using ABCBankApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace ABCBankApps.Controllers
{
    public class AccountController : Controller
    {
        ABCBank dbContext = new ABCBank();
        // GET: /Account/

        [HttpGet]
        public ActionResult Login(String returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult login(AccountUser newAccountUser,string returnUrl)
        {
            ViewBag.ValidResult = "Null";
            if (ModelState.IsValid)
            {
         bool isLoginOk=  isSucess(newAccountUser.nEmail,newAccountUser.nPassword);

              if(isLoginOk)
              {
                  FormsAuthentication.SetAuthCookie(newAccountUser.nEmail, true);
                  return RedirectToLocal(returnUrl);

              }
              else
              {
                  ModelState.AddModelError("", "Invalid email and password");
              }

            }
            return View(); 
           
        }
        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");

        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(AccountUser newAccountUser)
        {
            ViewBag.ValidResult = "Null";
            if (ModelState.IsValid)
            {
                var RegUsersFind = dbContext.AccountUsers.Find(newAccountUser.nEmail);
                if (RegUsersFind != null)
                {
                    ViewBag.ValidResult = "EmailTaken";
                    ModelState.AddModelError("", "The email is already taken");
                }
                else
                {
                   
                    var RegUsers = dbContext.AccountUsers.Create();
                    var simpleCrypto = new SimpleCrypto.PBKDF2();
                    var encryptpass = simpleCrypto.Compute(newAccountUser.nPassword);
                    RegUsers.nEmail = newAccountUser.nEmail;
                    RegUsers.nPassword = encryptpass;
                    RegUsers.nPasswordSalt = simpleCrypto.Salt;
                    RegUsers.cAuLd = Convert.ToString(Guid.NewGuid());
                    dbContext.AccountUsers.Add(RegUsers);
                    dbContext.SaveChanges();
                    ViewBag.ValidResult = "Success";
                    ModelState.Clear();
                    ModelState.AddModelError("", "Registration Sucessfull !");
                }


            }

            return View();

        }
        private bool isSucess(string newEmail, string newPassword)
        {
            var simpleCrypto = new SimpleCrypto.PBKDF2();
            bool isLoginOk = false;
            //var UserEmail = dbContext.AccountUsers.FirstOrDefault(u => u.nEmail == newEmail);
            var UserEmail = dbContext.AccountUsers.FirstOrDefault(user => user.nEmail.Equals(newEmail));
            if (UserEmail != null)
            {

                if (UserEmail.nPassword == simpleCrypto.Compute(newPassword, UserEmail.nPasswordSalt))
                {
                    isLoginOk = true;
                }

            }
            return isLoginOk;

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }


        }
        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
