using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.DAO
{
    public class ProductDAO
    {
        DCDGearDbContext db = new DCDGearDbContext();
        //admin 
        public List<Product> ListAll()
        {
            return db.Products.OrderBy(d => d.Name).ToList();
        }
        public long Create(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Product GetByID(long id)
        {
            return db.Products.Find(id);
        }
        public bool Update(Product entity)
        {
            var products = db.Products.Find(entity.ID);
            products.Name = entity.Name;
            products.CategoryID = entity.CategoryID;
            products.MetaTitle = entity.MetaTitle;
            products.Description = entity.Description;
            products.Image = entity.Image;
            products.MoreImages = entity.MoreImages;
            products.Price = entity.Price;
            products.PromotionPrice = entity.PromotionPrice;
            products.LinkVideo = entity.LinkVideo;
            products.Detail = entity.Detail;
            products.Quantity = entity.Quantity;
            products.CPU = entity.CPU;
            products.OperatingSystem = entity.OperatingSystem;
            products.RAM = entity.RAM;
            products.GPU = entity.GPU;
            products.Screen = entity.Screen;
            products.SSDHardDrive = entity.SSDHardDrive;
            products.ConnectionPorts = entity.ConnectionPorts;
            products.Keyboard = entity.Keyboard;
            products.Pin = entity.Pin;
            products.Size = entity.Size;
            products.Weight = entity.Weight;
            products.Status = entity.Status;
            products.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return true;
        }
        public bool Delete(long id)
        {
            try
            {
                var products = db.Products.Find(id);
                db.Products.Remove(products);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}