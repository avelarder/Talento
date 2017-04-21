using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Talento.Controllers;
using Talento.Models;
using Talento.Core;
using Talento.Entities;
using Talento.Core.Data;
using Talento.Core.Helpers;

namespace Talento.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<UserManager<ApplicationUser>>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ICustomPagingList, DashboardPagingHelper>();
            container.RegisterType<IPosition, PositionHelper>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IPosition, PositionHelper>();
            container.RegisterType<ICandidate, CandidateHelper>();
            container.RegisterType<ITag, TagHelper>();
            container.RegisterType<ICustomUser, UserHelper>();
            container.RegisterType<IApplicationSetting, SettingsHelper>();
        }
    }
}
