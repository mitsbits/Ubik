using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Web.Components.DTO;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Devices
{
    public class BasePartialModuleViewModel
    {
        private readonly ICollection<Tiding> _parameters;

        public BasePartialModuleViewModel() { _parameters = new HashSet<Tiding>(); }

        public string ModuleType { get; set; }
        public string FriendlyName { get; set; }
        public string Summary { get; set; }

        public string ModuleGroup { get; set; }

        public string FullName { get; set; }

        public ICollection<Tiding> Parameters { get { return _parameters; } }
    }
}
