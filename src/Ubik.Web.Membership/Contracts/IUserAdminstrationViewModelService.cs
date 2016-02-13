using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Membership.Contracts
{
    public interface IUserAdminstrationViewModelService
    {
        UserViewModel UserModel(string id);

        NewUserViewModel NewUserModel();

        RoleViewModel RoleModel(string id);

        RoleViewModel RoleByNameModel(string name);

        IEnumerable<UserRowViewModel> UserModels();

        IEnumerable<RoleViewModel> RoleModels();

        Task Execute(RoleSaveModel model);

        Task Execute(NewUserSaveModel model);

        Task Execute(UserSaveModel model);

        IEnumerable<RoleViewModel> SystemRoleViewModels { get; }
    }
}