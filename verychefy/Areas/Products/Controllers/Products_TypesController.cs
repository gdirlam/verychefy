using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace verychefy.Areas.Products.Controllers
{
    public class TypesController : Controller
    {
        //
        // GET: /Products/ProductsTypes/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult viewmodel() {
            Response.ContentType = "text/javascript";
            return View();
        }

        //
        // GET: /Products/ProductsTypes/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

   
    }
}
