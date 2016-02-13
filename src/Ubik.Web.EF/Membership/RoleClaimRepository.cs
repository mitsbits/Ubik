using Mehdime.Entity;
using Ubik.EF6;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.SSO;

namespace Ubik.Web.EF.Membership
{
    public class RoleClaimRepository : ReadWriteRepository<UbikRoleClaim, AuthDbContext>, IRoleClaimRepository
    {
        public RoleClaimRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}