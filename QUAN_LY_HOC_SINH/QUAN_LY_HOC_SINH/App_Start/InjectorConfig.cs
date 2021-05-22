using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace QUAN_LY_HOC_SINH
{
    public class InjectorConfig
    {
        public void RegisterInjector()
        {
            var container = new UnityContainer();

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            container.RegisterInstance<ISessionFactory>(sessionFactory);
        }

    }
}