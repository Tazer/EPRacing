﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPRacing.Model
{
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
        public decimal Total { get { return Products.Any() ? Products.Sum(x => x.Price) : 0; } }
    }

    public class Product : BaseEntity
    {
        public Product(string name,string description,decimal price,int stockamount,string image)
        {
            Name = name;
            Description = description;
            Price = price;
            StockAmount = stockamount;
            Image = image;
            Added = DateTime.Now;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockAmount { get; set; }
        public DateTime Added { get; set; }
        public string Image { get; set; }

    }

    public class BaseEntity
    {
        public long Id { get; set; }
    }
}
