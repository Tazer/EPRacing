using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EPRacing.Model;
using Raven.Client;
using Raven.Client.Linq;

namespace EPRacing.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDocumentSession documentSession;

        public LoginController(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View(new User());
        }
        [HttpPost]
        public ActionResult Index(string Username,string Password)
        {
            var user = documentSession.Query<User>().FirstOrDefault(x => x.Username == Username && x.Password == Password); 
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email,true);
               return RedirectToAction("Index","Home");
            }
            return View(new User());
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}
