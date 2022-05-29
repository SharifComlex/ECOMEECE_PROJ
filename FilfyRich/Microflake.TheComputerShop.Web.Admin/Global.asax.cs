
using Microflake.TheComputerShop.Application.Categories;
using Microflake.TheComputerShop.Application.Products;

using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Utilities.Logger;
using Microflake.TheComputerShop.Utilities.Response;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Web.Mvc;
using SimpleInjector.Lifestyles;
using System.Reflection;
using System.Web.Routing;
using SimpleInjector.Integration.Web;
using Microsoft.AspNet.Identity;
using Microflake.TheComputerShop.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using Microflake.TheComputerShop.Application.Orders;
using Microflake.TheComputerShop.Application.DealOfTheWeeks;
using Microflake.TheComputerShop.Application.Whislists;
using Microflake.TheComputerShop.Application.Currencies;
using Microflake.TheComputerShop.Application.ContactUs;
using Microflake.TheComputerShop.Application.Subscribers;
using Microflake.TheComputerShop.Application.SubCategories;

namespace Microflake.TheComputerShop.Web.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var _container = new SimpleInjector.Container();

            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            //Application
            _container.Register<ApplicationDbContext>(() => new ApplicationDbContext(), Lifestyle.Singleton);
            _container.Register<IUserStore<ApplicationUser, string>>(() => new UserStore<ApplicationUser>(), Lifestyle.Singleton);
            _container.Register<UserManager<ApplicationUser, string>>(() => new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>()), Lifestyle.Singleton);

           _container.Register<IResponse, Response>(Lifestyle.Transient);
            _container.Register<ILogger, Logger>(Lifestyle.Transient);

            _container.Register<ICategoryService, CategoryService>(Lifestyle.Transient);
            _container.Register<ISubCategoryService, SubCategorieservice>(Lifestyle.Transient);

            _container.Register<IProductService, ProductService>(Lifestyle.Transient);
            _container.Register<ICurrencyService, CurrencyService>(Lifestyle.Transient);
            _container.Register<ISubscribersService, SubsribersService>(Lifestyle.Transient);

            
            _container.Register<IOrderService, OrderService>(Lifestyle.Transient);
            _container.Register<IDealOfTheWeekService, DealOfTheWeekService>(Lifestyle.Transient);
            _container.Register<IWhislistService, WhislistService>(Lifestyle.Transient);
            _container.Register<IContactUsService, ContactUsService>(Lifestyle.Transient);
            _container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            _container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container));
        }
    }
}
