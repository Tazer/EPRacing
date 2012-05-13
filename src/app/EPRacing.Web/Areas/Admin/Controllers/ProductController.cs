using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IDocumentSession documentSession;

        public ProductController(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        //
        // GET: /Admin/Product/

        public ActionResult Index()
        {
            var products = documentSession.Query<Product>().ToList();
            return View(products);
        }

        //
        // GET: /Admin/Product/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Product/Create

        public ActionResult Create()
        {
            return View(new Product());
        }

        //
        // POST: /Admin/Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                documentSession.Store(product);
                documentSession.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Product/Edit/5

        public ActionResult Edit(int id)
        {
            var product = documentSession.Load<Product>(id);
            return View(product);
        }

        //
        // POST: /Admin/Product/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                // TODO: Add update logic here
                var ravenProduct = documentSession.Load<Product>(id);
                ravenProduct.Update(product);
                documentSession.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Product/Delete/5

        public ActionResult Delete(int id)
        {
            var product = documentSession.Load<Product>(id);
            documentSession.Delete(product);
            documentSession.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Admin/Product/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
