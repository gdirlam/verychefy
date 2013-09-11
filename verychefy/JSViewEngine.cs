using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace verychefy {
    public class JSViewEngine : RazorViewEngine {
        public JSViewEngine() {
            ViewLocationFormats = new[] { 
                "~/Views/{1}/{0}.csjs"
                , "~/Views/Shared/{0}.csjs" 
        };
            AreaViewLocationFormats = new[] { 
                "~/Areas/{2}/Views/{1}/{0}.csjs"
                , "~/Areas/{2}/Views/{1}/Shared/{0}.csjs"

        };
            FileExtensions = new[] { "csjs" };
        }
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath) {
            controllerContext.HttpContext.Response.ContentType = "text/javascript";
            return base.CreateView(controllerContext, viewPath, masterPath);
        }
    }
}