using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Managers
{
    public class UbikSignInManager<TUser> : SignInManager<TUser> where TUser : UbikUser
    {
        public UbikSignInManager(UbikUserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
        }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        //{
        //    return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        //}

        //public static UbikSignInManager Create(IdentityFactoryOptions<UbikSignInManager> options, IOwinContext context)
        //{
        //    return new UbikSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        //}
    }
}