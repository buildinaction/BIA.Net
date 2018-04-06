declare module BIA.Net.Dialog {
    class AjaxLoading {
        static removeParam(keys: any, sourceURL: any): any;
        static getResponseURL(xhr: any): any;
        static aForAbsoluteURL: any;
        static getAbsoluteUrl(url: string): string;
        static UniformmizeUrl(url: any): any;
        static ManageSubmitFormInDailog(dialogDiv: DialogDiv, formElem: JQuery): void;
        private static buildUrl(base, key, value);
        static AjaxLoadDialog(dialogDiv: DialogDiv, url: string, addHistory: boolean): void;
        static SuccesAjaxReplaceInCurrentDialog(data: string, dialogDiv: DialogDiv, urlorigin: string, url: string, responseURL: string, addHistory: boolean): void;
        static ErrorAjaxReplaceInCurrentDialog(data: any, dialogDiv: DialogDiv, urlorigin: any, url: any, responseURL: any, redirectURL: any, addHistory: any): void;
    }
}
