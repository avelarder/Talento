﻿using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using Talento.Core.Data;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class SettingsHelper : BaseHelper, IApplicationSetting
    {

        public SettingsHelper(ApplicationDbContext db) : base(db)
        {

        }


        /// <summary>
        /// Delete an app setting. 
        /// </summary>
        /// <param name="Id"></param>
        public void Delete(int Id)
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
            {
                ApplicationSetting tobedeleted = Db.ApplicationSetting.Single(u => u.ApplicationSettingId.Equals(Id)); //Get Current User
                var p = Db.Positions.Where(x => x.PositionId == Id).Single();
                Db.SaveChanges();
                tx.Complete();
            }
        }

        /// <summary>
        /// Get an ApplicationSetting by its name
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public ApplicationSetting GetByName(string settingName)
        {
            var aS = Db.ApplicationSetting.FirstOrDefault(p => p.SettingName == settingName);
            return aS;
        }

        /// <summary>
        /// Get an ApplicationSetting by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationSetting GetById(int id)
        {
            var aS = Db.ApplicationSetting.FirstOrDefault(p => p.ApplicationSettingId == id);
            return aS;
        }

        /// <summary>
        /// Get a list of all ApplicationSettings
        /// </summary>
        /// <returns></returns>
        public List<ApplicationSetting> GetAll()
        {
            var settings = Db.ApplicationSetting.ToList();
            return settings;
        }

        /// <summary>
        /// Get a list of ApplicationSettings using pagination. 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ApplicationSetting> GetPagination(string orderBy = "CreationDate", string filter = "")
        {
            try
            {
                // Get All
                var settings = from p in Db.ApplicationSetting select p;

                filter = filter.Trim();
                // Filter
                if (filter != "")
                {
                    var settingSearch = settings.Select(x => new
                    {
                        SettingName = x.SettingName,
                        ParameterName = x.ParameterName,
                        ParameterValue = x.ParameterValue,
                        CreationDate = x.CreationDate,
                        CreatedBy = x.CreatedBy.Email,
                        ApplicationSettingId = x.ApplicationSettingId
                    });
                    var settingMatch = settingSearch.ToList().Where(x => x.GetType()
                        .GetProperties()
                        .Any(p =>
                        {
                            var value = p.GetValue(x).ToString().ToLower();
                            return value != null && value.ToString().ToLower().Contains(filter);
                        }));
                    var listIds = settingMatch.Select(x => x.ApplicationSettingId).ToList();
                    settings = settings.Where(x => listIds.Contains(x.ApplicationSettingId));
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
                        settings = settings.OrderByDescending(p => p.SettingName);
                        break;
                    case "SettingName_asc":
                        settings = settings.OrderBy(p => p.SettingName);
                        break;
                    default:  // Date descending 
                        settings = settings.OrderByDescending(p => p.CreationDate);
                        break;
                }

                return settings.ToList<ApplicationSetting>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create a new applicationSetting
        /// </summary>
        /// <param name="aS"></param>
        /// <returns></returns>
        public int Create(ApplicationSetting aS)
        {
            Db.ApplicationSetting.Add(aS);
            int result = Db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Get a list of possible parameters by a prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public List<string> GetParameters(string prefix)
        {
            return Db.ApplicationSetting.Where(s => s.SettingName.StartsWith(prefix)).Select(x => x.SettingName).Distinct().ToList();
        }

        /// <summary>
        /// Edit an ApplicationSetting
        /// </summary>
        /// <param name="pApplicationSetting"></param>
        /// <returns></returns>
        public int Edit(ApplicationSetting pApplicationSetting)
        {
            try
            {
                //row to edit
                var appSettingToEdit = Db.ApplicationSetting.First(x => x.ApplicationSettingId == pApplicationSetting.ApplicationSettingId);

                Db.ApplicationSetting.Remove(appSettingToEdit);
                Db.SaveChanges();

                appSettingToEdit.ApplicationUser_Id = pApplicationSetting.ApplicationUser_Id;
                appSettingToEdit.SettingName = pApplicationSetting.SettingName;
                appSettingToEdit.ParameterName = pApplicationSetting.ParameterName;
                appSettingToEdit.ParameterValue = pApplicationSetting.ParameterValue;
                appSettingToEdit.CreatedBy = pApplicationSetting.CreatedBy;
                appSettingToEdit.CreationDate = pApplicationSetting.CreationDate;

                Db.ApplicationSetting.Add(appSettingToEdit);
                Db.SaveChanges();

                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
