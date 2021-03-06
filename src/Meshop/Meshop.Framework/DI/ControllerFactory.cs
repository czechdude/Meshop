﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Resources;
using System.Web.Routing;
using Castle.Windsor;

namespace Meshop.Framework.DI
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;

        public ControllerFactory(IWindsorContainer container)
        {
            _container = container;

        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404,"No controller found!");
            }
            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException("Wrong Controller Type!");
            } 
            return (IController)_container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
        }


    }
}