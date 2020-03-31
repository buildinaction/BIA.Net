declare namespace DataTables {
    interface Settings {
        buttons?: string[];
    }
}
declare module BIA.Net.DataTable {
    class ParameterAjax {
        columns: any;
        getDataExtend: any;
        constructor(init?: Partial<ParameterAjax>);
    }
    class Parameter {
        tableId: string;
        loadingType: DataTable.LoadingType;
        urlList: string;
        ajax: ParameterAjax;
        exportButtons: string[];
        advancedfilterId: string;
        advancedfilterButtonOpenId: string;
        advancedfilterContentPanelId: string;
        advancedfilterUrl: string;
        dataTableOptionAdditional: DataTables.Settings;
        isLastColumnAction: boolean;
        useFilterInHeader: boolean;
        urlOnClickRow: string;
        rowCallback: any;
        constructor(init?: Partial<Parameter>);
    }
    enum LoadingType {
        Standard = 1,
        Ajax = 2,
        FullAjax = 3
    }
    let DataTableParameters: Parameter[];
    let cultureDataTable: DataTables.LanguageSettings;
    function BuildExportButton(): void;
    function InitFullAjax(dataTableId: any, url_GetListData: any, columns: any, displayExportButton: any, getDataExtend: any, url_OnClickRow: any, rowCallback: any, dataTableOptionAdditional?: any): void;
    function InitFullAjax2(parameter: Parameter): void;
    function InitAjax(dataTableId: any, url_GetListData: any, columns: any, exportButtons: any, filterId: any, url_OnClickRow: any, rowCallback: any, dataTableOptionAdditional?: any): void;
    function InitAjax2(parameter: Parameter): void;
    function ReloadAjax(dataTableId: any): void;
    function InitWithFullCustomisableOptions(dataTableId: any, dataTableOptions: any): void;
    function InitStandard(dataTableId: string, url_List?: string, exportButtons?: any, filterId?: string, dataTableOptionAdditional?: any): void;
    function InitStandard2(parameter: Parameter): void;
    function RefreshList(dataTableId: string, loadingType?: DataTable.LoadingType): void;
    function RefreshList2(parameter: Parameter, viewId?: string, handlerAfterSucess?: () => void): void;
    function InitRefresh(parameter: Parameter, loadingType?: DataTable.LoadingType): void;
    function GetCurrentViewOption(tableId: string): any;
    function GetCurrentHeaderFilter(tableId: string): any[];
    function GetAppliedHeaderFilter(parameter: Parameter): [];
}
declare module BIA.Net.Design {
    let cultureDesign: {
        "showOptions": string;
        "hideOptions": string;
        "show": string;
        "elements": string;
        "views": string;
        "result": string;
    };
    class Site {
        static UrlStatic: string;
        static UrlRoot: string;
    }
    let DesignedDataTables: DataTables.DataTable[];
    class Page {
        static InitPageDesign(baseElem: JQuery): void;
        static PageTooltip(): void;
        static Tooltip(baseElem: JQuery): void;
    }
    class DataTable {
        static InitDataTableDesign(tableId: string, preference: BIA.Net.View.Preference, parameter: DataTable.Parameter): void;
        private static InitSearchHeaderFilters;
        private static ShowNbFilterNotEmpty;
        static NotifyFilter(buttonId: string, nbFiltersNotEmpty: number): void;
    }
}
interface JQuery {
    textWidth(): number;
}
declare module BIA.Net.Design {
    class Helper {
        static ResizeDesign(): void;
        static WidthJS(): void;
        static AutoVPadding(baseElem: JQuery): void;
        static closeMenu(): void;
        static toggleMenu(): void;
    }
}
declare module BIA.Net.Design {
    let DefaultModeFullScreen: boolean;
    let ManualToggleFullScreen: boolean;
    let DefaultTheme: string;
    let ManualTheme: boolean;
    let possibleTheme: string[];
    class Look {
        static InitThemeAndMode(theme: string, fullScreen: boolean, hiddeFullScreenIcon: boolean, hiddeBreadCrumb: boolean): void;
        static SetFullScreen(): void;
        static OutFullScreen(): void;
        static SetBreadcrumb(): void;
        static OutBreadcrumb(): void;
        static toggleBreadcrumb(): void;
        static toggleFullScreen(): void;
    }
}
declare module BIA.Net.Dialog {
    function Close(linkElem: JQuery): void;
    function LinkToContent(scopeElem: JQuery): void;
    function LinkToDialog(scopeElem: JQuery): void;
    function FormInDialog(scopeElem: JQuery): void;
    function LinkInDialog(scopeElem: JQuery): void;
    function AddRefreshAction(scopeElem: JQuery): void;
    function GetParentDialogDiv(linkElem: JQuery): DialogDiv;
    function GetParentContentDiv(linkElem: JQuery): ContentDiv;
    function GetParentElemToRefresh(linkElem: JQuery): JQuery;
    interface ChangeContentParameters {
        dataToPost?: any;
        refreshEvent?: boolean;
        parent?: DialogDiv;
        divType?: ContentDivType;
        handlerAfterSucess?: () => void;
    }
    function ChangeContent2(DivContent: string, url: string, parameters?: Partial<ChangeContentParameters>): void;
    function ChangeContent(DivContent: string, url: string, dataToPost?: any, refreshEvent?: boolean, parent?: DialogDiv, DivType?: ContentDivType, handlerAfterSucess?: () => void, isJSONdataToPost?: boolean): void;
    function DoActionAndChangeMultiContents(contentDivs: JQuery, url: string, dataToPost?: any, refreshEvent?: boolean, parent?: DialogDiv, DivType?: ContentDivType, handlerAfterSucess?: () => void, isJSONdataToPost?: boolean): void;
    function SetUrlToLink(linkElem: any, url: any): void;
    function SetUrlToLinkAndDataToPost(linkElem: any, url: any, getDataToPost: () => any, isJSONdataToPost: boolean): void;
    function DoActionAndRefresh(linkElem: JQuery, urlAction: string): void;
    function RefreshCurrentContent(linkElem: JQuery, handlerAfterSucess?: () => void): void;
    function RefreshContent(elemToRefresh: JQuery, handlerAfterSucess?: () => void): void;
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
        static removeParam(keys: string[], sourceURL: string): string;
        static getResponseURL(xhr: JQueryXHR): string;
        static aForAbsoluteURL: HTMLAnchorElement;
        static getAbsoluteUrl(url: string): string;
        static UniformmizeUrl(url: string): string;
        static ManageSubmitFormInDialog(contentDiv: ContentDiv, formElem: JQuery): void;
        private static buildUrl;
        static AjaxLoadDialog(contentDiv: ContentDiv, url: string, addHistory: boolean, dataToPost?: any, refreshEvent?: boolean, handlerAfterSucess?: () => void, isJSONdataToPost?: boolean): void;
        static AjaxLoadMultiContents(contentDivs: JQuery, url: string, dataToPost?: any, refreshEvent?: boolean, handlerAfterSucess?: () => void, isJSONdataToPost?: boolean): void;
        static SuccesAjaxReplaceInCurrentDialog(data: any, dialogDiv: ContentDiv, urlorigin: string, url: string, responseURL: string, addHistory: boolean): void;
        static ErrorAjaxReplaceInCurrentDialog(data: any, dialogDiv: ContentDiv, urlorigin: string, url: string, responseURL: string, redirectURL: string, addHistory: boolean): void;
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
        RefreshContent(contentDiv: ContentDiv, handlerAfterSucess?: () => void): void;
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
        refreshContentIfRequiered(urlValidated: string): boolean;
        ReplaceInCurrentContent(url: string, addHistory: boolean, dataToPost?: any, refreshEvent?: any, handlerAfterSucess?: () => void, isJSONdataToPost?: boolean): void;
        OnLoaded(data: any, newUrl: string): void;
        NavigationConversion(): void;
        LinkInDialog(scopeElem?: JQuery): void;
        LinkToDialog(scopeElem?: JQuery): void;
        FormInDialog(scopeElem?: JQuery): void;
        AddRefreshAction(scopeElem?: JQuery): void;
        static GetContentDivByJQuery(dialog: JQuery): ContentDiv;
        GetContentDivByJQueryOrCreate(dialog: JQuery, typeIfCreate: ContentDivType): ContentDiv;
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
        GetViewPort(): JQuery;
        static GetMainDialogDiv(): DialogDiv;
        OnLoaded(data: any, newUrl: string): void;
        LinkToContent(scopeElem?: JQuery): void;
        OnResizeDialog(): void;
        AddDialogToChildList(child: DialogDiv): void;
        RemoveDialogToChildList(child: DialogDiv): void;
        static refreshIfRequiered(urlValidated: string): void;
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
        dataToPost: any;
        isJSONdataToPost: boolean;
        constructor(linkElem: JQuery, parent: DialogDiv, url: string);
        static PrepareLinkElement(linkElem: JQuery, parent: DialogDiv): void;
        ReplaceInRelatedDialog(): void;
        PrepareDialogDiv(): void;
        ActionClick(): void;
    }
}
declare module BIA.Net.Filter {
    class Action {
        static ResetFilter(filterId: string, dataTableId: string, loadingType?: DataTable.LoadingType): void;
        static ResetFilter2(filterId: string, tableId: string, loadingType?: DataTable.LoadingType): void;
        static LoadFilterWhenOpen(filterButtonId: string, filterContentId: string, urlContent: string): void;
        static LoadFilterWhenOpen2(filterButtonId: string, filterContentId: string, urlContent: string, tableId: string, advancedfilterId: string): void;
    }
}
declare module BIA.Net.Filter {
    class Form {
        static GetAdvancedFilterValues: (tableId: string) => any;
        static ApplyJDataToAdvancedFilter: (formId: string, jData: any) => void;
        static SerializeObject(theform: JQuery): any;
        static _GetAdvancedFilterValues(parameter: BIA.Net.DataTable.Parameter): any;
        static _ApplyJDataToAdvancedFilter(formId: any, jData: any): void;
        static NotifyAdvancedFilter(parameter: DataTable.Parameter): void;
    }
}
declare module BIA.Net.Helper {
    function AjaxCall(url: any, params: any): void;
}
declare module BIA.Net.Helper {
}
interface ResizeDialogInterface {
    (dialog: JQuery): any;
}
declare var ResizeDialog: ResizeDialogInterface;
declare module BIA.Net.MVC {
    class Localisation {
        static cultureFormatDate: string;
        static cultureFormatGijgoDate: string;
        static cultureFormatGijgoDateTime: string;
        static cultureFormatGijgoTime: string;
        static cultureGijgoTimeMode: string;
        static cultureShort: string;
        static cultureGijgo: string;
        static cultureFormatMomentDate: string;
        static cultureFormatMomentTime: string;
        static isValidatorInit: boolean;
        static SetCalendarDatePicker(root: JQuery): void;
        static SetValidator(): void;
    }
}
declare module BIA.Net.MVC {
    class PreventDuplicateRequest {
        static Apply(form: JQuery): JQuery;
    }
}
declare module BIA.Net.View {
    class ViewPreference {
        viewId: number;
        preference: Preference;
    }
    let ViewsParameter: ViewPreference[];
    var urlViewPopup: string;
    var url_ListSiteView: string;
    var urlCreateView: string;
    var urlUpdateView: string;
    var urlDeleteView: string;
    var urlSetDefaultView: string;
    var urlAssignViewToSite: string;
    class Preference {
        dataTableOption: DataTables.Settings;
        advancedFilterValues: any;
        headerFilterValues: any;
    }
    function GetPreference(tableId: string): string;
    function ApplyView(tableId: string, viewId: string): void;
    function ViewApplied(tableId: string, viewParameter: ViewPreference): void;
}
