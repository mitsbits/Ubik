using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubik.EF6.Contracts;

namespace Ubik.EF6
{
    public abstract class SequenceProviderDbContext : DbContext, ISequenceProvider
    {
        protected SequenceProviderDbContext()
            : base("cmsconnectionstring")
        {
        }

        protected SequenceProviderDbContext(string connString)
            : base(connString)
        {
        }

        public void Next(DbEntityEntry entry)
        {
            var entity = entry.Entity;
            var entityName = entity.GetType().Name;

            var seqName = SeqName(entity);
            var id = GetNextId(seqName);

            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var wKey =
                objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace)
                    .BaseEntitySets.First(meta => meta.ElementType.Name == entityName)
                    .ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
            entry.Property(wKey).CurrentValue = id;
        }

        private int GetNextId(string seqName)
        {
            var sqlText = string.Format("SELECT NEXT VALUE FOR {0};", seqName);
            var id = default(int);
            try
            {
                id = Database.SqlQuery<int>(sqlText).First();
            }
            catch (Exception ex)
            {
                if (ex.Source != ".Net SqlClient Data Provider" /* TODO: && ex.*/) throw;
                var sqlToCreate = string.Format("CREATE SEQUENCE {0} AS INTEGER MINVALUE 1 NO CYCLE; ", seqName);
                id = Database.SqlQuery<int>(sqlToCreate + sqlText).First();
            }
            return id;
        }

        private static string SeqName(object entity)
        {
            var seqName = entity.GetType().Name;
            var baseType = entity.GetType().BaseType;
            while (baseType?.GetInterface(typeof(ISequenceBase).Name, false) != null)
            {
                seqName = baseType.Name;
            }
            return seqName;
        }

        protected virtual void NextIds()
        {
            foreach (var entry in from entry in ChangeTracker.
                                  Entries().Where(e => e.State == EntityState.Added)
                                  let entity = entry.Entity as ISequenceBase
                                  where entity != null
                                  select entry)
            {
                Next(entry);
            }
        }

        #region Overrides

        public override int SaveChanges()
        {
            NextIds();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            NextIds();
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion Overrides
    }
}