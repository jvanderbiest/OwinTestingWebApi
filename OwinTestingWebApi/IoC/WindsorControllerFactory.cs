using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace OwinTestingWebApi.IoC
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <returns>
        /// The controller instance.
        /// </returns>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param><param name="controllerType">The type of the controller.</param><exception cref="T:System.Web.HttpException"><paramref name="controllerType"/> is null.</exception><exception cref="T:System.ArgumentException"><paramref name="controllerType"/> cannot be assigned.</exception><exception cref="T:System.InvalidOperationException">An instance of <paramref name="controllerType"/> cannot be created.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            if (_container.Kernel.HasComponent(controllerType))
                return (IController)_container.Resolve(controllerType);
            
            return base.GetControllerInstance(requestContext, controllerType);
        }

        /// <summary>
        /// Releases the specified controller.
        /// </summary>
        /// <param name="controller">The controller to release.</param>
        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
            base.ReleaseController(controller);
        }
    }
}