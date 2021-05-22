using NHibernate;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Stat;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QUAN_LY_HOC_SINH.Session
{
    public class SessionFactory : ISessionFactory
    {
        public IStatistics Statistics => throw new NotImplementedException();

        public bool IsClosed => throw new NotImplementedException();

        public ICollection<string> DefinedFilterNames => throw new NotImplementedException();

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Evict(Type persistentClass)
        {
            throw new NotImplementedException();
        }

        public void Evict(Type persistentClass, object id)
        {
            throw new NotImplementedException();
        }

        public Task EvictAsync(Type persistentClass, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictAsync(Type persistentClass, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void EvictCollection(string roleName)
        {
            throw new NotImplementedException();
        }

        public void EvictCollection(string roleName, object id)
        {
            throw new NotImplementedException();
        }

        public Task EvictCollectionAsync(string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictCollectionAsync(string roleName, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void EvictEntity(string entityName)
        {
            throw new NotImplementedException();
        }

        public void EvictEntity(string entityName, object id)
        {
            throw new NotImplementedException();
        }

        public Task EvictEntityAsync(string entityName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictEntityAsync(string entityName, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void EvictQueries()
        {
            throw new NotImplementedException();
        }

        public void EvictQueries(string cacheRegion)
        {
            throw new NotImplementedException();
        }

        public Task EvictQueriesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictQueriesAsync(string cacheRegion, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IClassMetadata> GetAllClassMetadata()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
        {
            throw new NotImplementedException();
        }

        public IClassMetadata GetClassMetadata(Type persistentClass)
        {
            throw new NotImplementedException();
        }

        public IClassMetadata GetClassMetadata(string entityName)
        {
            throw new NotImplementedException();
        }

        public ICollectionMetadata GetCollectionMetadata(string roleName)
        {
            throw new NotImplementedException();
        }

        public ISession GetCurrentSession()
        {
            throw new NotImplementedException();
        }

        public FilterDefinition GetFilterDefinition(string filterName)
        {
            throw new NotImplementedException();
        }

        public ISession OpenSession(DbConnection connection)
        {
            throw new NotImplementedException();
        }

        public ISession OpenSession(IInterceptor sessionLocalInterceptor)
        {
            throw new NotImplementedException();
        }

        public ISession OpenSession(DbConnection conn, IInterceptor sessionLocalInterceptor)
        {
            throw new NotImplementedException();
        }

        public ISession OpenSession()
        {
            throw new NotImplementedException();
        }

        public IStatelessSession OpenStatelessSession()
        {
            throw new NotImplementedException();
        }

        public IStatelessSession OpenStatelessSession(DbConnection connection)
        {
            throw new NotImplementedException();
        }

        public ISessionBuilder WithOptions()
        {
            throw new NotImplementedException();
        }

        public IStatelessSessionBuilder WithStatelessOptions()
        {
            throw new NotImplementedException();
        }
    }
}