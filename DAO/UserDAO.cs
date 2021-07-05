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
        public List<User> ListAll()
        {
            return db.Users.OrderBy(d => d.GroupID).ToList();
        }
        //add user
        public bool Create (User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return true;
        }
        public bool Edit (User entity)
        {
            var user = db.Users.Find(entity.ID);
            user.UserName = entity.UserName;
            user.Name = entity.Name;
            if(!string.IsNullOrEmpty(entity.PassWord))
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
            return true;

        }
        public bool Delete(long id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return true;
        }
        //List usergroup l for Viewbag
        public List<UserGroup> ListParent()
        {
            return db.UserGroups.ToList();
        }
        public User GetByUserName(string userName)
        {
            return db.Users.SingleOrDefault(d=>d.UserName==userName);
        }
        public User GetByID(long id)
        {
            return db.Users.Find(id);
        }
        public int Login(string UserName, string PassWord, bool isAdmin = false)
        {
            var result = db.Users.SingleOrDefault(d => d.UserName == UserName);
            if(result==null)
            {
                return 0;
            }
            else
            {
                if(isAdmin==true)
                {
                    if(result.GroupID == "ADMIN")
                    {
                        if(result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.PassWord == PassWord)
                            {
                                return 1;
                            }
                            else
                            {
                                return -2;
                            }
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.PassWord == PassWord)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                }
            }

        }
    }
}