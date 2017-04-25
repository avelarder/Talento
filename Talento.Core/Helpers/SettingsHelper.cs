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

        public ApplicationSetting Get(string name)
        {
            var aS = Db.ApplicationSettings.FirstOrDefault(p => p.SettingName == name);


            return aS;
        }

        public List<ApplicationParameter> GetAll()
        {
            var settings = Db.ApplicationParameter.ToList();
            return settings;
        }

        public IPagedList<ApplicationParameter> GetPagination(int pageSize = 5, int page = 1, string orderBy = "CreationDate_desc", string filter = "")
        {
            try
            {
                // Get All
                var settings = from p in Db.ApplicationParameter select p;

                filter = filter.Trim();
                // Filter
                if (filter != "")
                {
                    var settingSearch = settings.Select(x => new {
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
                int total = paginated.Count;
                int totalPages = (total - 1) / pageSize + 1;

                if (page > totalPages || page < 1)
                {
                    return null;
                }

                return paginated.ToPagedList(page, pageSize);

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
                ApplicationSetting applicationSetting = this.Get(aS.SettingName);

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
            return Db.ApplicationSettings.Where(s => s.SettingName.StartsWith(prefix)).Select(x=> x.SettingName).ToList();
        }
    }
}
