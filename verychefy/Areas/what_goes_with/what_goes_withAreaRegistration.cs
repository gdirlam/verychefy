using System.Web.Mvc;

namespace verychefy.Areas.what_goes_with {
    public class what_goes_withAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "what_goes_with";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {


            context.MapRoute(
                "what_goes_with_default",
                "what_goes_with",
                new { controller = "what_goes_with", action = "List", item = UrlParameter.Optional }
            );

            context.MapRoute(
                "what_goes_with_no_space",
                "what goes with",
                new { controller = "what_goes_with", action = "List", item = UrlParameter.Optional }
            );

            context.MapRoute(
                "what_goes_with_item",
                "what_goes_with/{item}",
                new { controller = "what_goes_with", action = "Details", item = UrlParameter.Optional }
            );

            context.MapRoute(
                "what_goes_with_item_no_space",
                "what goes with/{item}",
                new { controller = "what_goes_with", action = "Details", item = UrlParameter.Optional }
            );


            /*
            context.MapRoute(
                "what_goes_with_default",
                "what_goes_with/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, 

            );
             */
        }
    }
}
