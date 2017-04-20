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
                cfg.CreateMap<ApplicationSetting, ApplicationSettingModels>()
                    .ForMember(s => s.ApplicationSettingId , opt => opt.MapFrom(o => o.ApplicationSettingId))
                ;
            });
        }

        // POST: Settings/New
        [HttpPost]
        public ActionResult New(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
