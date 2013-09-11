using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace verychefy.Areas.what_goes_with.Controllers
{
    public class IngredientController : Controller
    {
        //
        // GET: /what_goes_with/Ingredient/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /what_goes_with/Ingredient/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /what_goes_with/Ingredient/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /what_goes_with/Ingredient/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /what_goes_with/Ingredient/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /what_goes_with/Ingredient/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /what_goes_with/Ingredient/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /what_goes_with/Ingredient/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
