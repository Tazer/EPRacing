﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPRacing.Model
{
    public class Order : BaseEntity
    {
        public Order(string email,Basket basket)
        {
            Email = email;
            Basket = basket;
            Payed = false;
            Created = DateTime.Now;
        }

        public string Email { get; set; }
        public Basket Basket { get; set; }
        public bool Payed { get; protected set; }
        public DateTime Created { get; set; }
        public string TransactionId { get; set; }
        public void Completed()
        {
            Payed = true;
        }
    }

    public class Basket : BaseEntity
    {

        public Basket(Guid publicId)
        {
            PublicId = publicId;
            Created = DateTime.Now;
            Products = new List<Product>();
        }
        public Guid PublicId { get; set; }
        public IList<Product> Products { get; set; }
        public DateTime Created { get; set; }
        public decimal Shipping { get { return int.Parse(ConfigurationManager.AppSettings["Shipping"]); } }
        public decimal Total { get { return Products.Any() ? Products.Sum(x => x.Price) + Shipping : 0; } }
    }

    public class Product : BaseEntity
    {
        public Product()
        {
            Added = DateTime.Now;
        }
        public Product(string name,string description,string longdescription,decimal price,int stockamount,string image)
        {
            Name = name;
            Description = description;
            LongDescription = longdescription;
            Price = price;
            StockAmount = stockamount;
            Image = image;
            Added = DateTime.Now;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public int StockAmount { get; set; }
        public DateTime Added { get; set; }
        public string Image { get; set; }

        public void Update(Product product)
        {
            Name = product.Name;
            Description = product.Description;
            LongDescription = product.LongDescription;
            Price = product.Price;
            StockAmount = product.StockAmount;
            Image = product.Image;
        }
    }

    public class BaseEntity
    {
        public long Id { get; set; }
    }
}
