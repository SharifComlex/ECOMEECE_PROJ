
using Microflake.Core.Application.Categories;
using Microflake.Core.Application.Products;

using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Web.Mvc;
using SimpleInjector.Lifestyles;
using System.Reflection;
using System.Web.Routing;
using SimpleInjector.Integration.Web;
using Microsoft.AspNet.Identity;
using Microflake.Core.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using Microflake.Core.Application.Orders;
using Microflake.Core.Application.DealOfTheWeeks;
using Microflake.Core.Application.Whislists;
using Microflake.Core.Application.Currencies;
using Microflake.Core.Application.ContactUs;
using Microflake.Core.Application.Subscribers;
using Microflake.Core.Application.SubCategories;
using Microflake.Core.Application.CustomCategories;
using Microflake.Core.Application.CustomColors;
using Microflake.Core.Application.CustomItems;
using Microflake.Core.Application.CustomVariations;
using Microflake.Core.Application.CustomOrders;

namespace Microflake.Web
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

            _container.Register<ICustomCategoryService, CustomCategoryService>(Lifestyle.Transient);
            _container.Register<ICustomColorService, CustomColorService>(Lifestyle.Transient);
            _container.Register<ICustomItemService, CustomItemService>(Lifestyle.Transient);
            _container.Register<ICustomVariationService, CustomVariationService>(Lifestyle.Transient);

            _container.Register<ICustomOrderService, CustomOrderService>(Lifestyle.Transient);
            _container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            _container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container));
        }
    }
}
