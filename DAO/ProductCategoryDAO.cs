using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.DAO
{
    public class ProductCategoryDAO
    {
        DCDGearDbContext db = new DCDGearDbContext();
        public List<ProductCategory> ListByParent()
        {
            return db.ProductCategories.Where(d => d.ParentID == null).ToList();
        }
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.ToList();
        }
        public bool Create (ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return true;
        }
        public ProductCategory FindByID(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public bool Edit(ProductCategory entity)
        {
            var productCategory = db.ProductCategories.Find(entity.ID);
            productCategory.Name = entity.Name;
            productCategory.SeoTitle = entity.SeoTitle;
            productCategory.ParentID = entity.ParentID;
            productCategory.Status = entity.Status;
            productCategory.ModifiedBy = entity.ModifiedBy;
            productCategory.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return true;
        }
    }
}