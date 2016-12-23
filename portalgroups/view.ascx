<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="view.ascx.cs" Inherits="dnneurope.portalgroups.view" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" PathNameAlias="SharedScripts" FilePath="knockout.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/dnneurope/portalgroups/js/viewmodel.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/dnneurope/portalgroups/js/serviceclient.js" />


<div id="portalgroups" runat="server">
    <div data-bind="visible: showList">
        <table class="tableDefault">
            <thead>
                <tr>
                    <th>&nbsp;</th>
                    <th data-bind="text: resx.portalGroup"></th>
                    <th data-bind="text: resx.masterPortal"></th>
                </tr>
            </thead>
            <tbody data-bind="foreach: PortalGroups">
                <tr>
                    <td>
                        <img alt="edit" src='<%= IconURL("Edit") %>' data-bind="click: $root.editPortalGroup" />
                        <img alt="delete" src='<%= IconURL("Delete") %>' data-bind="click: $root.deletePortalGroup, visible: canBeDeleted" />
                    </td>
                    <td><span data-bind="text: PortalGroupName"></span></td>
                    <td><span data-bind="text: MasterPortal.PortalName"></span></td>
                </tr>
            </tbody>
        </table>

        <div data-bind="visible: canCreateGroup">
            <select data-bind="options: AvailablePortals, optionsText: 'PortalName', value: SelectedPortal, optionsCaption: resx.chooseMasterPortal"></select>
            <button class="primaryButton" data-bind="click: addPortalGroup, visible: SelectedPortal, text: resx.createPortalGroupButton"></button>
        </div>
    </div>
    <div class="dnnForm  dnnClear" data-bind="with: SelectedPortalGroup, visible: SelectedPortalGroup">
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;">
                <label data-bind="text: $root.resx.masterPortal" for="MasterPortal" />
            </div>
            <input id="MasterPortal" type="text" data-bind="value: MasterPortal.PortalName" disabled="disabled" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;">
                <label for="PortalGroupName" data-bind="text: $root.resx.portalGroupName" />
            </div>
            <input id="PortalGroupName" type="text" data-bind="value: PortalGroupName" /> *
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;">
                <label for="AuthenticationDomain" data-bind="text: $root.resx.authenticationDomain" />
            </div>
            <input id="AuthenticationDomain" type="text" data-bind="value: AuthenticationDomain" /> *
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;">
                <label for="Portals" data-bind="text: $root.resx.portals" />
                <label>Portals</label>
            </div>
            <select id="Portals" multiple="multiple" data-bind="options: Portals, selectedOptions: $root.SelectedPortals, optionsText: 'PortalName'"></select>
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"></div>
            <select data-bind="options: $root.AvailablePortals, visible: $root.canAddPortalToGroup, optionsText: 'PortalName', value: $root.SelectedPortal, optionsCaption: $root.resx.choosePortalToAdd"></select>
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"></div>
            <button data-bind="click: $root.addPortalToGroup, enable: $root.SelectedPortal, visible: $root.canAddPortalToGroup, text: $root.resx.addButton">Add</button>
            <button data-bind="click: $root.removePortalsFromGroup, enable: $root.canRemovePortalFromGroup, text: $root.resx.removeButton">Remove</button>
        </div>
        <ul class="dnnActions dnnClear">
            <li>
                <button class="primaryButton" data-bind="click: $root.saveEdit, text: $root.resx.saveButton">Save</button></li>
            <li>
                <button class="secondaryButton" data-bind="click: $root.cancelEdit, text: $root.resx.cancelButton">Cancel</button></li>
        </ul>
    </div>
</div>
<script>
    jQuery(document).ready(function ($) {
        var client = new dnneurope.portalgroups.ServiceClient(<%=ModuleId %>);
        var localization = {
            masterPortal: '<%=Localize("MasterPortal")%>',
            chooseMasterPortal: '<%=Localize("ChooseMasterPortal")%>',
            choosePortalToAdd: '<%=Localize("ChoosePortalToAdd")%>',
            createPortalGroupButton: '<%=Localize("CreatePortalGroupButton")%>',
            portalGroup: '<%=Localize("PortalGroup")%>',
            portalGroupName: '<%=Localize("PortalGroupName")%>',
            authenticationDomain: '<%=Localize("AuthenticationDomain")%>',
            portals: '<%=Localize("Portals")%>',
            addButton: '<%=Localize("AddButton")%>',
            removeButton: '<%=Localize("RemoveButton")%>',
            saveButton: '<%=Localize("SaveButton")%>',
            cancelButton: '<%=Localize("CancelButton")%>',
            newPortalGroupPrefix: '<%=Localize("NewPortalGroupPrefix")%>'
        };
        client.getModel()
            .done(function (model) {
                var viewModel = new dnneurope.portalgroups.ViewModel(model, localization , client);
                ko.applyBindings(viewModel, document.getElementById("<%= portalgroups.ClientID %>"));
            });
    });
</script>
