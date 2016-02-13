using System.Data.Entity;

namespace Ubik.EF6.Contracts
{
    public interface IRepositoryWithContext<out TDbContext> where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
    }
}