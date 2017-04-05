﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface IPosition
    {
        Task<Position> Get(int Id);
        Task<List<Position>> GetAll();
        void Create(Position log);
        Task Edit(Position log);
        Task Delete(int Id);
        ApplicationUser SearchPM(string userName);
        ApplicationUser GetUser(string user);
    }
}
