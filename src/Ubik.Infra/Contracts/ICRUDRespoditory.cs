namespace Ubik.Infra.Contracts
{
    public interface ICRUDRespoditory<T> : IReadRepository<T>, IReadAsyncRepository<T>, IWriteRepository<T>, IWriteAsyncRepository<T> where T : class
    {
    }
}