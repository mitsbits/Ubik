using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.SSO;


namespace Ubik.Web.Membership.Managers
{
    public class UbikRoleManager<TRole> : RoleManager<TRole> where TRole : UbikRole
    {
        public UbikRoleManager(IRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }

        public async Task ClearClaims(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var store = Store as IRoleStoreWithCustomClaims;
            if (store == null) throw new ArgumentNullException(nameof(store));
            await store.SetRoleClaims(role, new List<Claim>(), cancellationToken);
        }

        public async Task SetClaims(TRole role, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            var store = Store as IRoleStoreWithCustomClaims;
            if (store == null) throw new ArgumentNullException(nameof(store));
            await store.SetRoleClaims(role, claims, cancellationToken);
        }

        public async Task<IEnumerable<UbikRole>> AllRoles()
        {
            var store = Store as IRoleStoreWithCustomClaims;
            if (store == null) throw new ArgumentNullException(nameof(store));
            return await store.Query(x => true, x => x.Claims);
        }

    }
}