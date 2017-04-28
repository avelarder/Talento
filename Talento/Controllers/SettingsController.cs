using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    public class SettingsController : Controller
    {
        IApplicationSetting SettingsHelper;
        ICustomUser UserHelper;

        public SettingsController(IApplicationSetting settingsHelper, ICustomUser userHelper)
        {
            SettingsHelper = settingsHelper;
            UserHelper = userHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationSetting, ApplicationSettingModels>()
                    .ForMember(apsm => apsm.ApplicationSettingId, aps => aps.MapFrom(s => s.ApplicationSettingId))
                    .ForMember(apsm => apsm.ApplicationParameter, aps => aps.MapFrom(s => s.ApplicationParameter));
                cfg.CreateMap<ApplicationParameter, ApplicationParameterViewModel>();
                cfg.CreateMap<ApplicationSetting, ApplicationSettingsViewModel>();
            });
        }

        // List All Settings
        [ChildAndAjaxActionOnly]
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

            var parameterSettings = SettingsHelper.GetPagination(orderBy, filter);
            // Pagination
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
        public ActionResult Create(ApplicationSettingsViewModel applicationSetting)
        {
            try
            {
                if (IsStateValid())
                {
                    string user = User.Identity.GetUserId();

                    ApplicationSetting aS = new ApplicationSetting
                    {
                        SettingName = applicationSetting.SettingName,
                        ApplicationParameter = new List<ApplicationParameter> {
                        new ApplicationParameter {
                            ParameterName = applicationSetting.ParameterName,
                            ParameterValue = applicationSetting.ParameterValue,
                            CreationDate = DateTime.Now,
                            CreatedBy = UserHelper.GetUserById(user)
                        }
                    }
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
            ApplicationParameterViewModel editApplicationSettingsVM = AutoMapper.Mapper.Map<ApplicationParameterViewModel>(SettingsHelper.GetById(id));

            return PartialView(editApplicationSettingsVM);
        }

        // POST: Settings/Edit/5
        [HttpPost]
        [ChildAndAjaxActionOnly]
        public ActionResult Edit(ApplicationParameterViewModel appParamVM)
        {
            try
            {
                //ApplicationParameterViewModel ApplicationParameter = AutoMapper.Mapper.Map<ApplicationParameterViewModel>(SettingsHelper.GetById(applicationParameterVM.ApplicationParameterId));

                //if (ApplicationParameter == null)
                //{
                //    return HttpNotFound();
                //}

                ApplicationParameter aP = new ApplicationParameter {
                    ApplicationSetting = appParamVM.ApplicationSetting,
                    ApplicationSettingId = appParamVM.ApplicationSettingId,
                    ParameterName = appParamVM.ParameterName,
                    ParameterValue = appParamVM.ParameterValue,
                    CreatedBy = appParamVM.CreatedBy,
                    ApplicationParameterId = appParamVM.ApplicationParameterId,
                    ApplicationUser_Id = appParamVM.ApplicationUser_Id,
                    CreationDate = appParamVM.CreationDate
                };

                SettingsHelper.Edit(aP);

                return PartialView(aP);
            }
            catch (InvalidOperationException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The designated Application Setting does not have a valid ID");
            }
        }


        [Authorize]
        [HttpGet]
        public FileResult DownloadTiff()
        {
            return new FilePathResult("", "");
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Settings/Delete/5
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

        //Modal
        public JsonResult GetApplicationParameters(string prefix)
        {
            var retu = SettingsHelper.GetParameters(prefix);

            return Json(retu, JsonRequestBehavior.AllowGet);
        }

        // Test Action
        public ActionResult Test()
        {
            var retu = System.Web.HttpContext.Current.Session["AppSettings"] as String;
            return Content(retu);
        }

    }
}
