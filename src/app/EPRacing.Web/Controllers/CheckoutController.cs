using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EPRacing.Model;
using Raven.Client;

namespace EPRacing.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IDocumentSession _documentSession;

        public CheckoutController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        //
        // GET: /Checkout/

        public ActionResult Pay(string email)
        {
            var basket = GetBasket();
            var order = new Order(email, basket);
            _documentSession.Store(order);
            return View(new PaymentModel(basket,email,order.Id));
        }

        
        public ActionResult Receipt()
        {
            return View();
        }
        public Basket GetBasket()
        {
            var publicId = Session["PublicId"];

            Basket basket = null;
            if (publicId == null)
            {
                throw new KeyNotFoundException("Not basket found");
            }

                //Try to get from cookie here also.
            basket = _documentSession.Query<Basket>().FirstOrDefault(x => x.PublicId == (Guid) publicId);

            return basket;
        }

    }
    public class PaymentModel
    {
        public Basket Basket { get; set; }
        public string SellerEmail { get; set; }
        public string BuyerEmail { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal ExtraCost { get; set; }
        public string OkUrl { get; set; }
        public int AgentId { get; set; }
        public string Key { get; set; }
        public long RefNr { get; set; }
        public string MD5 { get { return FormsAuthentication.HashPasswordForStoringInConfigFile(SellerEmail + ":" + Cost + ":" + ExtraCost + ":" + OkUrl + ":"+ GuaranteeOffered + Key, "MD5"); } }
        public int GuaranteeOffered { get; set; }

        public PaymentModel(Basket basket,string buyerEmail,long orderId)
        {
            SellerEmail = "shop@epracing.se";
            Description = "Varor från EP-Racing";
            Cost = basket.Total - basket.Shipping;
            ExtraCost = basket.Shipping;
            OkUrl = "/Checkout/Receipt/"+ orderId;
            AgentId = 123;
            Basket = basket;
            BuyerEmail = buyerEmail;
            GuaranteeOffered = 1;
            RefNr = orderId;
        }
    }
}
