using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using verychefy.Models.Products;
using App_a_matic.Helper;

namespace verychefy.Areas.what_goes_with.Controllers {
    public class what_goes_withController : Controller {

        //
        // GET: /what_goes_with/what_goes_with/
        public ActionResult List(){
            var list = (new Ingredients()).GetCollection<Ingredients>();
            return View(list);
        }

        //
        // GET: /what_goes_with/what_goes_with/Details/5
        public ActionResult Details(string item)
        {
            var fltr = new DataExtensions.Parameter("CommonName", item);
            var model = (new Ingredients()).Query<Ingredients>(fltr).FirstOrDefault<Ingredients>();
            return View(model);
        }


    }
}
