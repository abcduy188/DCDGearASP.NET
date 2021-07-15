using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.ViewModel
{
   
    public class Cart
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        public long ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalItem
        {
            get { return Quantity * Price; }
        }
        public Cart(long productID)
        {
            ID = productID;
            Product product = db.Products.Find(ID);
            Name = product.Name;
            Image = product.Image;
            if (product.PromotionPrice != null)
            {
                Price = product.PromotionPrice;
            }
            else
            {
                Price = product.Price;
            }
            Quantity = 1;
        }
    }
}