using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System.Security.Claims;
using Ubik.Web.Membership;

namespace Ubik.Web.Client.Backoffice.Filters
{
    //TODO: move to auth policy
    public class AuthOpToResourceAttribute : ActionFilterAttribute
    {
        public string ResourceKey { get; set; }
        public string OperationKey { get; set; }

        protected virtual Claim InternalClaim
        {
            get { return SystemClaims.OperationToResource(OperationKey, ResourceKey); }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var claimsIdentity = httpContext.User as ClaimsPrincipal;
            if (claimsIdentity == null || !claimsIdentity.HasClaim(SystemClaims.OperationToResourceClaimType, InternalClaim.Value))
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }
    }
}