using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _documentSession;

        public HomeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var products = _documentSession.Query<Product>().Where(x => x.StockAmount > 0).ToList();

            return View(products);
        }

    }
}
