using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
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
                cfg.CreateMap<ApplicationParameter, ApplicationParameterModels>();
            });
        }
        //http://stackoverflow.com/questions/14677889/automapper-missing-type-map-configuration-or-unsupported-mapping

        // List All Settings
        [ChildAndAjaxActionOnly]
        public ActionResult List(int pageSize = 2, int page = 1, string orderBy = "CreationDate", string order = "ASC", string filter = "")
        {
            var parameterSettings = SettingsHelper.GetPagination(pageSize, page, orderBy, order, filter);
            return PartialView(parameterSettings);
        }

        // POST: Settings/New
        [HttpPost]
        public ActionResult Create(CreateApplicationSettingsViewModel applicationSetting) 
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
            return new HttpStatusCodeResult(500);
        }

        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Settings/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

        public ActionResult Modal(string prefix)
        {
            var retu = SettingsHelper.GetParameters(prefix);
            return Json(retu, JsonRequestBehavior.AllowGet);
        }
    }
}
