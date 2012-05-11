using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDocumentSession _documentSession;
        //
        // GET: /Product/
        public ProductController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public JsonResult Index(long id)
        {
            var product = _documentSession.Load<Product>(id);
            return Json(product,JsonRequestBehavior.AllowGet);
        }

    }
}
