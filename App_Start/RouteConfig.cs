using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DCDGear
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "InfoUser",
               url: "thong-tin-ca-nhan-{id}",
               defaults: new { controller = "Home", action = "Info", id = UrlParameter.Optional },
              namespaces: new[] { "DCDGear.Controllers" }
           );
            routes.MapRoute(
            name: "Login",
            url: "dang-nhap",
            defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
           namespaces: new[] { "DCDGear.Controllers" }
        );
            routes.MapRoute(
            name: "Register",
            url: "dang-ki",
            defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
           namespaces: new[] { "DCDGear.Controllers" }
        );
            routes.MapRoute(
            name: "DetailNew",
            url: "chi-tiet-tin-tuc/{SeoTitle}-{id}",
            defaults: new { controller = "New", action = "DetailNew", id = UrlParameter.Optional },
           namespaces: new[] { "DCDGear.Controllers" }
        );
            routes.MapRoute(
               name: "CategoryNew",
               url: "tin-tuc/{SeoTitle}-{id}",
               defaults: new { controller = "New", action = "Category", id = UrlParameter.Optional },
              namespaces: new[] { "DCDGear.Controllers" }
           );
            routes.MapRoute(
              name: "DetailProduct",
              url: "chi-tiet/{SeoTitle}-{id}",
              defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
             namespaces: new[] { "DCDGear.Controllers" }
          );
            routes.MapRoute(
              name: "Search",
              url: "tim-kiem",
              defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
             namespaces: new[] { "DCDGear.Controllers" }
          );
            routes.MapRoute(
               name: "Category",
               url: "san-pham/{SeoTitle}-{CateID}",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
              namespaces: new[] { "DCDGear.Controllers" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "DCDGear.Controllers" }
            );
        }
    }
}
