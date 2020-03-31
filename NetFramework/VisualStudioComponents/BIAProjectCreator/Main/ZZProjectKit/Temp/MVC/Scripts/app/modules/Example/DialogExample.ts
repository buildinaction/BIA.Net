module app.module.Exemple {
    declare var UrlSiteIndex: string;
    declare var UrlSiteCreate: string;

    $(document).ready(function () {
        setTimeout(function () { BIA.Net.Dialog.ChangeContent("#DivSiteList", UrlSiteIndex) }, 3000);
        BIA.Net.Dialog.SetUrlToLink($("#MyJSLink"), UrlSiteCreate)
    });   
}