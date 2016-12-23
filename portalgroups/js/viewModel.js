//namespace
Type.registerNamespace("dnneurope.portalgroups");

dnneurope.portalgroups.PortalGroupViewModel = function (model) {

    var portals = ko.observableArray(model.Portals);

    return {
        PortalGroupId: model.PortalGroupId,
        MasterPortal: model.MasterPortal,
        PortalGroupName: ko.observable(model.PortalGroupName),
        AuthenticationDomain: ko.observable(model.AuthenticationDomain),
        Portals: portals,
        canBeDeleted: ko.computed(function () {
            return portals().length == 0;
        })
    };
};

dnneurope.portalgroups.ViewModel = function (model, resx, serviceClient) {

    var portalGroups = ko.observableArray(
        ko.utils.arrayMap(
            model.PortalGroups, function (groupModel) {
                return new dnneurope.portalgroups.PortalGroupViewModel(groupModel);
            }));
    var availablePortals = ko.observableArray(model.AvailablePortals);

    //selected items
    var selectedPortal = ko.observable();
    var selectedPortalGroup = ko.observable();
    var selectedPortals = ko.observableArray();


    //cache for rollbacks
    var originalPortalGroup = ko.observable();
    var originalAvailablePortals = ko.observableArray();

    return {
        resx: resx,
        PortalGroups: portalGroups,
        AvailablePortals: availablePortals,
        SelectedPortal: selectedPortal,
        SelectedPortalGroup: selectedPortalGroup,
        SelectedPortals: selectedPortals,

        addPortalGroup: addPortalGroup,
        saveEdit: saveEdit,
        editPortalGroup: editPortalGroup,
        addPortalToGroup: addPortalToGroup,
        removePortalsFromGroup: removePortalsFromGroup,
        deletePortalGroup: deletePortalGroup,

        //visibility helpers
        showList : ko.computed(function () { return !selectedPortalGroup(); }),
        showEditor : ko.computed(function () { return selectedPortalGroup(); }),
        canCreateGroup : ko.computed(function () { return availablePortals() && availablePortals().length > 0; }),
        canAddPortalToGroup : ko.computed(function () { return availablePortals() && availablePortals().length > 0; }),
        canRemovePortalFromGroup : ko.computed(function () { return selectedPortals() && selectedPortals().length > 0; })
    }

    function resetAllSelections() {
        selectedPortalGroup(null);
        selectedPortal(null);
        selectedPortals([]);
    };

    //methods (events)
    function addPortalGroup() {
        console.log(selectedPortal().PortalName);
        var newGroup = new dnneurope.portalgroups.PortalGroupViewModel({
            PortalGroupId: -1,
            MasterPortal: ko.toJS(selectedPortal()),
            PortalGroupName: resx.newPortalGroupPrefix + selectedPortal().PortalName,
            Portals: []
        });
        portalGroups.push(newGroup);
        availablePortals.remove(selectedPortal());
        editPortalGroup(newGroup);
    };

    function saveEdit(group) {
        serviceClient.save(ko.toJS(group)).done(function (id) {
            group.PortalGroupId = id;
            resetAllSelections();
        });
    };

    function editPortalGroup(group) {
        resetAllSelections();
        selectedPortalGroup(group);
        originalPortalGroup(ko.toJS(group));
        originalAvailablePortals(ko.toJS(availablePortals()));
    };

    function addPortalToGroup(group) {
        group.Portals.push(selectedPortal());
        availablePortals.remove(selectedPortal());
    };

    function removePortalsFromGroup(group) {
        ko.utils.arrayForEach(selectedPortals(), function (portal) {
            availablePortals.push(portal);
            group.Portals.remove(portal);
        });
    };

    function deletePortalGroup(group) {
        serviceClient.delete(group.PortalGroupId)
            .done(function () {
                ko.utils.arrayForEach(group.Portals(), function (portal) {
                    availablePortals.push(portal);
                });
                availablePortals.push(group.MasterPortal);
                portalGroups.remove(group);
            });
    };

    function cancelEdit(group) {
        var groupIsNew = group.PortalGroupId === -1;
        if (groupIsNew) {
            portalGroups.remove(group);
        } else {
            group.PortalGroupName(originalPortalGroup().PortalGroupName);
            group.AuthenticationDomain(originalPortalGroup().AuthenticationDomain);
        }
        group.Portals(originalPortalGroup().Portals);
        availablePortals(originalAvailablePortals());
        resetAllSelections();
    };

};