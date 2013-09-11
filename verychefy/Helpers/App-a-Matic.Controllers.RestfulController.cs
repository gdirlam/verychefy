using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App_a_matic.Helper;
using App_a_matic;
using App_a_matic.Orm; 
using System.Net;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;


namespace App_a_Matic.Controllers {
    public abstract class RestfulController : Controller {

        protected ActionResult Default<T>( int? id) where T: IOrmModel, new(){
            if (id <= 0 || id == null) {
                try {
                    var models =  (IOrmModel) new T();
                    List<T> list = models.GetInParallel<T>();
                    this.Response.StatusCode = 200;
                    this.Response.StatusDescription = "OK";
                    return this.Content(SerializeToXML<List<T>>(list).ToString(), "text/xml");
                } catch (SqlException e) {
                    if (e.Message.Contains("No Records Found Matching the Search Criteria")) {
                        List<T> list = new List<T>(new T[] { new T() });
                        this.Response.StatusCode = 206;
                        this.Response.StatusDescription = "No Data Found in Persistance Store";

                        return this.Content(SerializeToXML<List<T>>(list).ToString(), "text/xml");
                    } else {
                        return new HttpStatusCodeResult(404, "Not Found");
                    }
                } catch (Exception) {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }
            } else {
                try {
                    var model =  (IOrmModel) new T();
                    var item = model.Get<T>((int)id);

                    this.Response.StatusCode = 200;
                    this.Response.StatusDescription = "OK";
                    return this.Content(SerializeToXML<T>(item), "text/xml");
                } catch (SqlException e) {
                    if (e.Message.Contains("No Records Found Matching the Search Criteria")) {
                        return new HttpStatusCodeResult(204, "No Content");
                    } else {
                        return new HttpStatusCodeResult(404, "Not Found");
                    }
                } catch (Exception) {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }



            }
        }

        protected ActionResult DefaultJson<T>(int? id) where T : IOrmModel, new() {
            if (id <= 0 || id == null) {
                try {
                    var models =  (IOrmModel) new T() ;
                    var list = models.GetInParallel<T>();

                    this.Response.StatusCode = 200;
                    this.Response.StatusDescription = "OK";
                    return Json(list, JsonRequestBehavior.AllowGet);
                } catch (SqlException e ) {
                    if (e.Message.Contains("No Records Found Matching the Search Criteria")) {
                        List<T> list = new List<T>(new T[] { new T() });
                        this.Response.StatusCode = 206;
                        this.Response.StatusDescription = "No Data Found in Persistance Store";
                        return Json(list, JsonRequestBehavior.AllowGet);

                    } else {
                        return new HttpStatusCodeResult(404, "Not Found");
                    }
                } catch (Exception) {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }

            } else {
                try {
                    var model = (IOrmModel)new T();
                    var item = model.Get<T>((int)id);

                    this.Response.StatusCode = 200;
                    this.Response.StatusDescription = "OK";
                    return Json(item, JsonRequestBehavior.AllowGet);
                } catch (SqlException e) {
                    return new HttpStatusCodeResult(404, "Not Found");
                } catch (Exception) {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }

            }
        }

        protected ActionResult Delete<T>(int? id, IOrmModel Item) where T : IOrmModel, new() {
            int? Pk = Item.PrimaryKey().Value ?? id;

            if (Pk != null && Pk > 0) {
                try {
                    var model = (IOrmModel)new T();
                    var item = model.Get<T>((int)Pk);
                    try {
                        item.Delete();
                    } catch (Exception) {
                        return new HttpStatusCodeResult(505, "Found but not deleted");
                    }
                } catch (SqlException) {
                    return new HttpStatusCodeResult(404, "Not Found");
                } catch (Exception) {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }

                return new HttpStatusCodeResult(204, "No Content");
            } else {
                return new HttpStatusCodeResult(404, "Not Found");
            }
        }

        protected JsonResult Update<T>(IOrmModel item) {
            int? Pk = item.PrimaryKey().Value;

            if (Pk != null && Pk > 0) {

                item.Update();
            } else {
                item.Create();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <lift>http://stackoverflow.com/questions/1564718/using-stringwriter-for-xml-serialization</lift>
        /// <returns></returns>
        public static string SerializeToXML<T>(T value) {

            if (value == null) {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter()) {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings)) {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }


        public class HttpOptions : ActionMethodSelectorAttribute {

            public override bool IsValidForRequest( ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo) {
                if (controllerContext.RequestContext.HttpContext.Request.HttpMethod == "OPTIONS") {
                    return true; 
                } else {
                    return false;
                }
            }
        }

        public class JsonContent : ActionMethodSelectorAttribute {

            public override bool IsValidForRequest(
                ControllerContext controllerContext
                , System.Reflection.MethodInfo methodInfo) {

                var Request = controllerContext.HttpContext.Request;
                string requestedWith = Request.ServerVariables["HTTP_X_REQUESTED_WITH"] ?? string.Empty;
                return string.Compare(requestedWith, "XMLHttpRequest", true) == 0
                    && Request.ContentType.ToLower().Contains("application/json");
            }
        }

        public class NotJson : ActionMethodSelectorAttribute {

            public override bool IsValidForRequest(
                ControllerContext controllerContext
                , System.Reflection.MethodInfo methodInfo) {

                var Request = controllerContext.HttpContext.Request;
                string requestedWith = Request.ServerVariables["HTTP_X_REQUESTED_WITH"] ?? string.Empty;
                bool isJson = string.Compare(requestedWith, "XMLHttpRequest", true) == 0
                        && Request.ContentType.ToLower().Contains("application/json");

                return !isJson;

            }
        }


    }
}

