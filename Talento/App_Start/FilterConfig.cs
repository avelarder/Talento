using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Talento.Core.Data;
using Talento.Core.Helpers;
using Talento.Entities;

namespace Talento
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    // Filter to load and update ApplicationSettings on Every Page
    public class ApplicationSettingsManager : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Application["AppSettingsDate"] == null)
            {
                SetData(filterContext);
            }
            else if ((DateTime)filterContext.HttpContext.Application["AppSettingsDate"] <= DateTime.Now)
            {
                SetData(filterContext);
            }
        }

        // Helper of Filter
        private void SetData(ActionExecutingContext filterContext)
        {
            var context = (ApplicationDbContext)DependencyResolver.Current.GetService(typeof(ApplicationDbContext));
            SettingsHelper sh = new SettingsHelper(context);
            var settings = sh.GetAll();
            filterContext.HttpContext.Application.Lock();
            filterContext.HttpContext.Application["AppSettings"] = settings as List<ApplicationSetting>;
            filterContext.HttpContext.Application["AppSettingsDate"] = DateTime.Now.AddMinutes(1);
            filterContext.HttpContext.Application.UnLock();
            base.OnActionExecuting(filterContext);
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
