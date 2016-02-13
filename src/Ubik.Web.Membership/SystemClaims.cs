using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ubik.Web.Membership
{
    public abstract class SystemClaims : IEnumerable<Claim>
    {
        public const string OperationClaimType = @"http://schemas.ubik/framework/identity/claims/operation";
        public const string ResourceClaimType = @"http://schemas.ubik/framework/identity/claims/resource";
        public const string OperationToResourceClaimType = @"http://schemas.ubik/framework/identity/claims/operation-to-resource";

        protected List<Claim> InternalList = new List<Claim>();

        protected readonly SystemRoles _systemRoles = new SystemRoles();

        public static Claim OperationToResource(string operation, string resource)
        {
            return new Claim(OperationToResourceClaimType, string.Format("{0}|{1}", operation, resource));
        }

        /// <summary>
        /// Call this method to populate the internal list
        /// </summary>
        public virtual void Initialize()
        {
            foreach (var actionClaim in new Operations())
            {
                foreach (var entityClaim in ResourceNames)
                {
                    var claim = OperationToResource(actionClaim, entityClaim);
                    InternalList.Add(claim);
                }
            }

            InternalList.AddRange(new SystemRoles());
        }

        public abstract string[] RoleNames { get; }

        protected abstract string[] ResourceNames { get; }

        public abstract string ResourceGroup { get; }

        public virtual bool ContainsClaim(Claim claim)
        {
            return InternalList.Any(x => x.Type == claim.Type && x.Value == claim.Value);
        }

        public abstract IEnumerable<Claim> Claims(string role);

        public IEnumerator<Claim> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        public class Operations : IEnumerable<string>
        {
            private readonly ICollection<string> _operationNames = new[]{
                    "Create" ,
                    "Delete" ,
                    "Edit"   ,
                    "Publish",
                    "Suspend",
                    "Approve",
                    "Submit" ,
                    "Schedule"
                };

            public const string Create = "Create";
            public const string Delete = "Delete";
            public const string Edit = "Edit";
            public const string Publish = "Publish";
            public const string Suspend = "Suspend";
            public const string Approve = "Approve";
            public const string Submit = "Submit";
            public const string Schedule = "Schedule";

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