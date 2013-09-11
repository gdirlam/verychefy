using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App_a_matic.Helper;
using verychefy.Models.Products;


namespace verychefy.Areas.Products.Controllers
{
    public class IngredientsController : Controller    {

        #region "Index/List"
        /// <summary>
        ///GET: /Ingredients/ 
        /// </summary>
        /// <verb>GET</verb>
        /// <returns></returns>
        public ActionResult Index() {
            List<Ingredients> ingredients = (new Ingredients()).GetInParallel<Ingredients>();
            return View("Index", ingredients);
        }
        #endregion

        #region "Create"
        /// <summary>
        ///GET: /Ingredients/Create 
        /// </summary>
        /// <verb>GET</verb>
        /// <returns></returns>
        public ActionResult Create() {
            ViewBag.Action = "Create";
            return View();
        }

        /// <summary>
        ///  POST: /Ingredients/Create
        /// </summary>
        /// <verb>POST</verb>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Ingredients ingredient) {
            ViewBag.Action = "Create";
            try {
                ingredient.Create();
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
        #endregion

        #region "Edit"
        /// <summary>
        ///  GET: /Ingredients/Edit/5
        /// </summary>
        /// <verb>GET</verb>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id) {
            ViewBag.Action = "Edit";
            Ingredients ingredient = (new Ingredients()).Get<Ingredients>(id);
            return View("Edit", ingredient);
        }

        /// <summary>
        /// POST: /Ingredients/Edit/5
        /// </summary>
        /// <verb>POST</verb>
        /// <param name="id"></param>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, Ingredients ingredient) {
            ViewBag.Action = "Edit";
            try {
                ingredient.Ingredient_pk = id;
                ingredient.Update();
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// GET: /Ingredients/Delete/5
        /// </summary>
        /// <verb>GET</verb>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id) {
            Ingredients ingredient = (new Ingredients()).Get<Ingredients>(id);
            return View("Delete", ingredient);
        }

        /// <summary>
        /// POST: /Ingredients/Delete/5
        /// </summary>
        /// <verb>POST</verb>
        /// <param name="id"></param>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, Ingredients ingredient) {
            try {
                ingredient.Ingredient_pk = id;
                ingredient.Delete();
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
        #endregion 

    }
}
