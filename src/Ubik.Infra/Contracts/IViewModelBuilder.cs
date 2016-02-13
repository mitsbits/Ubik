namespace Ubik.Infra.Contracts
{
    public interface IViewModelBuilder<in TEntity, TViewModel>
    {
        TViewModel CreateFrom(TEntity entity);

        void Rebuild(TViewModel model);
    }
}