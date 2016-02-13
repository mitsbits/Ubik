using Microsoft.AspNet.Authorization;
using Ubik.Web.Membership;

namespace Ubik.Web.Client.Backoffice
{
    public class AuthorizeRoleRequirement : AuthorizationHandler<AuthorizeRoleRequirement>, IAuthorizationRequirement
    {
        public string RoleName { get; set; }

        protected override void Handle(AuthorizationContext context, AuthorizeRoleRequirement requirement)
        {
            if (context.User.HasClaim(SystemRoles.RoleClaimType, RoleName)) context.Succeed(requirement);
            context.Fail();
        }
    }
}