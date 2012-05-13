using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        private readonly IDocumentSession documentSession;

        public SetupController(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        //
        // GET: /Admin/Setup/

        public ActionResult CreateAdmin(int id)
        {
            if(id == 1337)
            {
                var admin1 = new User("patrik","orka","patrik@epracing.se");
                var admin2 = new User("erik","Adolfsson1","patrik@epracing.se");

                documentSession.Store(admin1);
                documentSession.Store(admin2);
                documentSession.SaveChanges();
                return Content("Created");
            }
            return Content("Failed");
        }

    }
}
