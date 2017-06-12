using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talento.Core;
using Talento.Core.Utilities;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    [HandleError]
    public class SettingsController : Controller
    {
        IApplicationSetting SettingsHelper;
        ICustomUser UserHelper;
        IUtilityApplicationSettings AppSettings;

        public SettingsController(IApplicationSetting settingsHelper, ICustomUser userHelper, IUtilityApplicationSettings appSettings)
        {
            SettingsHelper = settingsHelper;
            UserHelper = userHelper;
            AppSettings = appSettings;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationSetting, ApplicationSettingModel>()
                    .ForMember(apsm => apsm.ApplicationSettingId, aps => aps.MapFrom(s => s.ApplicationSettingId));
            });
        }

        // List All Settings
        public ActionResult List(int pageSize = 5, int page = 1, string orderBy = "CreationDate", string filter = "")
        {
            // Set ViewBag
            ViewBag.SortCurrent = (orderBy == "CreationDate") ? "CreationDate_asc" : "CreationDate";
            ViewBag.SortGroup = (orderBy == "SettingName") ? "SettingName_asc" : "SettingName";
            ViewBag.SortName = (orderBy == "ParameterName") ? "ParameterName_asc" : "ParameterName";
            ViewBag.SortValue = (orderBy == "ParameterValue") ? "ParameterValue_asc" : "ParameterValue";
            ViewBag.SortDate = (orderBy == "CreationDate") ? "CreationDate_asc" : "CreationDate";
            ViewBag.SortUser = (orderBy == "CreatedBy") ? "CreatedBy_asc" : "CreatedBy";
            ViewBag.CurrentFilter = filter;
            ViewBag.CurrentSort = orderBy;

            var parameterSettings = SettingsHelper.GetPagination(orderBy, filter);
            // Pagination
            var pageSizeValue = AppSettings.GetSetting("pagination", "pagesize"); // Setting Parameter

            if ( pageSizeValue != null)
            {
                pageSize = Convert.ToInt32( pageSizeValue );
            } 
            var paginated = parameterSettings.ToPagedList(page, pageSize);

            int total = parameterSettings.Count();
            int totalPages = (total - 1) / pageSize + 1;
            
            // Check for valid page
            if (page > totalPages || page < 1)
            {
                return new HttpStatusCodeResult(405);
            }
            return PartialView(paginated);
        }

        // POST: Settings/New
        [HttpPost]
        public ActionResult Create(ApplicationSettingCreateModel applicationSetting)
        {
            try
            {
                if (IsStateValid())
                {
                    string user = User.Identity.GetUserId();

                    ApplicationSetting aS = new ApplicationSetting
                    {
                        SettingName = applicationSetting.SettingName,
                        ParameterName = applicationSetting.ParameterName,
                        ParameterValue = applicationSetting.ParameterValue,
                        CreationDate = DateTime.Now,
                        CreatedBy = UserHelper.GetUserById(user),
                    };
                    SettingsHelper.Create(aS);
                    return new HttpStatusCodeResult(200);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }
            return new HttpStatusCodeResult(500);
        }

        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
        }

        // GET: Settings/Edit/5
        public ActionResult EditSettingsForm(int id)
        {
            ApplicationSettingModel editApplicationSettingsVM = AutoMapper.Mapper.Map<ApplicationSettingModel>(SettingsHelper.GetById(id));

            return View(editApplicationSettingsVM);
        }

        // POST: Settings/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationSettingModel appParameterVM)
        {
            try
            {
                ApplicationSettingModel ApplicationParameter = AutoMapper.Mapper.Map<ApplicationSettingModel>(SettingsHelper.GetById(appParameterVM.ApplicationSettingId));

                if (ApplicationParameter == null)
                {
                    return HttpNotFound();
                }
                string userId = User.Identity.GetUserId();

                ApplicationSetting aP = new ApplicationSetting
                {
                    ApplicationUser_Id = ApplicationParameter.CreatedBy_Id,
                    ApplicationSettingId = ApplicationParameter.ApplicationSettingId,
                    SettingName = appParameterVM.SettingName,
                    ParameterName = appParameterVM.ParameterName,
                    ParameterValue = appParameterVM.ParameterValue,
                    CreationDate = DateTime.Now,
                    CreatedBy = UserHelper.GetUserById(userId)
                    
                };

                SettingsHelper.Edit(aP);
                //return null;
                return new HttpStatusCodeResult(200); 
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }
        }
             
        //Modal
        public JsonResult GetApplicationParameters(string prefix)
        {
            var result = SettingsHelper.GetParameters(prefix);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
