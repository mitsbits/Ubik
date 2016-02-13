using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.EF6
{
    public abstract class BaseReadWriteRepository<T, TDbContext> : BaseReadRepository<T, TDbContext>,
        IWriteRepository<T>, IWriteAsyncRepository<T>
        where TDbContext :
        DbContext
        where T : class
    {
        private readonly List<string> _listOfErrors = new List<string>();

        public virtual void CreateOrUpdate(T entity)
        {
            // Define an ObjectStateEntry and EntityKey for the current object.
            //EntityKey key;
            var entitySet = this.GetEntitySetName(typeof(T));
            //var entitySet = ObjectSet.EntitySet.Name
            EntityKey key = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entitySet, entity);

            // check if object is valid before save.
            if (this.IsObjectValid(entity))
            {
                // Get the original item based on the entity key from the context
                // or from the database.
                object originalItem;
                if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
                {
                    // Call the ApplyCurrentValues method to apply changes.
                    DbContext.Entry(originalItem).CurrentValues.SetValues(entity);
                }
                else
                {
                    //add the new entity
                    Add(entity);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Join(", ", _listOfErrors));
            }
        }

        public virtual void Delete(T entity)
        {
            // Define an ObjectStateEntry and EntityKey for the current object.
            //EntityKey key;
            var entitySet = this.GetEntitySetName(typeof(T));
            EntityKey key = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entitySet, entity);

            // Get the original item based on the entity key from the context
            // or from the database.
            object originalItem;
            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                DbContext.Set<T>().Remove(entity);
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            Delete(DbContext.Set<T>().FirstOrDefault(predicate));
        }

        protected void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        protected virtual bool IsObjectValid(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // here we check the entity properties
            Type entityTypeClass = entity.GetType();
            Type metadataTypeClass = GetMetaDataClass(entity);
            if (metadataTypeClass == null)
            {
                AddErrorMessage(entityTypeClass.ToString() + " does not have a metadata attribure");
                return false;
            }

            PropertyInfo[] properties = metadataTypeClass.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var valProps = from PropertyInfo property in properties
                           where property.GetCustomAttributes(typeof(ValidationAttribute), true).Length > 0
                           select new
                           {
                               Property = property,
                               ValidationAttributes = property.GetCustomAttributes(typeof(ValidationAttribute), true)
                           };

            foreach (var item in valProps)
            {
                foreach (ValidationAttribute attribute in item.ValidationAttributes)
                {
                    if ((!attribute.IsValid(entityTypeClass.GetProperty(item.Property.Name).GetValue(entity, null))))
                    {
                        this.AddErrorMessage(item.Property.Name + ": " + attribute.ErrorMessage);
                    }
                }

                // Datatime validation
                if (item.Property.PropertyType == typeof(DateTime))
                {
                    var propValue = (DateTime)entityTypeClass.GetProperty(item.Property.Name).GetValue(entity, null);
                    if (propValue == DateTime.MinValue)
                    {
                        this.AddErrorMessage(item.Property.Name + ": " + string.Format("date is not valid"));
                    }
                }
            }

            return _listOfErrors.Count == 0;
        }

        protected void AddErrorMessage(string errorMessage)
        {
            _listOfErrors.Add(errorMessage);
        }

        protected static Type GetMetaDataClass(T entity)
        {
            var dnAttribute = typeof(T).GetCustomAttributes(
                typeof(MetadataTypeAttribute), true).FirstOrDefault() as MetadataTypeAttribute;
            if (dnAttribute != null)
            {
                return dnAttribute.MetadataClassType;
            }

            return null;
        }

        #region Async

        public Task<T> CreateAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            var entitySet = GetEntitySetName(typeof(T));
            var key = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entitySet, entity);
            object originalItem;
            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                // Call the ApplyCurrentValues method to apply changes.
                DbContext.Entry(originalItem).CurrentValues.SetValues(entity);
            }
            //DbContext.Entry<T>(entity).State = EntityState.Modified;
            //DbContext.Set<T>().Attach(entity);


            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var items = await DbContext.Set<T>().Where(predicate).ToListAsync();
            foreach (var item in items)
            {
                DbContext.Set<T>().Remove(item);
            }
        }

        #endregion Async
    }
}