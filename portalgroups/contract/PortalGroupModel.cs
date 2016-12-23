using System.Collections.Generic;

namespace dnneurope.portalgroups.contract
{
    public class PortalGroupModel
    {
        public IEnumerable<PortalGroupInfo> PortalGroups { get; set; }
        public IEnumerable<PortalInfo> AvailablePortals { get; set; }
    }
}