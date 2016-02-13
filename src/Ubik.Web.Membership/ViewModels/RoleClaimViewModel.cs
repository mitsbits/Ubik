using Ubik.Infra.Contracts;

namespace Ubik.Web.Membership.ViewModels
{
    public class RoleClaimViewModel : ISelectable
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public string[] ResourceGroups { get; set; }
    }
}