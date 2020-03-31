var app;
(function (app) {
    var module;
    (function (module) {
        var Exemple;
        (function (Exemple) {
            $(document).ready(function () {
                setTimeout(function () { BIA.Net.Dialog.ChangeContent("#DivSiteList", UrlSiteIndex); }, 3000);
                BIA.Net.Dialog.SetUrlToLink($("#MyJSLink"), UrlSiteCreate);
            });
        })(Exemple = module.Exemple || (module.Exemple = {}));
    })(module = app.module || (app.module = {}));
})(app || (app = {}));
//# sourceMappingURL=DialogExample.js.map