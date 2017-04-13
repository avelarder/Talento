using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface IFileManagerHelper
    {
        Task<FileBlob> Get(int Id);
        Task<List<FileBlob>> GetAll();
        void Create(FileBlob file);
        void Delete(FileBlob file);
    }
}
