using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.Membership
{
    public class ResidentSecurity : IResidentSecurity
    {
        private readonly ICollection<Claim> _systemRoles;
        private readonly IEnumerable<IResourceAuthProvider> _providers;
        private readonly IDictionary<string, IEnumerable<Claim>> _roleToClaims;

        public ResidentSecurity(IEnumerable<IResourceAuthProvider> providers)
        {
            _providers = providers;
            _roleToClaims = new Dictionary<string, IEnumerable<Claim>>();
            _systemRoles = new HashSet<Claim>(_providers
                    .SelectMany(x => x.RoleNames)
                    .Distinct()
                    .Select(x => new Claim(SystemRoles.RoleClaimType, x)));
            foreach (var roleClaim in Roles)
            {
                _roleToClaims.Add(roleClaim.Value,
                    _providers
                    .SelectMany(x => x.Claims(roleClaim.Value)
                        .Distinct()));
            }
        }

        //TODO : this is not implemented correctly, see user view model service
        public IEnumerable<Claim> Roles
        {
            get { return _systemRoles; }
        }

        public IEnumerable<Claim> ClaimsForRole(string role)
        {
            return _roleToClaims[role];
        }
    }
}