using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace verychefy.Areas.Products.Controllers
{
    public class ProductsHomeController : Controller
    {
        //
        // GET: /Products/Home/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Products/Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

 
    }
}
