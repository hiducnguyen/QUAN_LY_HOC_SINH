using Repositories;
using Repositories.UnitOfWork;
using Services;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace QUAN_LY_HOC_SINH
{
    public class InjectorConfig
    {
        public static void RegisterInjectors()
        {
            var container = new UnityContainer();

            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IGenericRepository, GenericRepository>();
            container.RegisterSingleton<IStudentRepository, StudentRepository>();
            container.RegisterSingleton<IRuleRepository, RuleRepository>();
            container.RegisterSingleton<IClassRepository, ClassRepository>();
            container.RegisterSingleton<ISubjectRepository, SubjectRepository>();

            container.RegisterSingleton<IStudentService, StudentService>();
            container.RegisterSingleton<IRuleService, RuleService>();
            container.RegisterSingleton<IClassService, ClassService>();
            container.RegisterSingleton<ISubjectService, SubjectService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}