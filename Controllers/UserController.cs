using DCDGear.Common;
using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Random = System.Random;

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
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ConfirmRegister()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("DUY");
            return Redirect("/");
        }
        public ActionResult ForgotPassWord()
        {
            return View();
        }
        public ActionResult EnterCode()
        {
            return View();
        }
        public ActionResult ChangePass()
        {
            var sess = Session["Confirm"] as UserLogin;
            if (sess == null)
            {
                return Redirect("/dang-nhap");
            }
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
                    if (result.PassWord == Encryptor.MD5Hash(model.PassWord))
                    {
                        var userSession = new UserLogin();
                        userSession.UserName = result.UserName;
                        userSession.UserID = result.ID;
                        userSession.Name = result.Name;
                        Session.Add("DUY", userSession);
                        if (result.Status == true)
                        {                  
                            return Redirect("/");
                        }
                        else
                        {
                            Session.Add("Confirm", userSession);
                            return RedirectToAction("ConfirmRegister", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu sai!");
                    }
                }
            }
            return View("Login");
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
                    var user = Create(model);

                    ViewBag.SendMail = "Vui lòng nhập mã xác nhận trong mail";
                    Mail(model.Name, user.Code, model.Email);
                    return RedirectToAction("ConfirmRegister", "User");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmRegister(string Code, string UserName)
        {
            if (ModelState.IsValid)
            {
                var sess = Session["Confirm"] as UserLogin; // = (UserLogin)Session["Confirm"]
                var user = new User();
                if (sess != null)
                {
                    user = db.Users.Single(d => d.UserName == sess.UserName);
                }
                else
                {
                    user = db.Users.Single(d => d.UserName == UserName);
                }
                if (Code == user.Code)
                {
                    user.Status = true;
                    user.Code = Encryptor.MD5Hash(user.Code);
                    db.SaveChanges();
                    ViewBag.Success = "Kích hoạt thành công";
                    return Redirect("/dang-nhap");

                }
                else
                {
                    ModelState.AddModelError("", "Kích hoạt khoong thành công");
                }
            }

            return RedirectToAction("ConfirmRegister", "User");
        }

        [HttpPost]
        public ActionResult ForgotPassWord(RegisterModel model)
        {
            var user = new User();
            user = db.Users.SingleOrDefault(d => d.Email == model.Email);
            if (user != null)
            {
                var userSession = new UserLogin();
                userSession.UserName = user.UserName;
                Session.Add("Confirm", userSession);
                string code = Random();
                Mail(user.Name, code, user.Email);
                user.Code = code;
                db.SaveChanges();
                return RedirectToAction("EnterCode", "User");
            }
            else
            {
                ModelState.AddModelError("", "Email không trùng khớp");
            }
            return View();
        }

        [HttpPost]
        public ActionResult EnterCode(string Code, string UserName)
        {
            var sess = Session["Confirm"] as UserLogin; // = (UserLogin)Session["Confirm"]
            var user = new User();
            if (sess != null)
            {
                user = db.Users.Single(d => d.UserName == sess.UserName);
                if (Code == user.Code)
                {
                    return RedirectToAction("ChangePass", "User");
                }
                else
                {
                    ViewBag.Success = "Kích hoạt khoong thành công";
                }
            }
            return RedirectToAction("EnterCode", "User");
        }

        [HttpPost]
        public ActionResult ChangePass(RegisterModel model)
        {
            var user = new User();
            var sess = Session["Confirm"] as UserLogin; // = (UserLogin)Session["Confirm"]
            if (sess != null)
            {
                user = db.Users.Single(d => d.UserName == sess.UserName);
                user.PassWord = Encryptor.MD5Hash(model.PassWord);
                user.Code = null;
                db.SaveChanges();
                return Redirect("/dang-nhap");
            }
            return View();
        }
        public ActionResult ResendMail(string strURL)
        { 
            var user = new User();
            var sess = Session["Confirm"] as UserLogin; // = (UserLogin)Session["Confirm"]
            if (sess != null)
            {
                user = db.Users.Single(d => d.UserName == sess.UserName);
                string code = Random();
                Mail(sess.Name, code, user.Email);
                user.Code = code;
                db.SaveChanges();
                return Redirect(strURL);
            }
            return View();
        }
        public string Random()
        {
            Random rand = new Random();
            var numIterations = rand.Next(0, 999999);
            string x = numIterations.ToString("000000");
            return x;
        }
        public User Create(RegisterModel model)
        {
            var user = new User();
            user.UserName = model.UserName;
            user.PassWord = Encryptor.MD5Hash(model.PassWord);
            user.Name = model.Name;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Status = false;
            user.GroupID = "MEMBER";
            user.CreateDate = DateTime.Now;
            user.Code = Random();
            db.Users.Add(user);
            db.SaveChanges();
            var userSession = new UserLogin();
            userSession.UserName = user.UserName;
            Session.Add("Confirm", userSession);
            return user;
        }
        public void Mail(string toEmail, string Content, string AddressEmail)
        {

            //gửi mail cho khách hàng 
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Mail/ConfirmMail.html"));

            content = content.Replace("{{CustomerName}}", toEmail);
            content = content.Replace("{{Code}}", Content);

            new Mail().SendMail(AddressEmail, "Confirm Email form DCDGear", content);
        }

    }
}