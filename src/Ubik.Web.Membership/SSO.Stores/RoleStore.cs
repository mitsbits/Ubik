using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Infra.DataManagement;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.SSO.Stores
{
    public class RoleStore : RoleStoreBase<UbikRole, AuthDbContext>, IRoleStoreWithCustomClaims
    {
        public RoleStore(
            IDbContextScopeFactory dbContextScopeFactory, IRoleRepository roleRepository, IRoleClaimRepository roleClaimRepository,
            IdentityErrorDescriber describer = null)
            : base(dbContextScopeFactory, roleRepository, roleClaimRepository, describer)
        { }



        public async Task ClearRoleClaims(UbikRole role, CancellationToken cancellationToken)
        {
            role.Claims.Clear();
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _roleClaimRepo.DeleteAsync(x => x.RoleId == role.Id);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<UbikRole>> Query(Expression<Func<UbikRole, bool>> predicate, params Expression<Func<UbikRole, dynamic>>[] paths)
        {
            using (var db = _dbContextScopeFactory.CreateReadOnly())
            {
                return await _roleRepo.FindAllAsync(predicate, new[] { OrderByInfo<UbikRole>.SortAscending<UbikRole>(x => x.Name) }, paths);
            }
        }

        public async Task SetRoleClaims(UbikRole role, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            role.Claims.Clear();
            var input = claims.Select(x => new UbikRoleClaim(x.Type, x.Value) { RoleId = role.Id });

            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _roleClaimRepo.DeleteAsync(x => x.RoleId == role.Id);
                foreach (var dbClaim in input)
                {
                  await  _roleClaimRepo.CreateAsync(dbClaim);
                }
                await db.SaveChangesAsync(cancellationToken);
                foreach (var dbClaim in input)
                {
                    role.Claims.Add(dbClaim);
                }
            }
        }
    }

    public abstract class RoleStoreBase<TRole, TContext> :
        //IQueryableRoleStore<TRole>,
        IRoleStore<TRole>,
        IRoleClaimStore<TRole>
        where TRole : UbikRole
        where TContext : AuthDbContext
    {
        public RoleStoreBase(
            IDbContextScopeFactory dbContextScopeFactory,
            IRoleRepository roleRepository,
            IRoleClaimRepository roleClaimRepository,
            IdentityErrorDescriber describer = null)
        {

            _dbContextScopeFactory = dbContextScopeFactory;
            ErrorDescriber = describer ?? new IdentityErrorDescriber();
            _roleRepo = roleRepository;
            _roleClaimRepo = roleClaimRepository;
        }

        private bool _disposed;

        protected readonly IDbContextScopeFactory _dbContextScopeFactory;

        protected readonly IRoleRepository _roleRepo;

        protected readonly IRoleClaimRepository _roleClaimRepo;


        /// <summary>
        /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <value>
        /// True if changes should be automatically persisted, otherwise false.
        /// </value>
        public bool AutoSaveChanges { get { return false; } }



        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _roleRepo.CreateAsync(role);
                await db.SaveChangesAsync(cancellationToken);
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            try
            {
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
                {
                    await _roleRepo.UpdateAsync(role);
                    await db.SaveChangesAsync(cancellationToken);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            try
            {
                using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await _roleRepo.DeleteAsync(x => x.Id == role.Id);
                    await db.SaveChangesAsync(cancellationToken);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(ConvertIdToString(role.Id));
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to a strongly typed key object.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An instance of <typeparamref name="int"/> representing the provided <paramref name="id"/>.</returns>
        public virtual int ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(int);
            }
            return (int)TypeDescriptor.GetConverter(typeof(int)).ConvertFromInvariantString(id);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
        public virtual string ConvertIdToString(int id)
        {
            if (id.Equals(default(int)))
            {
                return null;
            }
            return id.ToString();
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual async Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return (await _roleRepo.GetAsync(x => x.Id == roleId, x => x.Claims)) as TRole;
            }

        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return (await _roleRepo.GetAsync(r => r.NormalizedName == normalizedName)) as TRole;
            }
        }

        /// <summary>
        /// Get a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        /// <summary>
        /// Set a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the stores
        /// </summary>
        public void Dispose()
        {

            _disposed = true;
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var roleId = role.Id;
                var roleWithClaimsFromDb = await _roleRepo.GetAsync(x => x.Id == roleId, (x) => x.Claims);
                return roleWithClaimsFromDb.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();
            }

        }

        /// <summary>
        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add to the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            using (var db = _dbContextScopeFactory.Create(DbContextScopeOption.JoinExisting))
            {
                if (!role.Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
                    _roleClaimRepo.CreateAsync(new UbikRoleClaim(claim.Type, claim.Value) { RoleId = role.Id });
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            using (var db = _dbContextScopeFactory.Create(DbContextScopeOption.JoinExisting))
            {
                Expression<Func<UbikRoleClaim, bool>> predicate = x => x.RoleId == role.Id && x.ClaimType == claim.Type && x.ClaimValue == claim.Value;


                var claimsToRemove = role.Claims.Where(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value).ToList();
                foreach (var claimToRemove in claimsToRemove)
                {
                    _roleClaimRepo.Delete(claimToRemove);
                    role.Claims.Remove(claimToRemove);
                }
            }
            return Task.FromResult(false);
        }

    }
}