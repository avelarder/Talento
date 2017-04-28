using PagedList;
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
            var aS = Db.ApplicationSetting.FirstOrDefault(p => p.SettingName == name);
            return aS;
        }

        public ApplicationSetting GetById(int id)
        {
            var aS = Db.ApplicationSetting.FirstOrDefault(p => p.ApplicationSettingId == id);
            return aS;
        }

        public List<ApplicationSetting> GetAll()
        {
            var settings = Db.ApplicationSetting.ToList();
            return settings;
        }

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

        public void Create(ApplicationSetting aS)
        {
            Db.ApplicationSetting.Add(aS);
            Db.SaveChanges();
        }

        public List<string> GetParameters(string prefix)
        {
            return Db.ApplicationSetting.Where(s => s.SettingName.StartsWith(prefix)).Select(x => x.SettingName).Distinct().ToList();
        }

        public void Edit(ApplicationSetting pApplicationSetting)
        {
            //row to edit
            var appSettingToEdit = Db.ApplicationSetting.First(x => x.ApplicationSettingId == pApplicationSetting.ApplicationSettingId);

            // Si ya existe la PK, eliminar la vieja y agregar una nueva
            if (appSettingToEdit.SettingName != pApplicationSetting.SettingName || appSettingToEdit.ParameterName != pApplicationSetting.ParameterName)
            {
                if (appSettingToEdit != null)
                {
                    Db.ApplicationSetting.Remove(appSettingToEdit);
                    Db.SaveChanges();
                }
            }
            
            //si no existe la PK se agrega
            appSettingToEdit.ApplicationUser_Id = pApplicationSetting.ApplicationUser_Id;
            appSettingToEdit.SettingName = pApplicationSetting.SettingName;
            appSettingToEdit.ParameterName = pApplicationSetting.ParameterName;
            appSettingToEdit.ParameterValue = pApplicationSetting.ParameterValue;
            appSettingToEdit.CreatedBy = pApplicationSetting.CreatedBy;
            appSettingToEdit.CreationDate = pApplicationSetting.CreationDate;

            Db.ApplicationSetting.Add(appSettingToEdit);
            Db.SaveChanges();
        }
    }
}
