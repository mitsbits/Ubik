using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Ubik.Web.Membership;

namespace Ubik.Web.Client.Backoffice
{
    public class AuthorizeOperationToResourceRequirement : AuthorizationHandler<AuthorizeOperationToResourceRequirement>, IAuthorizationRequirement
    {
        public string ResourceKey { get; set; }

        public string OperationKey { get; set; }

        protected virtual Claim InternalClaim => SystemClaims.OperationToResource(OperationKey, ResourceKey);

        protected override void Handle(AuthorizationContext context, AuthorizeOperationToResourceRequirement requirement)
        {
            if (context.User.HasClaim(SystemClaims.OperationToResourceClaimType, InternalClaim.Value)) context.Succeed(requirement);
            context.Fail();
        }
    }
}