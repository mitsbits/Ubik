using Mehdime.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.SSO;
using System;
using System.Linq.Expressions;

namespace Ubik.Web.EF.Membership
{
    public class UserRepository : ReadWriteRepository<UbikUser, AuthDbContext>, IUserRepository
    {
        public UserRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public async Task<IEnumerable<string>> GetRoleNames(int userId, CancellationToken cancelationToken)
        {
            return await DbContext.Roles.AsNoTracking().Where(x => x.Users.Any(u => u.UserId == userId)).OrderBy(x => x.Name).Select(x => x.Name).ToListAsync(cancelationToken);
        }

        public async Task<IEnumerable<UbikUserClaim>> GetUserClaims(int userId, CancellationToken cancelationToken)
        {
            return await DbContext.UserClaims.Where(x => x.UserId == userId).ToListAsync(cancelationToken);
        }

        public async Task<IList<UbikUser>> GetUsersInRole(string roleName, CancellationToken cancellationToken)
        {
            var query = from u in DbContext.Users
                        join ur in DbContext.UserRoles on u.Id equals ur.UserId
                        join r in DbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == roleName
                        select u;
            return await query.ToListAsync(cancellationToken);
        }

        public Task<bool> IsInRole(int userId, string roleName, CancellationToken cancelationToken)
        {
            return Task.FromResult(DbContext.Roles.Any(x => x.Users.Any(u => u.UserId == userId)));
        }

        public async virtual Task RemoveFromRole(int userId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleEntity = await DbContext.Roles.Include(x => x.Users).FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower() && x.Users.Any(u => u.UserId == userId), cancellationToken);
            if (roleEntity != null)
            {
                var userToRemove = roleEntity.Users.FirstOrDefault(u => u.UserId == userId);
                roleEntity.Users.Remove(userToRemove);
            }
        }

        public async Task RemoveLogin(int userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await DbContext.Users.Include(x => x.Logins).FirstOrDefaultAsync(x => x.Id == userId);
            var loginsToRemove = user.Logins.Where(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            foreach(var loginToRemove in loginsToRemove)
            {
                user.Logins.Remove(loginToRemove);
            }

        }

        public async Task<IEnumerable<UbikRoleClaim>> RoleRelatedClaims(int userId, CancellationToken cancelationToken)
        {
            var q = from c in DbContext.RoleClaims
                    join r in DbContext.Roles on c.RoleId equals r.Id
                    join ur in DbContext.UserRoles on r.Id equals ur.RoleId
                    where ur.UserId == userId
                    select c;
            return await q.ToListAsync(cancelationToken);
        }

        public Task SetUserRole(int userId, int roleId)
        {
            DbContext.UserRoles.Add(new UbikUserRole() { UserId = userId, RoleId = roleId });
            return Task.FromResult(0);
        }
    }
}