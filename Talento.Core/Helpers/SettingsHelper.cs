using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public IPagedList<ApplicationParameter> GetPagination(int pageSize = 5, int page = 1, string orderBy = "CreationDate", string order = "ASC", string filter = "")
        {
            var settings = Db.ApplicationParameter.ToList().OrderByDescending(x => x.CreationDate).ToPagedList(page, pageSize); ;
        
            return settings;
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
            catch (Exception)
            {
                throw;
            }
        }

        public List<ApplicationSetting> GetParameters(string prefix)
        {
            return Db.ApplicationSettings.Where(s => s.SettingName.StartsWith(prefix)).ToList();
        }
    }
}
