﻿using System.Collections.Generic;
using System.Web;
using Talento.Entities;

namespace Talento.Core.Utilities
{
    public class Pagination
    {
        public int Next { get; set; }
        public int Prev { get; set; }
        public int Current { get; set; }
        public int Total { get; set; }
        public string Url { get; set; }
    }

    public class UtilityApplicationSettings: IUtilityApplicationSettings
    {
        public string GetSetting(string settingName, string parameterName)
        {
            var current = HttpContext.Current;
            List<ApplicationSetting> settings = HttpContext.Current.Application["AppSettings"] as List<ApplicationSetting>;
            var retu = settings.Find(x => x.SettingName.ToLower() == settingName.ToLower() && x.ParameterName.ToLower() == parameterName);
            if (retu != null)
            {
                return retu.ParameterValue;
            }
            return null;
        }

        public List<ApplicationSetting> GetAllSettings()
        {
            List<ApplicationSetting> settings = HttpContext.Current.Application["AppSettings"] as List<ApplicationSetting>;
            return settings;
        }
    }

    public interface IUtilityApplicationSettings
    {
        string GetSetting(string settingName, string parameterName);
        List<ApplicationSetting> GetAllSettings();
    }
}
