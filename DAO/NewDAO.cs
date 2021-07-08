using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.DAO
{
    public class NewDAO
    {
        DCDGearDbContext db = new DCDGearDbContext();
        //admin 
        public List<New> ListAll()
        {
            return db.News.OrderBy(d => d.Name).ToList();
        }
        public long Create(New entity)
        {
            db.News.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public New GetByID(long id)
        {
            return db.News.Find(id);
        }
        public int Update(New entity)
        {
            var news = db.News.Find(entity.ID);
            news.Name = entity.Name;
            news.CategoryID = entity.CategoryID;
            news.SeoTitle = entity.SeoTitle;
            news.Description = entity.Description;
            try
            {
                if (entity.Image == null)
                {
                    if (!string.IsNullOrEmpty(news.Image))
                    {
                        news.Image = news.Image;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    news.Image = entity.Image;
                }
            }
            catch (Exception e)
            {
            }
           
            news.Detail = entity.Detail;
            news.Status = entity.Status;
            news.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return 1;
        }
    }
}