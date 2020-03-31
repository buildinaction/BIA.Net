module app.module.Exemple {
    $(document).ready(function () {
        BIA.Net.Dialog.GetParentContentDiv($('#DialogChildPopup')).Events.on('DialogChildPopupAction', function (e) {
            if (e.BIANetDialogData.eventData == "SubmitAndSubmitParent") {
                if ((e.BIANetDialogData.contentDiv != null) && (e.BIANetDialogData.dialogDiv.parent != null)) {
                    BIA.Net.Dialog.AjaxLoading.ManageSubmitFormInDialog(e.BIANetDialogData.dialogDiv.parent.contentDiv, (e.BIANetDialogData.dialogDiv.parent.dialogElem.find('#DialogParentPopup')));
                    e.BIANetDialogData.dialogDiv.dialogElem.dialog("close");
                }
            }
            else if (e.BIANetDialogData.eventData == "SubmitAndCloseParent") {
                if ((e.BIANetDialogData.dialogDiv != null) && (e.BIANetDialogData.dialogDiv.parent != null)) {
                    e.BIANetDialogData.dialogDiv.parent.dialogElem.dialog("close");
                    e.BIANetDialogData.dialogDiv.dialogElem.dialog("close");
                }
            }
            else {
                if ((e.BIANetDialogData.dialogDiv != null) && (e.BIANetDialogData.dialogDiv.parent != null)) {
                    e.BIANetDialogData.dialogDiv.dialogElem.dialog('close');
                    alert("action not implemented :" + e.BIANetDialogData.eventData);
                }
            }
        });
    });
}