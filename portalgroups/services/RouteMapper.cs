using System.Web.Http;
using DotNetNuke.Web.Api;

namespace dnneurope.portalgroups.services
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("dnneurope.portalgroups", "default", "{Controller}/{Action}/{Id}",
                                         namespaces: new[] { "dnneurope.portalgroups.services" },
                                         defaults: new { Id = RouteParameter.Optional });
        }
    }
}