using System.Threading.Tasks;

namespace Ubik.Infra.Contracts
{
    public interface IViewModelCommand<in TViewModel>
    {
        Task Execute(TViewModel model);
    }
}