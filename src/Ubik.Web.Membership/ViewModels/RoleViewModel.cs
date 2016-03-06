using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra.Contracts;
using Ubik.Web.Membership.Events;
using Ubik.Web.Membership.Managers;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.ViewModels
{
    public class RoleSaveModel
    {
        public int RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<RoleClaimViewModel> Claims { get; set; }
    }

    public class RoleViewModel : RoleSaveModel, ISelectable
    {
        public RoleClaimViewModel[] AvailableClaims { get; set; }
        public bool IsSytemRole { get; set; }
        public bool IsPersisted { get; set; }
        public bool IsSelected { get; set; }
    }

    public class RoleViewModelBuilder : IViewModelBuilder<UbikRole, RoleViewModel>
    {
        private readonly IEnumerable<RoleViewModel> _roleViewModels;

        public RoleViewModelBuilder(IEnumerable<RoleViewModel> roleViewModels)
        {
            _roleViewModels = roleViewModels;
        }

        public RoleViewModel CreateFrom(UbikRole entity)
        {
            if (entity == null) return new RoleViewModel();
            var viewModel = new RoleViewModel
            {
                RoleId = entity.Id,
                Name = entity.Name,
                Claims = entity.Claims.Select(
                    x => new RoleClaimViewModel() { Type = x.ClaimType, Value = x.ClaimValue }).ToList()
            };

            return viewModel;
        }

        public void Rebuild(RoleViewModel model)
        {
            //if (model.RoleId == default(int)) return;
            model.AvailableClaims = new List<RoleClaimViewModel>(_roleViewModels.First().AvailableClaims).ToArray();

            foreach (var roleClaimRowViewModel in model.AvailableClaims)
            {
                roleClaimRowViewModel.IsSelected =
                    model.Claims.Any(x => x.Type == roleClaimRowViewModel.Type
                        && x.Value == roleClaimRowViewModel.Value);
            }
        }
    }

    public class RoleViewModelCommand : IViewModelCommand<RoleSaveModel>
    {
        private readonly UbikRoleManager<UbikRole> _roleManager;
        private readonly IEventBus _eventBus;
        public RoleViewModelCommand(UbikRoleManager<UbikRole> roleManager, IEventBus eventBus)
        {
            _roleManager = roleManager;
            _eventBus = eventBus;
        }

        public async Task Execute(RoleSaveModel model)
        {
            var results = new List<IdentityResult>();
            var dbRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
            if (dbRole == null)
            {
                dbRole = new UbikRole() { Id = model.RoleId, Name = model.Name };
                results.Add(await _roleManager.CreateAsync(dbRole));
                await _eventBus.Publish(new RoleRowStateChanged(dbRole.Id, dbRole.Name, Infra.DataManagement.RowState.Added));
            }
            else
            {
                dbRole.Name = model.Name;

                await _roleManager.ClearClaims(dbRole);
                await _eventBus.Publish(new RoleRowStateChanged(dbRole.Id, dbRole.Name, Infra.DataManagement.RowState.Modified));

            }

            await _roleManager.SetClaims(dbRole, model.Claims.Select(x => new Claim(x.Type, x.Value)));

            results.Add(await _roleManager.UpdateAsync(dbRole));
            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }
    }

    public class CopyRoleViewModel : RoleViewModel
    {
        [Required]
        public string Target { get; set; }

        public CopyRoleViewModel()
        {
        }

        public CopyRoleViewModel(RoleViewModel source)
        {
            AvailableClaims = source.AvailableClaims;
            Claims = source.Claims;
            IsPersisted = source.IsPersisted;
            IsSytemRole = source.IsSytemRole;
            Name = source.Name;
            RoleId = source.RoleId;
            IsSelected = source.IsSelected;
        }
    }
}