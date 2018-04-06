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
