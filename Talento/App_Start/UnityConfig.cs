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
using Talento.EmailManager;
using Talento.Core.Utilities;

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
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerRequestLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new PerRequestLifetimeManager());
            container.RegisterType<ApplicationUserManager>(new PerRequestLifetimeManager());
            container.RegisterType<ICustomPagingList, DashboardPagingHelper>(new PerRequestLifetimeManager());
            container.RegisterType<IPosition, PositionHelper>(new PerRequestLifetimeManager());
            container.RegisterType<IComment, CommentHelper>(new PerRequestLifetimeManager());
            container.RegisterType<IPositionLog, PositionLogHelper>(new PerRequestLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ICandidate, CandidateHelper>(new PerRequestLifetimeManager());
            container.RegisterType<ICustomUser, UserHelper>(new PerRequestLifetimeManager());
            container.RegisterType<IApplicationSetting, SettingsHelper>(new PerRequestLifetimeManager());
            container.RegisterType<IMessenger, Messenger>(new PerRequestLifetimeManager());
            container.RegisterType<IUtilityApplicationSettings, UtilityApplicationSettings>(new PerRequestLifetimeManager());
        }
    }
}
