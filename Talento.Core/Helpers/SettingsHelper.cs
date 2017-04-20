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
        public SettingsHelper(Data.ApplicationDbContext db): base(db)
        {

        }

        public ApplicationSetting Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationSetting> GetAll()
        {
            var settings = Db.ApplicationSettings.ToList();
            return settings;
        }

        public List<ApplicationSetting> GetParameters(string prefix)
        {
            return Db.ApplicationSettings.Where(s => s.SettingName.StartsWith(prefix)).ToList();
        }
    }
}
