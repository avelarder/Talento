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

        public SettingsController(IApplicationSetting settingsHelper)
        {
            SettingsHelper = settingsHelper;
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationSetting, ApplicationSettingModels>();
            });
        }

        // List All Settings
        public ActionResult List()
        {
            var settings = AutoMapper.Mapper.Map<ApplicationSettingModels>( SettingsHelper.GetAll() );
            return PartialView(settings);
        }

        // POST: Settings/New
        [HttpPost]
        public ActionResult Create(CreateApplicationSettingsViewModel cAppSettingsVM) //FormCollection collection
        {
            //var listData = _appFunctions.GetAllCategory();
            //model.CategoryList = new SelectList(listData, "CategoryTypeID ", "CategoryTitle");

            if (IsStateValid())
            {
                ApplicationSetting aS = new ApplicationSetting
                {
                    SettingName = cAppSettingsVM.SettingName,
                    ApplicationParameter = cAppSettingsVM.ApplicationParameter
                };

                SettingsHelper.Create(aS);
                return RedirectToAction("Dashboard", "AppSettings");
            }
            return RedirectToAction("Dashboard", "AppSettings");
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
