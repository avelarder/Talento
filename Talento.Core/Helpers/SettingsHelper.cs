﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class SettingsHelper : BaseHelper, IApplicationSetting
    {
        public SettingsHelper(Core.Data.ApplicationDbContext db): base(db)
        {

        }

        public ApplicationSetting Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationSetting> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
