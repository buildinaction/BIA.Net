module BIA.Net.Dialog {
    export function Close(linkElem:JQuery) {
        BIA.Net.Dialog.DialogDiv.GetParentDialogDiv(linkElem).dialogElem.dialog('close');
    }

    export function DoActionAndRefresh(linkElem: JQuery, urlAction: string) {
        $("body").css("cursor", "progress");
        var ajaxSettings = {

            type: 'POST',
            async: true,
            dataType: 'json',
            url: urlAction,
            linkElem: linkElem,
            success: function (data) {
                RefreshCurrentDialog(this.linkElem);
                $("body").css("cursor", "default");
            },
            error: function (error) {
                console.log("Ajax Error " + error.status + " when calling : " + urlAction);
            }
        }
        $.ajax(ajaxSettings);
    }

    export function RefreshCurrentDialog(linkElem: JQuery) {
        var dialogDiv = BIA.Net.Dialog.DialogDiv.GetParentDialogDiv(linkElem);
        dialogDiv.ReplaceInCurrentDialog(dialogDiv.urlCurrent, false);
    }

    $.fn.submitNoValidation = function (e) {
        $(this).removeData('validator');
        $(this).removeData('unobtrusiveValidation');
        $(this).validate().cancelSubmit = true;
        $(this).submit();
        //e.preventDefault();
        //return false;
    };

    $(document).ready(function () {
        BIA.Net.Dialog.DialogDiv.LinkToDialog($(document));
        BIA.Net.Dialog.DialogDiv.AddRefreshAction($(document));
    });
}
