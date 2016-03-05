using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Client.Backoffice.Filters;
using Ubik.Web.Membership;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class UserAdministrationController : BackofficeController
    {
        private readonly IUserAdminstrationService _userService;
        private readonly IUserAdminstrationViewModelService _viewModelsService;

        private const int daysToLock = 365 * 200;

        public UserAdministrationController(IErrorLogManager errorLogManager, IEventBus eventDispatcher, IUserAdminstrationService userService, IUserAdminstrationViewModelService viewModelsService) : base(errorLogManager, eventDispatcher)
        {
            _userService = userService;
            _viewModelsService = viewModelsService;
        }

        protected IUserAdminstrationService UserService
        {
            get { return _userService; }
        }

        protected IUserAdminstrationViewModelService ViewModelsService
        {
            get { return _viewModelsService; }
        }

        public ActionResult Index()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage memberships" });
            return View();
        }

        #region Users

        public ActionResult Users(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GetAllUsers() : GetOneUserById(id);
        }


        [ActionName("new-user")]
        [AuthOpToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.User)]
        public ActionResult NewUser()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can create a new user" });
            var model = ViewModelsService.NewUserModel();
            return View("NewUser", model);
        }

        public async Task<ActionResult> CreateUser(NewUserViewModel model)
        {
            try
            {
                //model.Email = model.UserName;
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return View("Users", model);
                }
                model.Roles = model.AvailableRoles.Where(x => x.IsSelected).ToArray();
                await ViewModelsService.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("User '{0}' created!", model.UserName));
                return RedirectToAction("Users", "UserAdministration", new { id = model.UserId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Users", "UserAdministration");
            }
        }

        public async Task<ActionResult> UpdateUser(UserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return View("NewUser", model);
                }
                model.Roles = model.AvailableRoles.Where(x => x.IsSelected).ToArray();
                await ViewModelsService.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("User '{0}' updated!", model.UserName));
                return RedirectToAction("Users", "UserAdministration", new { id = model.UserId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Users", "UserAdministration");
            }
        }

        public async Task<ActionResult> LockUser(string id, string redirectUrl)
        {
            try
            {
                await _userService.LockUser(id, daysToLock);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, "User locked!");
                if (string.IsNullOrWhiteSpace(redirectUrl))
                    return RedirectToAction("Users", "UserAdministration");
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Users", "UserAdministration");
            }
        }

        public async Task<ActionResult> UnlockUser(string id, string redirectUrl)
        {
            try
            {
                await _userService.UnockUser(id);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, "User unlocked!");
                if (string.IsNullOrWhiteSpace(redirectUrl))
                    return RedirectToAction("Users", "UserAdministration");
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Users", "UserAdministration");
            }
        }

        [HttpPost]
        public async Task<ActionResult> LockUser(UserLockedStateViewModel model)
        {
            await _userService.LockUser(model.UserId, daysToLock);
            return Redirect(model.RedirectURL);
        }

        [HttpPost]
        public async Task<ActionResult> UnlockUser(UserLockedStateViewModel model)
        {
            await _userService.UnockUser(model.UserId);
            return Redirect(model.RedirectURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangUserPassword(UserChangPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                    await _userService.SetPassword(model.UserId, model.NewPassword);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, "New password set!");
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
            }
            return Redirect(model.RedirectURL);
        }

        private ActionResult GetOneUserById(string id)
        {
            try
            {
                var model = _viewModelsService.UserModel(id);
                SetContentPage(new BackofficeContent()
                {
                    Title = "Manage: " + model.UserName,
                    Subtitle = "here you can manage this user"
                });
                return View(model);
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Users");
            }
        }

        private ActionResult GetNoUser()
        {
            var content = new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage users" };
            SetContentPage(content);
            return View(new List<UserRowViewModel>());
        }

        private ActionResult GetAllUsers()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage users" });
            try
            {
                return View(_viewModelsService.UserModels());
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return Redirect(Url.Content("~/backoffice"));
            }
        }

        #endregion Users

        #region Roles

        public ActionResult Roles(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GetAllRoles() : GetOneRoleByName(id);
        }

        [ActionName("new-role")]
        //[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.Role)]
        public ActionResult NewRole()
        {
            SetContentPage(new BackofficeContent() { Title = "Role Administration", Subtitle = "here you can create a new role" });
            var model = ViewModelsService.RoleModel(string.Empty);
            return View("NewRole", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> UpdateRole(RoleViewModel model)
        {
            var o = this.Request.Form;
            if (!ModelState.IsValid) return View("NewRole", model);
            var c = _viewModelsService.SystemRoleViewModels.First().AvailableClaims;
            var f = o["AvailableClaims"];
            var currentClaims = model.AvailableClaims.Where(x => x.IsSelected).ToList();
            model.Claims = new List<RoleClaimViewModel>(currentClaims);
            await ViewModelsService.Execute(model);
            return RedirectToAction("Roles", "UserAdministration", new { id = model.Name });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CopyRole(CopyRoleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return View("~/Areas/Backoffice/Views/UserAdministration/Roles.cshtml", model);
                }
                await UserService.CopyRole(model.Name, model.Target);
                return RedirectToAction("Roles", "UserAdministration", new { id = model.Target });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Roles", "UserAdministration", new { id = "" });
            }
        }

        public async Task<ActionResult> DeleteRole(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                try
                {
                    await UserService.DeleteRole(id);
                    AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Role {0} deleted!", id));
                }
                catch (Exception ex)
                {
                    AddRedirectMessage(ex);
                }
            }
            return RedirectToAction("Roles", "UserAdministration");
        }

        private ActionResult GetOneRoleByName(string id)
        {
            var model = ViewModelsService.RoleByNameModel(id);
            var content = new BackofficeContent { Title = model.Name, Subtitle = "role management" };
            SetContentPage(content);
            return View(model);
        }

        private ActionResult GetAllRoles()
        {
            var content = new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage roles" };
            SetContentPage(content);
            return View(ViewModelsService.RoleModels());
        }

        #endregion Roles
    }
}