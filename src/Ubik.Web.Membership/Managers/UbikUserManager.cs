using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.SSO;


namespace Ubik.Web.Membership.Managers
{
    public class UbikUserManager<TUser> : UserManager<TUser> where TUser : UbikUser 
    {
        public UbikUserManager(IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger,
            IHttpContextAccessor contextAccessor) : base(store,
                optionsAccessor, passwordHasher, userValidators,
                passwordValidators, keyNormalizer, errors, services, logger, contextAccessor)
        { }

        public async Task<IEnumerable<string>> GetRolesForUserAsync(string userId)
        {
            var ubikStore = Store as IUbikUserStore<int>;
            return ubikStore != null ? await ubikStore.GetRoleNamesForUser(int.Parse(userId)) : new List<string>();
        }
    }
}