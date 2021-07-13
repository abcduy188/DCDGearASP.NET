using DCDGear.Common;
using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = db.Users.SingleOrDefault(d => d.UserName == model.UserName);
                if (result == null)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else
                {
                    if (result.Status == false)
                    {
                        ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                    }
                    else
                    {
                        if (result.PassWord == Encryptor.MD5Hash(model.PassWord))
                        {
                            var userSession = new UserLogin();
                            userSession.UserName = result.UserName;
                            userSession.UserID = result.ID;
                            userSession.Name = result.Name;
                            Session.Add("DUY", userSession);
                            return Redirect("/");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Mật khẩu sai!");
                        }
                    }
                }
            }
            return View("Login");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            var list = db.Users.ToList();
            if (ModelState.IsValid)
            {
                if (list.Where(d => d.UserName == model.UserName).Count() > 0)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã có người sử dụng");
                }
                else if (list.Where(d => d.Email == model.Email).Count() > 0)
                {
                    ModelState.AddModelError("", "Email này đã có người sử dụng");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.PassWord = Encryptor.MD5Hash(model.PassWord);
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.Status = true;
                    user.GroupID = "MEMBER";
                    user.CreateDate = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Success = "Đăng kí thành côgng";
                    return Redirect("/dang-nhap");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("DUY");
            return Redirect("/");
        }
    }
}