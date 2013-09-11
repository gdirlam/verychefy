using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using App_a_matic.Orm;
using App_a_matic.Helper;


namespace App_a_matic.Controllers {
    public class ApiController: System.Web.Http.ApiController {
      
        protected List<T> Get<T>()  where T : IOrmModel, new()  {
            var models = (IOrmModel)new T();
            List<T> list = models.GetInParallel<T>();
            return list;
        }

        protected T Get<T>(int id) where T : IOrmModel, new() {
            var item = (IOrmModel)new T();
            var model = item.Get<T>((int)id);
            return (T)Convert.ChangeType(model, typeof(T));
        }

        protected void Delete<T>(int id) where T : IOrmModel, new() {
            var item= (IOrmModel)new T();
            var model  = item.Get<T>(id);
            item.Delete(id);      
        }

        protected void Put<T>(int id, IOrmModel model) where T : IOrmModel, new() {
            int? Pk = model.PrimaryKey().Value;
            model.Update();
        }

        protected void Post<T>( IOrmModel model) where T : IOrmModel, new() {
            model.Create();
        }


    }
}