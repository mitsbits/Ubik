using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.Managers;
using Ubik.Web.Membership.ViewModels;
using Ubik.Web.SSO;

namespace Ubik.Web.Membership.Services
{
    public class UserAdminstrationService : IUserAdminstrationService, IUserAdminstrationViewModelService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        private readonly UbikUserManager<UbikUser> _userManager;
        private readonly UbikRoleManager<UbikRole> _roleManager;

        private readonly IViewModelBuilder<UbikUser, UserViewModel> _userBuilder;
        private readonly IViewModelBuilder<UbikUser, NewUserViewModel> _newUserBuilder;
        private readonly IViewModelBuilder<UbikRole, RoleViewModel> _roleBuilder;

        private readonly IViewModelCommand<RoleSaveModel> _roleCommand;
        private readonly IViewModelCommand<NewUserSaveModel> _newUserCommand;
        private readonly IViewModelCommand<UserSaveModel> _userCommand;

        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        private readonly IEnumerable<IResourceAuthProvider> _authProviders;

        private readonly ICacheProvider _cache;



        public UserAdminstrationService(IUserRepository userRepository, IRoleRepository roleRepository, UbikUserManager<UbikUser> userManager, UbikRoleManager<UbikRole> roleManager, IViewModelCommand<RoleSaveModel> roleCommand, IViewModelCommand<NewUserSaveModel> newUserCommand, IDbContextScopeFactory dbContextScopeFactory, IEnumerable<IResourceAuthProvider> authProviders, ICacheProvider cache, IViewModelCommand<UserSaveModel> userCommand)
        {
            _userRepo = userRepository;
            _roleRepo = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleCommand = roleCommand;
            _newUserCommand = newUserCommand;
            _dbContextScopeFactory = dbContextScopeFactory;
            _authProviders = authProviders;
            _cache = cache;
            _userCommand = userCommand;

            _userBuilder = new UserViewModelBuilder(RoleViewModels);
            _newUserBuilder = new NewUserViewModelBuilder(RoleViewModels);
            _roleBuilder = new RoleViewModelBuilder(RoleViewModels);
        }

        public async Task CopyRole(string source, string target)
        {
            var sourceIsSytemRole = SystemRoleViewModels.Any(x => x.Name == source);
            UbikRole copy;
            if (sourceIsSytemRole)
            {
                var sourceViewModel = RoleModels().First(x => x.Name == source);
                copy = new UbikRole() { Name = target };
                foreach (var roleClaimRowViewModel in sourceViewModel.Claims)
                {
                    copy.Claims.Add(new UbikRoleClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                }
            }
            else
            {
                var original = await _roleManager.FindByNameAsync(source);
                if (original == null) throw new ApplicationException("source role not found");
                copy = new UbikRole(target);
                foreach (var applicationClaim in original.Claims)
                {
                    copy.Claims.Add(applicationClaim);
                }
            }
            var result = await _roleManager.CreateAsync(copy);
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
            _cache.RemoveItem(MembershipConstants.RoleViewModelsCacheKey);
        }

        public async Task DeleteRole(string name)
        {
            var sourceIsSytemRole = SystemRoleViewModels.Any(x => x.Name == name);
            if (sourceIsSytemRole) throw new ApplicationException("can not delete a system role");
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null) throw new ApplicationException("role to delete not found");
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
            _cache.RemoveItem(MembershipConstants.RoleViewModelsCacheKey);
        }

