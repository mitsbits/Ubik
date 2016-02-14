using System;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Ubik.EF6.Contracts
{
    public interface ISequenceBase {
        int Id { get; }
    }

    public interface ISequenceProvider
    {
        void Next(DbEntityEntry entry);
        Task<int> GetNext(Type entityType);
    }
}