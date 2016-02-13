using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Contracts
{
    public interface IUserAdminstrationService
    {
        IEnumerable<UbikUser> Find(Expression<Func<UbikUser, bool>> predicate, int pageNumber, int pageSize, out int totalRecords);

        IEnumerable<UbikRole> Find(Expression<Func<UbikRole, bool>> predicate, int pageNumber, int pageSize, out int totalRecords);

        UbikUser CreateUser(UbikUser user, string password);

        Task<IdentityResult> SetRoles(UbikUser user, string[] newRoles);

        Task CopyRole(string source, string target);

        Task DeleteRole(string name);

        Task LockUser(string userId, int days);

        Task UnockUser(string userId);

        Task SetPassword(string userId, string newPassword);
    }
}