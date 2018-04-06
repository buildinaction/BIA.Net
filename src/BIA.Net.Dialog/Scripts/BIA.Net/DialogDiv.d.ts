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
        static ChangeContent(parent: DialogDiv, addHistory: boolean, url: string, DivContent: string, DivScript: string, DivType: DialogDivType): void;
        ReplaceInCurrentDialog(url: any, addHistory: any): void;
        GetViewPort(): JQuery;
        static GetMainDiv(): DialogDiv;
        static GetParentDialogDiv(linkElem: JQuery): DialogDiv;
        static LinkInDialog(scopeElem: any): void;
        LinkInDialog(scopeElem?: JQuery): void;
        static LinkToDialog(scopeElem: any): void;
        LinkToDialog(scopeElem?: JQuery): void;
        FormInDialog(scopeElem?: JQuery): void;
        OnDialogLoaded(data: any, newUrl: any): void;
        OnResizeDialog(): void;
        CleanDialog(): void;
        DisposeDialog(): void;
        AddDialogToChildList(child: DialogDiv): void;
        static refreshIfRequiered(urlValidated: any): void;
        private refreshIfRequiered(urlValidated);
        static AddRefreshAction(scopeElem: any): void;
        AddRefreshAction(scopeElem?: JQuery): void;
        CleanRefreshAction(scopeElem: JQuery): void;
    }
}
