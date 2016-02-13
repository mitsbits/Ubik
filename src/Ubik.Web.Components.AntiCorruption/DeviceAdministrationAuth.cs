using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ubik.Web.Membership;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.AntiCorruption
{
    public class DeviceAdministrationAuth : SystemClaims, IResourceAuthProvider
    {
        private readonly Dictionary<string, List<Claim>> _rolesToClaims;
        private const string _adminRoleName = SystemRoles.AppAdmin;
        private const string _recourseGroup = "Devices";

        public DeviceAdministrationAuth()
            : base()
        {
            _rolesToClaims = new Dictionary<string, List<Claim>>();
            Initialize();
        }

        public override sealed void Initialize()
        {
            base.Initialize();

            var deviceAdminClaims = new List<Claim>();
            foreach (var actionClaim in new Operations())
            {
                deviceAdminClaims.AddRange(ResourceNames.Select(entityClaim => OperationToResource(actionClaim, entityClaim)));
            }

            InternalList.AddRange(deviceAdminClaims);
            _rolesToClaims.Add(SystemRoles.AppAdmin, deviceAdminClaims);
            _rolesToClaims.Add(SystemRoles.SysAdmin, deviceAdminClaims);
        }

        public override string[] RoleNames
        {
            get
            {
                return
                    InternalList.Where(x => x.Type == SystemRoles.RoleClaimType)
                        .Select(x => x.Value)
                        .Distinct()
                        .ToArray();
            }
        }

        public override string ResourceGroup => _recourseGroup;

        public override IEnumerable<Claim> Claims(string role)
        {
            return (_rolesToClaims.ContainsKey(role)) ? new List<Claim>(_rolesToClaims[role]) : new List<Claim>();
        }

        protected override string[] ResourceNames => new Resources().ToArray();

        public class Resources : IEnumerable<string>
        {
            private readonly ICollection<string> _operationNames = new[]{
                "Device" ,
                "Section" ,
                "Module"   ,
            };

            public const string Device = "Device";
            public const string Section = "Section";
            public const string Module = "Module";

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