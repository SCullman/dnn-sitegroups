using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;

namespace dnneurope.portalgroups
{
    [DNNtc.PackageProperties("dnnEurope.portalgroups", 1, "Site Group Editor", "Site Group Editor", "BBxxx1.png", "Stefan Cullmann", "dnn connect", "http://dnn-connect.org", "stefan.cullmann@gmail.com", true)]
    [DNNtc.ModuleDependencies(DNNtc.ModuleDependency.CoreVersion, "07.04.00")]
    [DNNtc.ModuleProperties("dnnEurope.portalgroups", "Site Group Editor", -1)]
    [DNNtc.ModuleControlProperties("", "", DNNtc.ControlType.Host, "", false, false)]
    public partial class view : PortalModuleBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //jQuery.RequestRegistration();
            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
        }

        protected string IconURL(string key)
        {
            return DotNetNuke.Entities.Icons.IconController.IconURL(key);
        }

        protected string Localize(string key)
        {
            return DotNetNuke.UI.Utilities.ClientAPI.GetSafeJSString(LocalizeString( key));
        }
      
    }
}