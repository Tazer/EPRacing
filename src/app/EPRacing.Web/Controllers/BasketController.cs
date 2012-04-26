using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly IDocumentSession _documentSession;
        //
        // GET: /Basket/
        public BasketController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }


        public JsonResult Get()
        {
            var basket = GetBasket();

            return Json(new { Basket = basket }, JsonRequestBehavior.AllowGet);
        }

        private Basket GetBasket()
        {
            var publicId = Session["PublicId"];

            Basket basket = null;
            if (publicId == null)
            {
                basket = CreateBasket();
            }
            else
            {
                //Try to get from cookie here also.
                basket = _documentSession.Query<Basket>().FirstOrDefault(x => x.PublicId == (Guid) publicId) ??
                         CreateBasket();
            }
            return basket;
        }

        private Basket CreateBasket()
        {
            var publicId = Guid.NewGuid();
            var basket = new Basket(publicId);
            Session["PublicId"] = publicId;
            _documentSession.Store(basket);
            return basket;
        }

        public JsonResult Add(long id)
        {
            var product = _documentSession.Load<Product>(id);
            if (product == null)
                throw new KeyNotFoundException("Didnt find any product with that id");



            var basket = GetBasket();
            basket.Products.Add(product);
            _documentSession.SaveChanges();

            return Json(new
                            {
                                Basket = basket,
                                Product = product,
                            });
        }

    }
}
