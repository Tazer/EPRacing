using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            return View(new PaymentModel(basket, email, order.Id));
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
            basket = _documentSession.Query<Basket>().FirstOrDefault(x => x.PublicId == (Guid)publicId);

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
        public string MD5 { get { return Md5Helper.CalculateMD5Hash(SellerEmail + ":" + Cost + ":" + ExtraCost + ":" + OkUrl + ":" + GuaranteeOffered, Key); } }
        public int GuaranteeOffered { get; set; }

        public PaymentModel(Basket basket, string buyerEmail, long orderId)
        {
            SellerEmail = "contact@epracing.se";
            Description = "Varor från EP-Racing";
            Cost = basket.Total - basket.Shipping;
            ExtraCost = basket.Shipping;
            OkUrl = "http://www.epracing.se/Checkout/Receipt/" + orderId;
            AgentId = 12993;
            Basket = basket;
            BuyerEmail = buyerEmail;
            GuaranteeOffered = 1;
            RefNr = orderId;
            Key = "d65bfc8d-9655-4536-95a6-41b5d2c11f7a";
        }
    }

    /// <summary>
    /// Använd denna klass för att generera er MD5-sträng
    /// Lägg till denna fil i ert projekt.
    /// 
    /// Exempel:
    ///     
    ///     Md5Helper md5h = new Md5Helper(); // skapar en instans av hjälpklassen
    ///     String md5 = md5h.CalculateMD5Hash("Md5-Strängen","Md5-Nyckeln"); //skapar den hashade Md5strängen
    ///     
    /// 
    /// </summary>
    public class Md5Helper
    {


        public static string CalculateMD5Hash(string MD5String, string key)
        {
            string md5Hash;
            string completeMD5String = MD5String + key;

            ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            Byte[] result = md5Provider.ComputeHash(encoding.GetBytes(completeMD5String));
            md5Hash = ToHexString(result);

            return md5Hash;
        }

        private static string ToHexString(Byte[] input)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                builder.Append(input[i].ToString("x2"));
            }

            return builder.ToString();
        }

    }
}
