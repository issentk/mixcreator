using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace MixCreator.Model
{
    public interface IContext<T> : IDisposable where T : class
    {
        DbSet<T> GetTable();
        DatabaseFacade GetDatabase();
        void Update(T obj);
        void Save();
    }
}
