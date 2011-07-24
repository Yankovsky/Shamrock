using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace Shamrock_WebSite
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private IUnityContainer _unityContainer;

        public UnityControllerFactory(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException();
            _unityContainer = unityContainer;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;
            if (!typeof(IController).IsAssignableFrom(controllerType))
                throw new ArgumentException(string.Format("Type requested is not a controller: {0}", controllerType.Name), "controllerType");

            return _unityContainer.Resolve(controllerType) as IController;
        }
    }
}