using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OwinTestingWebApi.Controllers;

namespace OwinTestingWebApi.IoC.Installer
{
    public class ApiControllerInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                     .InSameNamespaceAs<ValuesApiController>()
                                     .LifestylePerWebRequest()
                                     .Configure(x => x.Named(x.Implementation.FullName)));
        }
    }
}