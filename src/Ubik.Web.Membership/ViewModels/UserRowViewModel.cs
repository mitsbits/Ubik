using System;
using System.Collections.Generic;

namespace Ubik.Web.Membership.ViewModels
{
    public class UserRowViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime? LockedOutEndUtc { get; set; }

        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}