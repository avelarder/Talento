﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class TagHelper : BaseHelper, ITag
    {
        public TagHelper(Core.Data.ApplicationDbContext db) : base(db)
        {
        }

        public Task Create(Tag log)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(Tag log)
        {
            throw new NotImplementedException();
        }

        public Task<Tag> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tag>> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Tag> GetByPositionId(int Id)
        {
            //var tags = Db.Tags.Where(x => x.Position_Id == Id).ToList();
            //var tags = Db.Tags.Where(x => x.Position_Id == Id).ToList();
            //return tags;

            throw new NotImplementedException();
        }
    }
}