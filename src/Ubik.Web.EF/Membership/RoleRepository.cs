using Mehdime.Entity;
using Ubik.EF6;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.SSO;

namespace Ubik.Web.EF.Membership
{
    public class RoleRepository : ReadWriteRepository<UbikRole, AuthDbContext>, IRoleRepository
    {
        public RoleRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}