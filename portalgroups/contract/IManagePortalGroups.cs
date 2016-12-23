namespace dnneurope.portalgroups.contract
{
    public interface IManagePortalGroups
    {
        PortalGroupModel Get();
        int Save(PortalGroupInfo portalGroup);
        void Delete(int portalGroupId);
    }
}