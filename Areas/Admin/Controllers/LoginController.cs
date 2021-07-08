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
    public class LoginController : Controller
    {
        // GET: Admin/Login
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("DUY");
            return RedirectToAction("Index", "Login");
        }
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
                    if (result.GroupID != "ADMIN")
                    {
                        ModelState.AddModelError("", "Bạn không có quyền đăng nhập");
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
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Mật khẩu sai!");
                            }
                        }
                    }

                }
            }
            return View("Index");
        }
    }
}