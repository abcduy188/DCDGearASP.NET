using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class BannerController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Admin/Banner
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Banner banner)
        {
            try
            {
                if (ModelState.IsValid) // kiem tra co valid form hay khong
                {
                    HttpFileCollectionBase files = Request.Files;
                    var x = "";
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase fileUpload = files[i];
                        if (i == 0 && fileUpload.ContentLength == 0)
                        {
                            SetAlert("Vui lòng thêm ảnh cho banner", "warning");
                            return RedirectToAction("Create", "Banner");
                        }
                        else
                        {
                            var fileName = Path.GetFileName(fileUpload.FileName);
                            x += fileName + ",";
                            var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
                            fileUpload.SaveAs(path);
                        }
                    }
                    banner.Image = x.Remove(x.Length - 1);
                    db.Banners.Add(banner);
                    db.SaveChanges();
                    SetAlert("Thêm banner thành công", "success");
                    return RedirectToAction("Index", "Banner");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("Index");
        }
        #region Imageupload
        //public ActionResult ImageUpload()
        //{
        //    return View();
        //}
        //public ActionResult Create(Banner banner)
        //{
        //    try
        //    {
        //        //ImageUpload objTaskDetails = new ImageUpload();
        //        HttpFileCollectionBase files = Request.Files;
        //        var x = "";
        //        if (!string.IsNullOrEmpty(banner.ToString()))
        //        {

        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                HttpPostedFileBase file = files[i];
        //                byte[] ByteImgArray;
        //                ByteImgArray = ConvertToBytes(file);
        //                var ImageQuality = ConfigurationManager.AppSettings["ImageQuality"];
        //                var reduceIMage = ReduceImageSize(ByteImgArray, ImageQuality);
        //                string fileName = file.FileName;
        //                x += fileName + ",";
        //                string serverMapPath = Server.MapPath("~/Assets/Thumbnail");
        //                string filePath = serverMapPath + "//" + fileName;
        //                SaveFile(reduceIMage, filePath, file.FileName);
        //            }
        //            banner.Image = x.Remove(x.Length - 1);
        //            db.Banners.Add(banner);
        //            db.SaveChanges();
        //            SetAlert("Thêm banner thành công", "success");
        //            return RedirectToAction("Index", "Banner");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return View("Index");
        //}
        // GET: Admin/New/Delete/5
        public ActionResult Delete(long id)
        {   
            Banner banner = db.Banners.Find(id);
            SetAlert("Bạn có chắc muốn xóa?", "warning");
            return View(banner);
        }

        // POST: Admin/New/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            db.SaveChanges();
            SetAlert("Xóa banner thành công", "success");
            return RedirectToAction("Index");
        }
#endregion
        #region ImageResize
        //private byte[] ConvertToBytes(HttpPostedFileBase image)
        //{
        //    byte[] CoverImageBytes = null;
        //    BinaryReader reader = new BinaryReader(image.InputStream);
        //    CoverImageBytes = reader.ReadBytes((int)image.ContentLength);
        //    return CoverImageBytes;
        //}
        //public static byte[] ReduceImageSize(byte[] inputBytes, string ImageQuality)
        //{
        //    Byte[] imageBytes;
        //    int jpegQuality;

        //    //string ImageQuality = "";/*ConfigurationManager.AppSettings["ImageQuality"];*/
        //    if (!string.IsNullOrEmpty(ImageQuality))
        //    {
        //        jpegQuality = Convert.ToInt32(ImageQuality);
        //    }
        //    else
        //    {
        //        jpegQuality = 25;
        //    }

        //    System.Drawing.Image image;

        //    using (var inputStream = new MemoryStream(inputBytes))
        //    {
        //        // Create an Encoder object based on the GUID  for the Quality parameter category.  
        //        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
        //        image = System.Drawing.Image.FromStream(inputStream);
        //        var jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
        //        var encoderParameters = new EncoderParameters(1);
        //        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
        //        encoderParameters.Param[0] = myEncoderParameter;
        //        using (var outputStream = new MemoryStream())
        //        {
        //            image.Save(outputStream, jpegEncoder, encoderParameters);
        //            imageBytes = outputStream.ToArray();
        //        }
        //    }
        //    return imageBytes;
        //}
        //public string SaveFile(byte[] file, string filePath, string filename)
        //{
        //    string directoryPath = Path.GetDirectoryName(filePath);
        //    if (!Directory.Exists(directoryPath))
        //        System.IO.Directory.CreateDirectory(directoryPath);

        //    if (file != null)
        //    {
        //        using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(file)))
        //        {
        //            //var i = Image.FromFile(filePath + file);
        //            //var i2 = new Bitmap(i);
        //            if (filename.ToLower().Contains(".jpg") || filename.ToLower().Contains(".jpeg"))
        //            {
        //                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                //i2.Save(filePath, ImageFormat.Jpeg);
        //            }
        //            else if (filename.ToLower().Contains(".png"))
        //            {
        //                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        //                //i2.Save(filePath, ImageFormat.Png);
        //            }
        //            else if (filename.ToLower().Contains(".bmp"))
        //            {
        //                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
        //                //i2.Save(filePath, ImageFormat.Bmp);
        //            }
        //            else if (filename.ToLower().Contains(".gif"))
        //            {
        //                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif);
        //                //i2.Save(filePath, ImageFormat.Gif);
        //            }
        //        }
        //    }
        //    return string.Empty;
        //}

        #endregion
    }
}