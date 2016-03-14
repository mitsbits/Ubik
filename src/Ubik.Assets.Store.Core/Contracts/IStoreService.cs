using System.Threading.Tasks;

namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IStoreService<TKey>
    {
        Task<IFileInfo<TKey>> Upload(byte[] content, string fileName, string parentFolder);


    }


}