var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
            function DoActionAndRefresh(dollarThis, urlAction) {
                $("body").css("cursor", "progress");
                var ajaxSettings = {
                    type: 'POST',
                    async: true,
                    dataType: 'json',
                    url: urlAction,
                    dollarThis: dollarThis,
                    success: function (data) {
                        RefreshCurrentDialog(this.dollarThis);
                        $("body").css("cursor", "default");
                    },
                    error: function (error) {
                        console.log("Ajax Error " + error.status + " when calling : " + urlAction);
                    }
                };
                $.ajax(ajaxSettings);
            }
            function RefreshCurrentDialog(linkElem) {
                var dialogDiv = BIA.Net.Dialog.DialogDiv.GetParentDialogDiv(linkElem);
                dialogDiv.ReplaceInCurrentDialog(dialogDiv.urlCurrent, false);
            }
            $.fn.submitNoValidation = function (e) {
                $(this).removeData('validator');
                $(this).removeData('unobtrusiveValidation');
                $(this).validate().cancelSubmit = true;
                $(this).submit();
            };
            $(document).ready(function () {
                BIA.Net.Dialog.DialogDiv.LinkToDialog($(document));
                BIA.Net.Dialog.DialogDiv.AddRefreshAction($(document));
            });
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