        public async Task LockUser(string userId, int days)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var results = new List<IdentityResult>
            {
              await  _userManager.SetLockoutEnabledAsync(user, true),
              await   _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(days)),
              await  _userManager.ResetAccessFailedCountAsync(user)
            };
            if (!results.All(x => x.Succeeded)) throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        public async Task UnockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var results = new List<IdentityResult>
            {
              await _userManager.SetLockoutEnabledAsync(user, false),
              await _userManager.ResetAccessFailedCountAsync(user)
            };
            if (!results.All(x => x.Succeeded)) throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        public async Task SetPassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
        }

        public IEnumerable<UbikUser> Find(Expression<Func<UbikUser, bool>> predicate, int pageNumber, int pageSize, out int totalRecords)
        {
            return _userRepo.Find(predicate, user => user.UserName, false, pageNumber, pageSize, out totalRecords);
        }

        public IEnumerable<UbikRole> Find(Expression<Func<UbikRole, bool>> predicate, int pageNumber, int pageSize, out int totalRecords)
        {
            return _roleRepo.Find(predicate, role => (role != null) ? role.Name : string.Empty, false, pageNumber, pageSize, out totalRecords);
        }

        public UbikUser CreateUser(UbikUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded) throw new ApplicationException("you should handle this looser");
            return user;
        }

        public async Task<IdentityResult> SetRoles(UbikUser user, string[] newRoles)
        {
            user.Roles.Clear();
            return await _userManager.AddToRolesAsync(user, newRoles);
        }

        public UserViewModel UserModel(string id)
        {
            UbikUser entity;
            entity = string.IsNullOrWhiteSpace(id) ? new UbikUser() : _userManager.FindByIdAsync(id).Result;
            var model = _userBuilder.CreateFrom(entity);
            _userBuilder.Rebuild(model);
            return model;
        }

        public NewUserViewModel NewUserModel()
        {
            var entity = new UbikUser();
            var model = _newUserBuilder.CreateFrom(entity);
            _newUserBuilder.Rebuild(model);
            return model;
        }

        public RoleViewModel RoleModel(string id)
        {
            UbikRole roleEntity;
            if (string.IsNullOrWhiteSpace(id)) //creates a new blank transient entity
            {
                roleEntity = new UbikRole();
            }
            else
            {
                using (_dbContextScopeFactory.CreateReadOnly())
                {
                    Expression<Func<UbikRole, bool>> predicate = role => role.Id == Convert.ToInt32(id);
                    roleEntity = _roleRepo.GetAsync(predicate, x => x.Claims).Result;
                }
            }

            var model = _roleBuilder.CreateFrom(roleEntity);
            _roleBuilder.Rebuild(model);
            return model;
        }

        public RoleViewModel RoleByNameModel(string name)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                RoleViewModel model;
                Expression<Func<UbikRole, bool>> predicate = role => role.Name == name;
                var roleEntity = _roleRepo.GetAsync(x => x.Name == name, x => x.Claims).Result;
                if (roleEntity == null && SystemRoleViewModels.Any(x => x.Name == name))
                {
                    model = SystemRoleViewModels.First(x => x.Name == name);
                }
                else
                {
                    if (
                        SystemRoleViewModels.Any(
                            x => x.Name.Equals(roleEntity.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var systemClaims =
                            SystemRoleViewModels.Single(
                                x => x.Name.Equals(roleEntity.Name, StringComparison.InvariantCultureIgnoreCase)).Claims;
                        foreach (var roleClaimRowViewModel in systemClaims.Where(roleClaimRowViewModel => !roleEntity.Claims.Any(x =>
                           x.ClaimType == roleClaimRowViewModel.Type && x.ClaimValue == roleClaimRowViewModel.Value)))
                        {
                            roleEntity.Claims.Add(new UbikRoleClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                        }
                    }
                    model = _roleBuilder.CreateFrom(roleEntity);
                }
                _roleBuilder.Rebuild(model);
                model.IsSytemRole = SystemRoleViewModels.Any(x => x.Name == name);
                return model;
            }
        }

        public IEnumerable<UserRowViewModel> UserModels()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var dbCollection = _userRepo.FindAllAsync(x => true,
                new[] { OrderByInfo<UbikUser>.SortAscending<UbikUser>(p => p.NormalizedUserName) },
                x => x.Roles, x => x.Claims).Result;
                return dbCollection.Select(appUser => new UserRowViewModel
                {
                    UserId = appUser.Id.ToString(),
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    IsLockedOut = appUser.LockoutEnabled,
                    LockedOutEndUtc = appUser.LockoutEnd.HasValue ? appUser.LockoutEnd.Value.UtcDateTime : default(DateTime?),
                    Roles = appUser.Roles.Select(
                        role => RoleViewModels.FirstOrDefault(x => x.RoleId == role.RoleId)
                           ).ToList()
                }).ToList();
            }
        }

        public IEnumerable<RoleViewModel> RoleModels()
        {
            return RoleViewModels;
        }

        public async Task Execute(NewUserSaveModel model)
        {
            await _newUserCommand.Execute(model);
            //TODO: publish message for new role
        }

        public async Task Execute(UserSaveModel model)
        {
            await _userCommand.Execute(model);

            //TODO: publish message for new role
        }

        private List<RoleViewModel> _systemRoleViewModels;

        public virtual IEnumerable<RoleViewModel> SystemRoleViewModels
        {
            get
            {
                if (_systemRoleViewModels != null) return _systemRoleViewModels;
                _systemRoleViewModels = new List<RoleViewModel>(_authProviders.RoleModels());
                return _systemRoleViewModels;
            }
        }

        private IEnumerable<RoleViewModel> RoleViewModels
        {
            get
            {
                var inCache = _cache.GetItem(MembershipConstants.RoleViewModelsCacheKey) as IEnumerable<RoleViewModel>;
                if (inCache != null) return inCache;

                _cache.SetItem(MembershipConstants.RoleViewModelsCacheKey, new List<RoleViewModel>(_authProviders.RoleModelsCheckDB(_roleManager)));
                return _cache.GetItem(MembershipConstants.RoleViewModelsCacheKey) as IEnumerable<RoleViewModel>;
            }
        }

        public async Task Execute(RoleSaveModel model)
        {
            await _roleCommand.Execute(model);

            //_cache.RemoveItem(MembershipConstants.RoleViewModelsCacheKey); // force cache to invalidate
            //TODO: publish message for new role
        }
    }
}