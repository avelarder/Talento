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
        Task Create(Position log);
        void Edit(Position log, string EmailModifier);
        Task Delete(int Id);
    }
}