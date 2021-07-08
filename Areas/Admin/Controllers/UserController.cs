using DCDGear.Common;
using DCDGear.DAO;
using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Admin/User
        public ActionResult Index()
        {
            var user = db.Users.OrderBy(d => d.Name).ToList();
            return View(user);
        }
        public ActionResult Create()
        {
            ViewBag.GroupID = new SelectList(db.UserGroups.ToList(), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            ViewBag.GroupID = new SelectList(db.UserGroups.ToList(), "ID", "Name");
            var session = (UserLogin)Session["DUY"];
            user.CreateBy = session.Name;
            user.CreateDate = DateTime.Now;
            user.PassWord = Encryptor.MD5Hash(user.PassWord);
            if (ModelState.IsValid)
            {
                if (db.Users.Count(d => d.UserName == user.UserName) > 0)
                {
                    SetAlert("Username đã có người sử dụng", "error");
                    return RedirectToAction("Create", "User");
                }
                else
                {
                    if (db.Users.Count(d => d.Email == user.Email) > 0)
                    {
                        SetAlert("Email đã có người sử dụng", "error");
                        return RedirectToAction("Create", "User");
                    }
                    else
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        SetAlert("Thêm tài khoản thành công", "success");
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {

            var user = db.Users.Find(id);
            ViewBag.GroupID = new SelectList(db.UserGroups.ToList(), "ID", "Name", user.GroupID);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User entity)
        {
            ViewBag.GroupID = new SelectList(db.UserGroups.ToList(), "ID", "Name", entity.GroupID);
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(entity.ID);
                var session = (UserLogin)Session["DUY"];
                entity.ModifiedBy = session.Name;
                
                if (db.Users.Count(d => d.UserName == entity.UserName) > 0 && user.UserName != entity.UserName)
                {

                    SetAlert("Username đã có người sử dụng", "error");
                    return RedirectToAction("Edit", "User");
                }
                else 
                {
                    if (db.Users.Count(d => d.Email == entity.Email) > 0 && user.Email != entity.Email)
                    {
                        SetAlert("Email đã có người sử dụng", "error");
                        return RedirectToAction("Edit", "User");
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
                        SetAlert("Sửa tài khoản thành công", "success");
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return Redirect("Index");
        }
    }
}