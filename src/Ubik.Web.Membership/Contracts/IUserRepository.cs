using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Contracts
{
    public interface IUserRepository : ICRUDRespoditory<UbikUser>
    {
        Task RemoveFromRole(int userId, string roleName, CancellationToken cancelationToken);

        Task<IEnumerable<string>> GetRoleNames(int userId, CancellationToken cancelationToken);

        Task<bool> IsInRole(int userId, string roleName, CancellationToken cancelationToken);

        Task<IEnumerable<UbikUserClaim>> GetUserClaims(int userId, CancellationToken cancelationToken);

        Task<IEnumerable<UbikRoleClaim>> RoleRelatedClaims(int userId, CancellationToken cancelationToken);

        Task RemoveLogin(int userId, string loginProvider, string providerKey, CancellationToken cancellationToken);

        Task<IList<UbikUser>> GetUsersInRole(string roleName, CancellationToken cancellationToken);

        Task SetUserRole(int userId, int roleId);
    }
}