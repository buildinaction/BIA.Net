﻿module BIA.Net.Dialog {
    export function Close(linkElem:JQuery) {
        BIA.Net.Dialog.GetParentDialogDiv(linkElem).dialogElem.dialog('close');
    }

    export function LinkToDialog(scopeElem: JQuery) {
        let currentDialogDiv = GetParentDialogDiv(scopeElem);
        currentDialogDiv.LinkToDialog(scopeElem);
    };

    export function LinkInDialog(scopeElem) {
        let currentDialogDiv = GetParentDialogDiv(scopeElem);
        currentDialogDiv.LinkInDialog(scopeElem);
    };

    export function AddRefreshAction(scopeElem) {
        let currentDialogDiv = GetParentDialogDiv(scopeElem);
        currentDialogDiv.AddRefreshAction(scopeElem);
    };

    export function GetParentDialogDiv(linkElem: JQuery): DialogDiv {
        var dialog = linkElem.closest(".BiaNetDialogDiv");
        if (dialog == null || dialog.length == 0) {
            return DialogDiv.GetMainDiv();
        }
        return DialogDiv.GetDialogDivByJQuery(dialog);
    }
    
    export function ChangeContent(parent: DialogDiv, addHistory: boolean, url: string, DivContent: string, DivScript: string = "", DivType: DialogDivType = DialogDivType.Content) {
        let dialogDiv: DialogDiv = DialogDiv.PrepareContentDiv(parent, DivContent, DivScript, DivType)
        dialogDiv.ReplaceInCurrentDialog(url, addHistory);
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
        var dialogDiv = BIA.Net.Dialog.GetParentDialogDiv(linkElem);
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
        BIA.Net.Dialog.LinkToDialog(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
        BIA.Net.Dialog.AddRefreshAction(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
    });
}