[assembly: WebActivator.PreApplicationStartMethod(typeof(ConferenceApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(ConferenceApp.App_Start.NinjectWebCommon), "Stop")]

namespace ConferenceApp.App_Start
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Web;
    using Core.Security;
    using Core.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Model;
    using Ninject;
    using Ninject.Web.Common;
    using Core.Services.Sessions;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var config = ConfigurationManager.ConnectionStrings["ConferenceContext"].ConnectionString;

            kernel.Bind<ConferenceContext>().ToSelf().InRequestScope().WithConstructorArgument("connectionString", config); 
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<ICryptographyService>().To<CryptographyService>().InRequestScope();
            kernel.Bind<IEmailService>().To<EmailService>().InRequestScope();
            kernel.Bind<ISessionService>().To<SessionService>().InRequestScope();
            kernel.Bind<IImageService>().To<ImageService>().InRequestScope();
        }        
    }
}
