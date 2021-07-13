using DCDGear.Common;
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
        public int Update(Product entity)
        {
            var products = db.Products.Find(entity.ID);
            products.Name = entity.Name;
            products.CategoryID = entity.CategoryID;
            products.SeoTitle = entity.SeoTitle;
            products.Description = entity.Description;
            try
            {
                if (entity.Image == null)
                {
                    if (!string.IsNullOrEmpty(products.Image))
                    {
                        products.Image = products.Image;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    products.Image = entity.Image;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
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
            return 1;
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
            catch
            {
                return false;
            }

        }
    }
}