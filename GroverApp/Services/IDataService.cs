using GroverApp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroverApp.Services
{
    public interface IDataService<T> : IDisposable
    {
        void Load();
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
