using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Contracts
{
    public interface IRoleStoreWithCustomClaims
    {
        Task<IEnumerable<UbikRole>> Query(Expression<Func<UbikRole, bool>> predicate, params Expression<Func<UbikRole, dynamic>>[] paths);

        //Task ClearRoleClaims(UbikRole role, CancellationToken cancellationToken);

        Task SetRoleClaims(UbikRole role, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken));
    }
}