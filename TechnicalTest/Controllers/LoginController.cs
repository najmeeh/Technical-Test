using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TechnicalTest.Models;


namespace TechnicalTest.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user, string ReturnUrl)
        {
            if (IsValid(user))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return Redirect(ReturnUrl); }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(user);
            }
        }
        private bool IsValid(LoginViewModel user)
        {
            return (((user.Email == "admin@yahoo.com") && (user.Password == "123")) ||
                ((user.Email == "user@yahoo.com") && (user.Password == "123")));
        }

        
    }
}