using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class ProductController : Controller
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Product
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            List<ProductCategory> model = db.ProductCategories.ToList();
            return PartialView(model);

        }
        [ChildActionOnly]
        public PartialViewResult CategoryList()
        {
            List<ProductCategory> model = db.ProductCategories.ToList();
            return PartialView(model);

        }

        //public ActionResult Category(long CateID)
        //{
        //    var product = db.Products.Where(d => d.CategoryID == CateID).ToList();

        //    return View(product);
        //}
        public ActionResult Category(long CateID)
        {
            
            var product = db.Products.ToList().Where(d => d.CategoryID == CateID); //lấy ra danh sách sản phẩm theo danh mục
            ProductCategory cate = db.ProductCategories.Find(CateID); //lấy ra danh mục có id = cateID
            var child = db.ProductCategories.Where(d => d.ParentID == cate.ID).ToList(); // lấy ra danh sách danh mục con của cate
            if (child.Count() != 0)
            {
                foreach (var item in child)
                {
                    var list = db.Products.Where(d => d.CategoryID == item.ID).ToList();
                    product = product.Concat(list);
                }
            }
            return View(product);
        }
        public ActionResult Detail(long id)
        {
            Product product = db.Products.Find(id);
            ViewBag.ListRele = db.Products.Where(d => d.CategoryID == product.CategoryID || d.ProductCategory.ParentID == product.ProductCategory.ParentID).ToList();
            return View(product);
        }
    }
}