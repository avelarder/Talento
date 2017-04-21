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

        public List<ApplicationSetting> GetAll()
        {
            var settings = Db.ApplicationSettings.ToList();
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
