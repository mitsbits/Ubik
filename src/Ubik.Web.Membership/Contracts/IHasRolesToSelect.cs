using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Membership.Contracts
{
    public interface IHasRolesToSelect
    {
        RoleViewModel[] AvailableRoles { get; }
    }

    public interface IHasRoles
    {
        RoleViewModel[] Roles { get; }
    }
}