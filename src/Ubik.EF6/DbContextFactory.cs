using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Ubik.EF6
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IDictionary<Type, string> _connectionStrings;

        public DbContextFactory(IDictionary<Type, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            var type = typeof(TDbContext);
            if (!_connectionStrings.ContainsKey(typeof(TDbContext)))
                throw new Exception(string.Format("no connection string is registered for {0}", type));
            var connString = _connectionStrings[type];
            return (TDbContext)Activator.CreateInstance(typeof(TDbContext), connString);
        }
    }
}