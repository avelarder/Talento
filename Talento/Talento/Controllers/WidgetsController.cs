using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Talento.Controllers
{
    [Authorize]
    public class WidgetsController : Controller
    {
        // GET: Widget
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult DataTable(IList Data)
        {
            ViewBag.Data = Data;
            return View();
        }
    }
}