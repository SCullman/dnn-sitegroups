using DotNetNuke.Framework;
using dnneurope.portalgroups.contract;

namespace dnneurope.portalgroups.logic
{
    public class PortalGroups : ServiceLocator<IManagePortalGroups, PortalGroups>
    {
        protected override System.Func<IManagePortalGroups> GetFactory()
        {
            return () => new DotNetNukePortalGroupAdapter();
        }
    }
}