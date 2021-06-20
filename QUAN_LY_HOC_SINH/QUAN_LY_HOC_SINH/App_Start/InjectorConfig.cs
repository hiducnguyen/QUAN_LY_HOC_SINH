using NHibernate;
using NHibernate.Cfg;
using Repositories.UnitOfWork;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace QUAN_LY_HOC_SINH
{
    public class InjectorConfig
    {
        public void RegisterInjector()
        {
            var container = new UnityContainer();

            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}