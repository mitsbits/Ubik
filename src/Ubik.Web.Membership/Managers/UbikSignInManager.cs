using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.ViewModels;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Managers
{
    public class UbikSignInManager<TUser, TRole> : SignInManager<TUser> where TUser : UbikUser where TRole : UbikRole
    {
        private readonly IEnumerable<IResourceAuthProvider> _authProviders;
        private readonly UbikRoleManager<TRole> _roleManager;

        public UbikSignInManager(UbikUserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            UbikRoleManager<TRole> roleManager,
            IEnumerable<IResourceAuthProvider> authProviders)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
            _roleManager = roleManager;
            _authProviders = authProviders;
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user)
        {
            var result = await base.CreateUserPrincipalAsync(user);
            var claimsIdentity = result.Identity as ClaimsIdentity;
            var roleClaims = await RoleRelatedClaims(user);

            foreach (var c in roleClaims)
            {
                claimsIdentity.AddClaim(c);
            }

            return result;
        }

        private async Task<IEnumerable<Claim>> RoleRelatedClaims(TUser user)
        {
            var systemRoles = new List<RoleViewModel>(_authProviders.RoleModels());
            var dbRoles = await _roleManager.AllRoles();
            foreach (var role in systemRoles)
            {
                var hit = dbRoles.FirstOrDefault(x => x.Name == role.Name);
                role.RoleId = (hit == null) ? -1 : hit.Id;
            }
            var userId = user.Id;
            var userclaims = systemRoles.Where(x => user.Roles.Any(r => r.RoleId == x.RoleId)).SelectMany(x => x.Claims).Distinct().Select(c => new Claim(c.Type, c.Value)).ToList();
            return userclaims;
        }
    }
}