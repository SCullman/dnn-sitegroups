using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using dnneurope.portalgroups.contract;
using dnneurope.portalgroups.logic;

namespace dnneurope.portalgroups.services
{
    [RequireHost]
    public class PortalGroupController : DnnApiController
    {
        IManagePortalGroups GroupManager {get
        {
           return PortalGroups.Instance;
        }}
        
        public HttpResponseMessage Get()
        {
            var model = GroupManager.Get();
            return Request.CreateResponse(HttpStatusCode.OK,model);
        }

        [HttpPost] 
        public HttpResponseMessage Save(PortalGroupInfo portalGroup)
        {
            var id = GroupManager.Save(portalGroup);
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }
        
        [HttpGet] 
        public HttpResponseMessage Delete(int portalGroupId)
        {
            GroupManager.Delete(portalGroupId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}