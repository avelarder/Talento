using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Core.Utilities;
using Talento.Entities;

namespace Talento.Core
{
    public interface IPositionLog
    {
        void Add(Log log);
    }
}
