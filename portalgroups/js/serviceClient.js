Type.registerNamespace("dnneurope.portalgroups");

dnneurope.portalgroups.ServiceClient = function (moduleId) {
    var self = this;
    var serviceFramework = $.ServicesFramework(moduleId);
    var serviceRoot = serviceFramework.getServiceRoot('dnneurope.portalgroups') + 'PortalGroup/';

    self.getModel = function () {
        console.log("get Model from Service");
        return $.ajax({
            type: "GET",
            url: serviceRoot + "Get",
            beforeSend: serviceFramework.setModuleHeaders,
            cache: false
        });
    };
    self.save = function(group) {
        console.log("save Group with Service");
        console.log(group);
        return $.ajax({
            type: "POST",
            url: serviceRoot + "Save",
            beforeSend: serviceFramework.setModuleHeaders,
            data: group,
            cache: false
        });
    };
    self.delete = function (groupId) {
        console.log("delete Group by id with Service");
        return $.ajax({
            type: "GET",
            url: serviceRoot + "Delete",
            beforeSend: serviceFramework.setModuleHeaders,
            data:{portalGroupId:groupId} ,
            cache: false
        });
    };
};
