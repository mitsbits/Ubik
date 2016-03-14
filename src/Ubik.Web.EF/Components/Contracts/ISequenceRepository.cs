using System.Threading.Tasks;
using Ubik.EF6.Contracts;

namespace Ubik.Web.EF.Components.Contracts
{
    public interface ISequenceRepository<T> where T : ISequenceBase
    {
        Task<int> GetNext();
    }
}