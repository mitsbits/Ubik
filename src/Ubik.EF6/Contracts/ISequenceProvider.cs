using System.Data.Entity.Infrastructure;

namespace Ubik.EF6.Contracts
{
    public interface ISequenceBase { }

    public interface ISequenceProvider
    {
        void Next(DbEntityEntry entry);
    }
}