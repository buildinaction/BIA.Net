declare module BIA.Net.DataTable {
    let cultureDataTable: DataTables.LanguageSettings;
    function InitFullAjax(dataTableId: any, url_GetListData: any, columns: any, displayExportButton: any, getDataExtend: any, url_OnClickRow: any, rowCallback: any): void;
    function InitAjax(dataTableId: any, url_GetListData: any, columns: any, exportButtons: any, formId: any, url_OnClickRow: any, rowCallback: any): void;
    function ReloadAjax(dataTableId: any): void;
    function InitWithFullCustomisableOptions(dataTableId: any, dataTableOptions: any): void;
    function InitStandard(dataTableId: any, exportButtons: any): void;
}
declare module BIA.Net.Design {
    let cultureDesign: {
        "showOptions": string;
        "hideOptions": string;
        "show": string;
        "elements": string;
        "result": string;
    };
    class Page {
        static InitPageDesign(baseElem: JQuery): void;
    }
    class DataTable {
        static InitDataTableDesign(table: DataTables.DataTable): void;
    }
}
interface JQuery {
    textWidth(): number;
}
declare module BIA.Net.Design {
    class Helper {
        static WidthJS(): void;
        static AutoVPadding2(): void;
        static AutoVPadding(baseElem: JQuery): void;
        static closeMenu(): void;
        static toggleMenu(): void;
    }
}
declare module BIA.Net.Dialog {
    function Close(linkElem: JQuery): void;
    function LinkToDialog(scopeElem: JQuery): void;
    function FormInDialog(scopeElem: JQuery): void;
    function LinkInDialog(scopeElem: any): void;
    function AddRefreshAction(scopeElem: any): void;
    function GetParentDialogDiv(linkElem: JQuery): DialogDiv;
    function GetParentContentDiv(linkElem: JQuery): ContentDiv;
    function GetParentElemToRefresh(linkElem: JQuery): JQuery;
    function ChangeContent(DivContent: string, url: string, dataToPost?: any, refreshEvent?: boolean, parent?: DialogDiv, DivType?: ContentDivType): void;
    function DoActionAndRefresh(linkElem: JQuery, urlAction: string): void;
    function RefreshCurrentContent(linkElem: JQuery): void;
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
        static initialApplication: string;
        static Init(): void;
        static removeParam(keys: any, sourceURL: any): any;
        static getResponseURL(xhr: any): any;
        static aForAbsoluteURL: any;
        static getAbsoluteUrl(url: string): string;
        static UniformmizeUrl(url: any): any;
        static ManageSubmitFormInDialog(dialogDiv: ContentDiv, formElem: JQuery): void;
        private static buildUrl;
        static AjaxLoadDialog(dialogDiv: ContentDiv, url: string, addHistory: boolean, dataToPost?: any, refreshEvent?: boolean): void;
        static SuccesAjaxReplaceInCurrentDialog(data: any, dialogDiv: ContentDiv, urlorigin: string, url: string, responseURL: string, addHistory: boolean): void;
        static ErrorAjaxReplaceInCurrentDialog(data: any, dialogDiv: ContentDiv, urlorigin: any, url: any, responseURL: any, redirectURL: any, addHistory: any): void;
    }
}
declare module BIA.Net.Dialog {
    enum ContentDivType {
        Popup = 1,
        MainPageContent = 2,
        Document = 3,
        Content = 4,
        Refresh = 5
    }
    class RefreshAction {
        formToRefresh: string;
        refreshUrl: string;
        OnValidatedFormsUrls: string[];
        isOnlyEvent: boolean;
        parentContentDiv: ContentDiv;
        RefreshContent(contentDiv: ContentDiv): void;
    }
    class ContentDiv {
        private static MainContentDiv;
        private static AllContentDiv;
        dialogElem: JQuery;
        protected events: JQuery;
        readonly Events: JQuery;
        protected refreshEvents: JQuery;
        readonly RefreshEvents: JQuery;
        refrechAction: RefreshAction[];
        contentChildren: ContentDiv[];
        private currentDialogDiv;
        CurrentDialogDiv: DialogDiv;
        private parentContent;
        ParentContent: ContentDiv;
        IsStandardHistory(): boolean;
        urlCurrent: string;
        type: ContentDivType;
        private divScript;
        constructor(dialogElem: JQuery, type: ContentDivType);
        Clean(): void;
        Dispose(): void;
        private static RemoveContentFromAll;
        static GetMainContentDiv(): ContentDiv;
        refreshContentIfRequiered(urlValidated: any): boolean;
        ReplaceInCurrentContent(url: any, addHistory: any, dataToPost?: any, refreshEvent?: any): void;
        OnLoaded(data: any, newUrl: any): void;
        NavigationConversion(): void;
        LinkInDialog(scopeElem?: JQuery): void;
        LinkToDialog(scopeElem?: JQuery): void;
        FormInDialog(scopeElem?: JQuery): void;
        AddRefreshAction(scopeElem?: JQuery): void;
        static GetContentDivByJQuery(dialog: JQuery): ContentDiv;
        static GetContentDivByJQueryOrCreate(dialog: JQuery, parentDivIfCreate: DialogDiv, typeIfCreate: ContentDivType): ContentDiv;
    }
    class DialogDiv {
        private static AllDialogDiv;
        static GetContentDivByJQuery(dialog: JQuery): DialogDiv;
        private static RemoveDialogFromAll;
        static GetDialogDivByJQuery(dialog: JQuery): DialogDiv;
        private static MainDialogDiv;
        contentDiv: ContentDiv;
        dialogElem: JQuery;
        children: DialogDiv[];
        parent: DialogDiv;
        IsMainDialogDiv(): boolean;
        refrechAction: RefreshAction[];
        similarReturnUrls: string;
        constructor(dialogElem: JQuery, parent: DialogDiv, contentDiv: ContentDiv);
        Clean(): void;
        Dispose(): void;
        GetParentUrl(): string;
        GetViewPort(): any;
        static GetMainDialogDiv(): DialogDiv;
        OnLoaded(data: any, newUrl: any): void;
        LinkToContent(scopeElem?: JQuery): void;
        OnResizeDialog(): void;
        AddDialogToChildList(child: DialogDiv): void;
        static refreshIfRequiered(urlValidated: any): void;
        private refreshIfRequiered;
    }
}
interface JQueryEventObject extends BaseJQueryEventObject, JQueryInputEventObject, JQueryMouseEventObject, JQueryKeyEventObject {
    BIANetDialogData?: BIA.Net.Dialog.DialogEvent;
}
declare module BIA.Net.Dialog {
    class DialogEvent {
        dialogDiv: DialogDiv;
        contentDiv: ContentDiv;
        targetElem: JQuery;
        eventData: any;
        static Send(contentDiv: ContentDiv, eventName: string, eventData: any, targetElem: JQuery): void;
    }
}
declare module BIA.Net.Dialog {
    class DialogLink {
        linkElem: JQuery;
        url: string;
        targetContent: ContentDiv;
        parent: DialogDiv;
        private target;
        constructor(linkElem: JQuery, parent: DialogDiv, url: string);
        static PrepareLinkElement(linkElem: JQuery, parent: DialogDiv): void;
        ReplaceInRelatedDialog(): void;
        PrepareDialogDiv(): void;
        ActionClick(): void;
    }
}
declare module BIA.Net.Helper {
    function AjaxCall(url: any, params: any): void;
}
declare module BIA.Net.Design {
    let DefaultModeFullScreen: boolean;
    let ManualToggleFullScreen: boolean;
    let DefaultTheme: string;
    let ManualTheme: boolean;
    let possibleTheme: string[];
    class Look {
        static InitThemeAndMode(theme: any, fullScreen: any, hiddeFullScreenIcon: any, hiddeBreadCrumb: any): void;
        static SetFullScreen(): void;
        static OutFullScreen(): void;
        static SetBreadcrumb(): void;
        static OutBreadcrumb(): void;
        static toggleBreadcrumb(): void;
        static toggleFullScreen(): void;
    }
}
interface ResizeDialogInterface {
    (dialog: JQuery): any;
}
declare var ResizeDialog: ResizeDialogInterface;
declare module BIA.Net.MVC {
    class Localisation {
        static cultureFormatDate: string;
        static cultureShort: string;
        static SetCalendarDatePicker(root: any): void;
        static SetFloatValidator(): void;
    }
}
declare module BIA.Net.MVC {
    class PreventDuplicateRequest {
        static Apply(form: JQuery): JQuery;
    }
}
