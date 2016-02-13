namespace Ubik.Web.Components.Contracts
{
    public interface IModuleDescriptor
    {
        string FriendlyName { get; }
        string Summary { get; }
        string ModuleGroup { get; }
        ModuleType ModuleType { get; }

        BasePartialModule Default();
    }
}