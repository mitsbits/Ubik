using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Ubik.Web.SSO
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class AuthDbContext : IdentityDbContext<UbikUser, UbikRole> {
        public AuthDbContext(string connString)
            : base(connString)
        {
        }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public abstract class IdentityDbContext<TUser> : IdentityDbContext<TUser, UbikRole> where TUser : UbikUser { }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public abstract class IdentityDbContext<TUser, TRole> : DbContext
        where TUser : UbikUser
        where TRole : UbikRole
    {
        public IdentityDbContext()
            : base("authconnectionstring")
        {
        }

        public IdentityDbContext(string connString)
            : base(connString)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<TUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public DbSet<UbikRoleClaim> RoleClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<UbikUserClaim> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<UbikUserLogin> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
        /// </summary>
        public DbSet<UbikUserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public DbSet<TRole> Roles { get; set; }



        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Conventions.Remove<PluralizingTableNameConvention>();
            builder.Configurations.Add(new UserConfig());
            builder.Configurations.Add(new RoleConfig());
            builder.Configurations.Add(new IdentityRoleClaimConfig());
            builder.Configurations.Add(new IdentityUserClaimConfig());
            builder.Configurations.Add(new IdentityUserRoleConfig());
            builder.Configurations.Add(new IdentityUserLoginConfig());
            base.OnModelCreating(builder);
        }

        internal class UserConfig : EntityTypeConfiguration<UbikUser>
        {
            public UserConfig()
            {
                ToTable("UbikUsers");
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                HasKey(u => u.Id);
                Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                Property(u => u.UserName).HasMaxLength(256);
                Property(u => u.NormalizedUserName).HasMaxLength(256);
                Property(u => u.Email).HasMaxLength(256);
                Property(u => u.NormalizedEmail).HasMaxLength(256);
                HasMany(u => u.Claims).WithRequired(x => x.User).HasForeignKey(x => x.UserId);
                HasMany(u => u.Logins).WithRequired(x => x.User).HasForeignKey(x => x.UserId);
                HasMany(u => u.Roles).WithRequired(x => x.User).HasForeignKey(x => x.UserId);

            }
        }

        internal class RoleConfig : EntityTypeConfiguration<UbikRole>
        {
            public RoleConfig()
            {
                ToTable("UbikRoles");
                HasKey(r => r.Id);
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                Property(u => u.Name).HasMaxLength(256);
                Property(u => u.NormalizedName).HasMaxLength(256);
                HasMany(r => r.Users).WithRequired(x => x.Role).HasForeignKey(x => x.RoleId);
                HasMany(r => r.Claims);
            }
        }

        internal class IdentityUserClaimConfig : EntityTypeConfiguration<UbikUserClaim>
        {
            public IdentityUserClaimConfig()
            {
                HasKey(uc => uc.Id);
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                ToTable("UbikUserClaims");
            }
        }

        internal class IdentityRoleClaimConfig : EntityTypeConfiguration<UbikRoleClaim>
        {
            public IdentityRoleClaimConfig()
            {
                HasKey(rc => rc.Id);         
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                ToTable("UbikRoleClaims");
            }
        }

        internal class IdentityUserRoleConfig : EntityTypeConfiguration<UbikUserRole>
        {
            public IdentityUserRoleConfig()
            {
                HasKey(r => new { r.UserId, r.RoleId });
                ToTable("UbikUserRoles");
            }
        }

        internal class IdentityUserLoginConfig : EntityTypeConfiguration<UbikUserLogin>
        {
            public IdentityUserLoginConfig()
            {
                HasKey(l => new { l.LoginProvider, l.ProviderKey });
                ToTable("UbikUserLogins");
            }
        }
    }
}