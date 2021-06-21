using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Reflection;

namespace Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;
        private ISession _session;

        public ISession Session { get => _session; }

        public UnitOfWork()
        {
            //  Configure mapping
            var config = new Configuration().Configure();
            var mapping = GetMappings();
            config.AddDeserializedMapping(mapping, null);

            //  Create session factory
            _sessionFactory = config.BuildSessionFactory();
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }


        public IUnitOfWork Start()
        {
            if (_session == null || _session.IsOpen == false)
            {
                _session = _sessionFactory.OpenSession();
            }
            if (_transaction == null || _transaction.IsActive == false)
            {
                _transaction = _session.BeginTransaction();
            }
            return this;
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            if (_transaction != null && _transaction.IsActive == true)
            {
                _transaction.Dispose();
            }
            if (_session != null && _session.IsOpen)
            {
                _session.Dispose();
            }
        }
    }
}
