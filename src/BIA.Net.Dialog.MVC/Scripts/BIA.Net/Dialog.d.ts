declare module BIA.Net.Dialog {
    function Close(linkElem: JQuery): void;
    function LinkToDialog(scopeElem: JQuery): void;
    function FormInDialog(scopeElem: JQuery): void;
    function LinkInDialog(scopeElem: any): void;
    function AddRefreshAction(scopeElem: any): void;
    function GetParentDialogDiv(linkElem: JQuery): DialogDiv;
    function ChangeContent(parent: DialogDiv, addHistory: boolean, url: string, DivContent: string, DivScript?: string, DivType?: DialogDivType, dataToPost?: any, refreshEvent?: boolean): void;
    function DoActionAndRefresh(linkElem: JQuery, urlAction: string): void;
    function RefreshCurrentDialog(linkElem: JQuery): void;
    function RefreshContent(elemToRefresh: JQuery): void;
}
declare module BIA.Net.Dialog {
    class DialogEventContainer {
        IsBiaNetDialogEvent: boolean;
        EventName: string;
        EventData: any;
    }
    class AjaxLoading {
        static initialAppliVersion: string;
        static Init(): void;
        static removeParam(keys: any, sourceURL: any): any;
        static getResponseURL(xhr: any): any;
        static aForAbsoluteURL: any;
        static getAbsoluteUrl(url: string): string;
        static UniformmizeUrl(url: any): any;
        static ManageSubmitFormInDialog(dialogDiv: DialogDiv, formElem: JQuery): void;
        private static buildUrl(base, key, value);
        static AjaxLoadDialog(dialogDiv: DialogDiv, url: string, addHistory: boolean, dataToPost?: any, refreshEvent?: boolean): void;
        static SuccesAjaxReplaceInCurrentDialog(data: any, dialogDiv: DialogDiv, urlorigin: string, url: string, responseURL: string, addHistory: boolean): void;
        static ErrorAjaxReplaceInCurrentDialog(data: any, dialogDiv: DialogDiv, urlorigin: any, url: any, responseURL: any, redirectURL: any, addHistory: any): void;
    }
}
declare module BIA.Net.Dialog {
    enum DialogDivType {
        Popup = 1,
        MainPageContent = 2,
        Document = 3,
        Content = 4,
    }
    class RefreshAction {
        elemToRefresh: JQuery;
        formToRefresh: string;
        refreshUrl: string;
        OnValidatedFormsUrls: string[];
        isOnlyEvent: boolean;
        parentDialogDiv: DialogDiv;
        RefreshContent(): void;
    }
    class DialogDiv {
        static AllDialogDiv: DialogDiv[];
        static MainDialogDiv: any;
        dialogElem: JQuery;
        private events;
        Events: JQuery;
        children: DialogDiv[];
        parent: DialogDiv;
        type: DialogDivType;
        urlCurrent: string;
        IsMainDialogDiv(): boolean;
        IsStandardHistory(): boolean;
        refrechAction: RefreshAction[];
        similarReturnUrls: string;
        divContent: string;
        divScript: string;
        constructor(dialogElem: JQuery, parent: DialogDiv, type: DialogDivType);
        GetParentUrl(): string;
        static GetDialogDivByJQuery(dialog: JQuery): DialogDiv;
        private static RemoveDialogfromArrayByJQuery(dialog);
        static PrepareContentDiv(parent: DialogDiv, DivContent: string, DivScript: string, DivType: DialogDivType): DialogDiv;
        ReplaceInCurrentDialog(url: any, addHistory: any, dataToPost?: any, refreshEvent?: any): void;
        GetViewPort(): any;
        static GetMainDiv(): DialogDiv;
        LinkInDialog(scopeElem?: JQuery): void;
        LinkToDialog(scopeElem?: JQuery): void;
        FormInDialog(scopeElem?: JQuery): void;
        OnDialogLoaded(data: any, newUrl: any): void;
        OnResizeDialog(): void;
        CleanDialog(): void;
        DisposeDialog(): void;
        AddDialogToChildList(child: DialogDiv): void;
        static refreshIfRequiered(urlValidated: any): void;
        private refreshIfRequiered(urlValidated);
        AddRefreshAction(scopeElem?: JQuery): void;
        CleanRefreshAction(scopeElem: JQuery): void;
    }
}
interface JQueryEventObject extends BaseJQueryEventObject, JQueryInputEventObject, JQueryMouseEventObject, JQueryKeyEventObject {
    BIANetDialogData?: BIA.Net.Dialog.DialogEvent;
}
declare module BIA.Net.Dialog {
    class DialogEvent {
        dialogDiv: DialogDiv;
        targetElem: JQuery;
        eventData: any;
        static Send(dialogDiv: DialogDiv, eventName: string, eventData: any, targetElem: JQuery): void;
    }
}
declare module BIA.Net.Dialog {
    class DialogLink {
        linkElem: JQuery;
        url: string;
        dialogDiv: DialogDiv;
        parent: DialogDiv;
        private target;
        constructor(linkElem: JQuery, parent: DialogDiv, url: string);
        static PrepareLinkElement(linkElem: JQuery, parent: DialogDiv): void;
        ReplaceInRelatedDialog(): void;
        PrepareDialogDiv(): void;
        ActionClick(): void;
    }
}
