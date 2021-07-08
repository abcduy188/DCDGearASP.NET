using DCDGear.Common;
using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.DAO
{
   
    public class UserDAO
    {
        DCDGearDbContext db = new DCDGearDbContext();
        //add user
        public int Create (User entity)
        {
            
            if(db.Users.Count(d=>d.UserName==entity.UserName)>0)
            {
                return 0;
            }
            else
            {
                if (db.Users.Count(d => d.Email == entity.Email) > 0)
                {
                    return -1;
                }
                else
                {
                    db.Users.Add(entity);
                    db.SaveChanges();
                    return 1;
                }
            }
            
           
        }
        public int Edit (User entity)
        {
            var user = db.Users.Find(entity.ID);
            if (db.Users.Count(d => d.UserName == entity.UserName) > 0)
            {
                return 0;
            }
            else
            {
                if (db.Users.Count(d => d.Email == entity.Email) > 0)
                {
                    return -1;
                }
                else
                {
                    user.UserName = entity.UserName;
                    user.Name = entity.Name;
                    if (!string.IsNullOrEmpty(entity.PassWord))
                    {
                        user.PassWord = Encryptor.MD5Hash(entity.PassWord);
                    }
                    else
                    {
                        user.PassWord = user.PassWord;
                    }
                    user.GroupID = entity.GroupID;
                    user.Email = entity.Email;
                    user.Phone = entity.Phone;
                    user.Address = entity.Address;
                    user.Status = entity.Status;
                    user.ModifiedBy = entity.ModifiedBy;
                    user.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return 1;
                }
            }
           
        

        }
        
    }
}