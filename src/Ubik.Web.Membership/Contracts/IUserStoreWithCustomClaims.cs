using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ubik.Web.Membership.Contracts
{
    public interface IUserStoreWithCustomClaims<in TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<Claim>> RoleRelatedClaims(TKey userId);
    }

    public interface IUbikUserStore<in TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<string>> GetRoleNamesForUser(TKey userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}