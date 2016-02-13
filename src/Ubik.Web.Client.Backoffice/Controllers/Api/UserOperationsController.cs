using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;
using Ubik.Domain.Core;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Client.Backoffice.Controllers.Api
{
    public class UserOperationsController : BackofficeOperationsController
    {
        private readonly IUserAdminstrationViewModelService _viewModelService;

        public UserOperationsController(IUserAdminstrationViewModelService viewModelService)
        {
            _viewModelService = viewModelService;
        }

        [Route("api/backoffice/users/claimsforroles/{id?}")]
        [HttpGet]
        public IActionResult ClaimsForRoles(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id)) return Ok(new List<RoleClaimViewModel>().ToArray());

            var names = id.Split(';');

            return Ok(_viewModelService.RoleModels()
                   .Where(x => names.Contains(x.Name))
                   .SelectMany(x => x.Claims.Select(c => c.Value))
                   .Distinct()
                   .ToArray());
        }


    }


}