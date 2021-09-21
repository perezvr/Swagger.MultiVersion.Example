using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Swagger.MultiVersion.Example
{
    public static class RoutePrefixExtension
    {
        public static void UseRoutePrefix(this MvcOptions opts, string prefix)
            => opts.UseRoutePrefix(new RouteAttribute(prefix));

        public static void UseRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
            => opts.Conventions.Add(new RoutePrefix(routeAttribute));
    }

    public class RoutePrefix : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefix(IRouteTemplateProvider route)
            => _routePrefix = new AttributeRouteModel(route);

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
                selector.AttributeRouteModel = selector.AttributeRouteModel != null
                    ? AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel)
                    : _routePrefix;
        }
    }
}