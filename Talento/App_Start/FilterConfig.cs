using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Talento
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    // Child and Ajax Only Filter
    public class ChildAndAjaxActionOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if( !filterContext.IsChildAction )
            {
                if( !filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ViewDataDictionary data = new ViewDataDictionary()
                    {
                        new KeyValuePair<string, object>( "ErrorMessage", "405 Access denied" )
                    };
                    filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml", ViewData = data };
                }
            }
        }
    }
}
