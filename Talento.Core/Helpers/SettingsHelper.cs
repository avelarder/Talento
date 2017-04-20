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
        public SettingsHelper(Core.Data.ApplicationDbContext db) : base(db)
        {

        }

        public ApplicationSetting Get(string name)
        {
            var aS = Db.ApplicationSettings.Find(name);
            return aS;
        }

        public List<ApplicationSetting> GetAll()
        {
            var settings = Db.ApplicationSettings.ToList();
            return settings;
        }

        public void Create(ApplicationSetting aS)
        {
            ApplicationSetting appS = this.Get(aS.SettingName);

            if (appS != null)
            {
                Db.ApplicationSettings.Add(aS);
            }
            Db.SaveChanges();
        }
    }
}
