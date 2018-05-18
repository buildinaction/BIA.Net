interface JQueryEventObject extends BaseJQueryEventObject, JQueryInputEventObject, JQueryMouseEventObject, JQueryKeyEventObject {
    BIANetDialogData?: BIA.Net.Dialog.DialogEvent;
}

module BIA.Net.Dialog {
    export class DialogEvent {
        dialogDiv: DialogDiv;
        targetElem: JQuery;
        eventData: any;

        public static Send(dialogDiv: DialogDiv, eventName: string, eventData: any, targetElem: JQuery) {
            var evt = $.Event(eventName);
            evt.BIANetDialogData = new DialogEvent();
            evt.BIANetDialogData.dialogDiv = dialogDiv;
            evt.BIANetDialogData.eventData = eventData;
            evt.BIANetDialogData.targetElem = targetElem;

            $(window).trigger(evt);
            dialogDiv.dialogElem.trigger(evt);
        }
    }
}