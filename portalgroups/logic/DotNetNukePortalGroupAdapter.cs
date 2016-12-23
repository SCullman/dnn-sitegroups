using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using dnneurope.portalgroups.contract;
using PortalGroupInfo = dnneurope.portalgroups.contract.PortalGroupInfo;
using PortalInfo = dnneurope.portalgroups.contract.PortalInfo;

namespace dnneurope.portalgroups.logic
{
    public class DotNetNukePortalGroupAdapter : IManagePortalGroups
    {
        IPortalGroupController PortalGroupController
        {
            get { return DotNetNuke.Entities.Portals.PortalGroupController.Instance; }
        }

        public PortalGroupModel Get()
        {
            var groups = PortalGroupController.GetPortalGroups().Select(g => new PortalGroupInfo
                {
                    AuthenticationDomain = g.AuthenticationDomain,
                    PortalGroupId = g.PortalGroupId,
                    MasterPortal = new PortalInfo
                        {
                            PortalName = g.MasterPortalName,
                            PortalId = g.MasterPortalId
                        },
                    PortalGroupName = g.PortalGroupName,
                    Portals = PortalsOfGroup(g.PortalGroupId,g.MasterPortalId)
                        .Select(p => new PortalInfo
                            {
                                PortalId = p.PortalID,
                                PortalName = p.PortalName
                            })

                });
            
            var availablePortals =
                new PortalController().GetPortals()
                        .Cast<DotNetNuke.Entities.Portals.PortalInfo>()
                        .Where(x => x.PortalGroupID == Null.NullInteger)
                        .Select(p => new PortalInfo
                            {
                                PortalId = p.PortalID,
                                PortalName = p.PortalName
                            });

            var model = new PortalGroupModel
                            {
                                PortalGroups = groups,
                                AvailablePortals = availablePortals
                            };
            return model;
        }

        IEnumerable<DotNetNuke.Entities.Portals.PortalInfo> PortalsOfGroup(int groupId, int masterPortalId)
        {
            return PortalGroupController
                .GetPortalsByGroup(groupId)
                .Where(x => x.PortalID != masterPortalId);
        }

        public int Save(PortalGroupInfo portalGroup)
        {
            Trace.TraceInformation("Save PortalGroup");
            if (portalGroup.PortalGroupId == -1)
            {
                return AddPortalGroup(portalGroup);
            }
            else
            {
                return UpdatePortalGroup(portalGroup);
            }
        }

        int UpdatePortalGroup(PortalGroupInfo portalGroup)
        {
            UserCopiedCallback callback = delegate { };
            var @group = PortalGroupController.GetPortalGroups().Single(g => g.PortalGroupId == portalGroup.PortalGroupId);
            @group.PortalGroupName = portalGroup.PortalGroupName;
            @group.AuthenticationDomain = portalGroup.AuthenticationDomain;
            PortalGroupController.UpdatePortalGroup(@group);
            var currentPortals = PortalsOfGroup(portalGroup.PortalGroupId,portalGroup.MasterPortal.PortalId).ToList();
            foreach (var portal in currentPortals)
            {
                if (portalGroup.Portals == null || portalGroup.Portals.All(p => p.PortalId != portal.PortalID))
                    PortalGroupController.RemovePortalFromGroup(portal, @group, false, callback);
            }

            if (portalGroup.Portals != null)
                foreach (var portal in portalGroup.Portals)
                {
                    if (currentPortals.All(p => p.PortalID != portal.PortalId))
                    {
                        var p = new PortalController().GetPortal(portal.PortalId);
                        PortalGroupController.AddPortalToGroup(p, @group, callback);
                    }
                }
            return @group.PortalGroupId;
        }

        int AddPortalGroup(PortalGroupInfo portalGroup)
        {
             UserCopiedCallback callback = delegate { };
            var group = new DotNetNuke.Entities.Portals.PortalGroupInfo
                {
                    AuthenticationDomain = portalGroup.AuthenticationDomain,
                    MasterPortalId = portalGroup.MasterPortal.PortalId,
                    PortalGroupDescription = portalGroup.PortalGroupName,
                    PortalGroupName = portalGroup.PortalGroupName
                };
            PortalGroupController.AddPortalGroup(@group);
            if (portalGroup.Portals != null)
            {
                foreach (var portal in portalGroup.Portals)
                {
                    var p = new PortalController().GetPortal(portal.PortalId);
                    PortalGroupController.AddPortalToGroup(p, @group, callback);
                }
            }
            return @group.PortalGroupId;
        }

        public void Delete(int portalGroupId)
        {
            Trace.TraceInformation("Delete PortalGroup");
            var group = PortalGroupController.GetPortalGroups().Single(g => g.PortalGroupId == portalGroupId);
            PortalGroupController.DeletePortalGroup(group);
        }
    }
}