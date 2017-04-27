﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class SettingsHelper : BaseHelper, IApplicationSetting
    {
        public SettingsHelper(Data.ApplicationDbContext db) : base(db)
        {

        }

        public ApplicationSetting GetByName(string name)
        {
            var aS = Db.ApplicationSettings.FirstOrDefault(p => p.SettingName == name);
            return aS;
        }

        public ApplicationParameter GetById(int id)
        {
            var aS = Db.ApplicationParameter.FirstOrDefault(p => p.ApplicationSettingId == id);
            return aS;
        }

        public List<ApplicationParameter> GetAll()
        {
            var settings = Db.ApplicationParameter.ToList();
            return settings;
        }

        public List<ApplicationParameter> GetPagination(string orderBy = "CreationDate_desc", string filter = "")
        {
            try
            {
                // Get All
                var settings = from p in Db.ApplicationParameter select p;

                filter = filter.Trim();
                // Filter
                if (filter != "")
                {
                    var settingSearch = settings.Select(x => new
                    {
                        SettingName = x.ApplicationSetting.SettingName,
                        ParameterName = x.ParameterName,
                        ParameterValue = x.ParameterValue,
                        CreationDate = x.CreationDate,
                        CreatedBy = x.CreatedBy.Email,
                        ApplicationParameterId = x.ApplicationParameterId
                    });
                    var settingMatch = settingSearch.ToList().Where(x => x.GetType()
                        .GetProperties()
                        .Any(p =>
                        {
                            var value = p.GetValue(x).ToString().ToLower();
                            return value != null && value.ToString().ToLower().Contains(filter);
                        }));
                    var listIds = settingMatch.Select(x => x.ApplicationParameterId).ToList();
                    settings = settings.Where(x => listIds.Contains(x.ApplicationParameterId));
                }
                // Order List
                switch (orderBy)
                {
                    case "CreationDate":
                        settings = settings.OrderByDescending(p => p.CreationDate);
                        break;
                    case "CreationDate_asc":
                        settings = settings.OrderBy(p => p.CreationDate);
                        break;
                    case "ParameterName":
                        settings = settings.OrderByDescending(p => p.ParameterName);
                        break;
                    case "ParameterName_asc":
                        settings = settings.OrderBy(p => p.ParameterName);
                        break;
                    case "CreatedBy":
                        settings = settings.OrderByDescending(p => p.CreatedBy.Email);
                        break;
                    case "CreatedBy_asc":
                        settings = settings.OrderBy(p => p.CreatedBy.Email);
                        break;
                    case "ParameterValue":
                        settings = settings.OrderByDescending(p => p.ParameterValue);
                        break;
                    case "ParameterValue_asc":
                        settings = settings.OrderBy(p => p.ParameterValue);
                        break;
                    case "SettingName":
                        settings = settings.OrderByDescending(p => p.ApplicationSetting.SettingName);
                        break;
                    case "SettingName_asc":
                        settings = settings.OrderBy(p => p.ApplicationSetting.SettingName);
                        break;
                    default:  // Date descending 
                        settings = settings.OrderByDescending(p => p.CreationDate);
                        break;
                }
                // Pagination
                if (!settings.Any())
                {
                    return null;
                }

                var paginated = settings.ToList();

                return settings.ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Create(ApplicationSetting aS)
        {
            try
            {
                ApplicationSetting applicationSetting = this.GetByName(aS.SettingName);

                if (applicationSetting == null)
                {
                    Db.ApplicationSettings.Add(aS);
                }
                else
                {
                    foreach (var p in aS.ApplicationParameter)
                    {
                        applicationSetting.ApplicationParameter.Add(p);
                    }
                }
                Db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> GetParameters(string prefix)
        {
            return Db.ApplicationSettings.Where(s => s.SettingName.StartsWith(prefix)).Select(x => x.SettingName).ToList();
        }

        public void Edit(ApplicationParameter aP)
        {

            ApplicationSetting asToChange = this.GetByName(aP.ApplicationSetting.SettingName); //SettingName nuevo
            ApplicationParameter apToChange = this.GetById(aP.ApplicationParameterId); //Db.ApplicationSettings.Find(s => s....

            if (asToChange.ApplicationParameter.Count() == 1)
            {
                if (aP.ApplicationSetting.SettingName != apToChange.ApplicationSetting.SettingName)
                {
                    asToChange.SettingName = apToChange.ApplicationSetting.SettingName;
                }
                apToChange.CreatedBy = aP.CreatedBy;
                apToChange.CreationDate = aP.CreationDate;
                apToChange.ParameterName = aP.ParameterName;
                apToChange.ParameterValue = aP.ParameterValue;
            }
            else
            {
                if (aP.ApplicationSetting.SettingName != apToChange.ApplicationSetting.SettingName)
                {
                    if (apToChange != null)
                    {
                        Db.ApplicationParameter.Remove(apToChange);
                        var appSettingDefault = aP.ApplicationSetting;
                        var appSetting = new ApplicationSetting() {
                            ApplicationParameter = new List<ApplicationParameter>(),
                            SettingName = aP.ApplicationSetting.SettingName
                        };
                        appSetting.ApplicationParameter.Add(aP);
                    }
                }
            }
                apToChange.CreatedBy = aP.CreatedBy;
                apToChange.CreationDate = aP.CreationDate;
                apToChange.ParameterName = aP.ParameterName;
                apToChange.ParameterValue = aP.ParameterValue;

                var apDb = asToChange.ApplicationParameter.First(x => x.ApplicationParameterId == apToChange.ApplicationParameterId);
                apDb = apToChange;
                
                Db.ApplicationSettings.Where(a => a.ApplicationSettingId == aP.ApplicationSettingId).
                // Save DB
                Db.SaveChanges();
            }

        //if (aP.ParameterName != apToChange.ParameterName) {
        //    apToChange.ParameterName = aP.ParameterName;
        //}
        //if (aP.ParameterValue != apToChange.ParameterValue)
        //{
        //    apToChange.ParameterValue = aP.ParameterValue;
        //}
        //if (aP.ApplicationSetting.SettingName != aP.ApplicationSetting.SettingName)
        //{
        //    asToChange.SettingName = aP.ApplicationSetting.SettingName;
        //}

        //Db.ApplicationParameter.Single(x => x.ApplicationParameterId == aP.ApplicationParameterId).ApplicationSetting.SettingName = aP.ApplicationSetting.SettingName;
        //    Db.ApplicationParameter.Single(x => x.ApplicationParameterId == aP.ApplicationParameterId).ParameterName = aP.ParameterName;
        //    Db.ApplicationParameter.Single(x => x.ApplicationParameterId == aP.ApplicationParameterId).ParameterValue = aP.ParameterValue;
        //    Db.ApplicationParameter.Single(x => x.ApplicationParameterId == aP.ApplicationParameterId).CreationDate = aP.CreationDate;
        //    Db.ApplicationParameter.Single(x => x.ApplicationParameterId == aP.ApplicationParameterId).CreatedBy = aP.CreatedBy;

        }
    }
}
