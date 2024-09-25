using Repository.Challenge;
using Repository.Common;
using Repository.CPanel;
using Repository.Customer;
using Repository.Email;
using Repository.HelperFunction;
using Repository.Product;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace CipherHunt
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IUtilityHelper, UtilityHelper>();
            container.RegisterType<ICommonRepository, CommonRepository>();
            container.RegisterType<ICalorieCalculator, CalorieCalculator>();
            container.RegisterType<IApplicationConfig, ApplicationConfig>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICpanelUserRepository, CpanelUserRepository>();
            container.RegisterType<IChallengeRepository, ChallengeRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IEmailRepository, EmailRepository>();
            container.RegisterType<ISMTPService, SMTPService>();
            container.RegisterType<IGraphRepository, GraphRepository>();
            container.RegisterType<IStatusRepository, StatusRepository>();
            return container;
        }
    }
}