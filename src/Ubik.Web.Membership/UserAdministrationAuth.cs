using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.Membership
{
    public class UserAdministrationAuth : SystemClaims, IResourceAuthProvider
    {

        //todo: move to config
        public const string AuthCookieName = "YXV0aA==";

        private readonly Dictionary<string, List<Claim>> _rolesToClaims;
        private const string _adminRoleName = "UserAdmin";
        private const string _recourseGroup = "Users";

        public UserAdministrationAuth()
            : base()
        {
            _rolesToClaims = new Dictionary<string, List<Claim>>();
            Initialize();
        }

        public override sealed void Initialize()
        {
            base.Initialize();

            var userAdminClaims = new List<Claim>();
            foreach (var actionClaim in new Operations())
            {
                userAdminClaims.AddRange(ResourceNames.Select(entityClaim => OperationToResource(actionClaim, entityClaim)));
            }
            //userAdminClaims.Add(new Claim(SystemRoles.RoleClaimType, _adminRoleName));
            InternalList.AddRange(userAdminClaims);
            _rolesToClaims.Add(_adminRoleName, userAdminClaims);
            _rolesToClaims.Add(SystemRoles.AppAdmin, userAdminClaims);
            _rolesToClaims.Add(SystemRoles.SysAdmin, userAdminClaims);
        }

        public override string[] RoleNames
        {
            get
            {
                return new[] { _adminRoleName };
            }
        }

        public override IEnumerable<Claim> Claims(string role)
        {
            return (_rolesToClaims.ContainsKey(role)) ? new List<Claim>(_rolesToClaims[role]) : new List<Claim>();
        }

        protected override string[] ResourceNames
        {
            get { return new Resources().ToArray(); }
        }

        public override string ResourceGroup
        {
            get { return _recourseGroup; }
        }

        public class Resources : IEnumerable<string>
        {
            private readonly ICollection<string> _operationNames = new[]{
                "User" ,
                "Role" ,
                "Claim"   ,
            };

            public const string User = "User";
            public const string Role = "Role";
            public const string Claim = "Claim";

            public IEnumerator<string> GetEnumerator()
            {
                return _operationNames.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}