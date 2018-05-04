module BIA.Net.Dialog {
    $(document).ready(function () {
        BIA.Net.Dialog.DialogDiv.LinkToDialog(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
        BIA.Net.Dialog.DialogDiv.AddRefreshAction(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
    });

    export enum DialogDivType {
        Popup = 1,
        MainPageContent,
        Document,
        Content
    }

    export class RefreshAction {
        //element to replace
        elemToRefresh: JQuery;
        formToRefresh: string;
        //url to requete for replace
        refreshUrl: string;
        //run refresh when one of this form is validated
        OnValidatedFormsUrls: string[];
        //send only an event : to not replace content
        isOnlyEvent: boolean;
        //check if used
        parentDialogDiv: DialogDiv;


        public RefreshContent() {
            var formElem = null;
            var dataToSend = null;
            var formId = this.formToRefresh;
            if (formId != null && formId != "") {
                console.log("BIADialogRefresh : Searh form to refresh : " + formId);
                formElem = $(formId);
                if (formElem.length == 0)
                    formElem = null;
                if (formElem != null) {
                    if ((!formElem.valid) || (formElem.data("validator") && formElem.data("validator").cancelSubmit) || (formElem.valid())) {
                        dataToSend = formElem.serialize();
                    }
                    else {
                        console.log("BIADialogRefresh : Form not valid");
                        return;
                    }
                }
            }
            var refreshUrl = this.refreshUrl;
            if (refreshUrl == null || refreshUrl == "") {
                if (formElem != null) {
                    refreshUrl = formElem.attr('action');
                }
                else {
                    refreshUrl = this.parentDialogDiv.urlCurrent;
                }
            }
            var ParentDialog = this.parentDialogDiv;
            var onlyEvent = this.isOnlyEvent;
            if (onlyEvent) {
                var evt = $.Event('OnBIADialogRefresh');
                evt.dialog = ParentDialog.dialogElem;
                evt.element = this.elemToRefresh;
                $(window).trigger(evt);
            }
            else {
                var evt = $.Event('OnBIADialogRefreshing');
                evt.dialog = ParentDialog.dialogElem;
                evt.element = this.elemToRefresh;
                $(window).trigger(evt);
                var ajaxSettings = {
                    type: "POST",
                    url: refreshUrl,
                    data: dataToSend,
                    ParentDialog: ParentDialog,
                    CurrentDialogDiv: this,
                    success: function (data) {
                        this.ParentDialog.CleanRefreshAction(this.CurrentDialogDiv.elemToRefresh);
                        this.CurrentDialogDiv.elemToRefresh.html(data);
                        this.ParentDialog.LinkInDialog(this.CurrentDialogDiv.elemToRefresh);
                        this.ParentDialog.FormInDialog(this.CurrentDialogDiv.elemToRefresh);
                        this.ParentDialog.LinkToDialog(this.CurrentDialogDiv.elemToRefresh);

                        this.ParentDialog.AddRefreshAction();
                        var evt = $.Event('OnBIADialogRefreshed');
                        evt.dialog = this.ParentDialog.dialogElem;
                        evt.element = this.CurrentDialogDiv.elemToRefresh;
                        $(window).trigger(evt);
                    },
                    error: function (error) {
                        console.log("Ajax Error " + error.status + " when calling : " + refreshUrl);
                    }
                };
                $.ajax(ajaxSettings);
            }
        };

    }

    export class DialogDiv {
        static AllDialogDiv: DialogDiv[] = [];
        static MainDialogDiv = null;

        public dialogElem: JQuery;
        public children: DialogDiv[];
        public parent: DialogDiv;
        public type: DialogDivType;
        public urlCurrent: string;
        public IsMainDialogDiv() {
            return (this.parent == null);
        }
        public IsStandardHistory() {
            return (this.type == DialogDivType.Document || this.type == DialogDivType.MainPageContent);
        }
        public refrechAction: RefreshAction[];
        public similarReturnUrls: string;

        public divContent: string;
        public divScript: string;

        constructor(dialogElem: JQuery, parent: DialogDiv, type: DialogDivType) {
            this.dialogElem = dialogElem;
            this.children = [];
            if (parent != null) parent.AddDialogToChildList(this);
            this.parent = parent;
            this.type = type;
            this.urlCurrent = null;
            DialogDiv.AllDialogDiv.push(this);
            this.refrechAction = null;
            this.similarReturnUrls = null;
            if (!dialogElem.hasClass("BiaNetDialogDiv")) {
                dialogElem.addClass("BiaNetDialogDiv");
            }
        }

        public GetParentUrl(): string {
            if (this.parent == null) return null;
            else return this.parent.urlCurrent;
        }

        public static GetDialogDivByJQuery(dialog: JQuery) {
            let dialogDiv: DialogDiv = null;
            for (let entry of DialogDiv.AllDialogDiv) {
                if (entry.dialogElem.attr('id') == dialog.attr('id')) {
                    dialogDiv = entry;
                    break;
                }
            }
            return dialogDiv;
        }
        private static RemoveDialogfromArrayByJQuery(dialog: JQuery) {
            DialogDiv.AllDialogDiv.forEach((entry, index) => {
                if (entry.dialogElem == dialog) DialogDiv.AllDialogDiv.splice(index, 1);
            });
        }

        public static PrepareContentDiv(parent: DialogDiv, DivContent: string, DivScript: string, DivType: DialogDivType): DialogDiv {
            let dialog: JQuery;
            if (parent == null) parent = DialogDiv.GetMainDiv();
            if (DivContent != "") {
                dialog = parent.dialogElem.find(DivContent);
                if (dialog == null || dialog.length == 0) {
                    dialog = $(document).find(DivContent);
                }
            }
            else
                dialog = parent.dialogElem;
            if (DivScript != "")
                (parent.dialogElem.find(DivScript)).html("");

            let dialogDiv: DialogDiv = DialogDiv.GetDialogDivByJQuery(dialog);

            if (dialogDiv == null) dialogDiv = new DialogDiv(dialog, parent, DivType);
            dialogDiv.divContent = DivContent;
            dialogDiv.divScript = DivScript;

            return dialogDiv;
        }

        public static ChangeContent(parent: DialogDiv, addHistory: boolean, url: string, DivContent: string, DivScript: string = "", DivType: DialogDivType = DialogDivType.Content) {
            let dialogDiv: DialogDiv = DialogDiv.PrepareContentDiv(parent, DivContent, DivScript, DivType)
            dialogDiv.ReplaceInCurrentDialog(url, addHistory);
        }


        public ReplaceInCurrentDialog(url, addHistory) {
            var urlsParent = this.GetParentUrl();
            if (urlsParent != null) {
                var aUrlsParent = urlsParent.split(';');
                var urlToTest = AjaxLoading.UniformmizeUrl(url);
                for (var i = 0; i < aUrlsParent.length; i++) {
                    if (urlToTest == AjaxLoading.UniformmizeUrl(aUrlsParent[i])) {
                        this.dialogElem.dialog("close");
                        return;
                    }
                }
            }
            BIA.Net.Dialog.AjaxLoading.AjaxLoadDialog(this, url, addHistory);
        }


        public GetViewPort() {
            if (this.type == DialogDivType.MainPageContent || this.type == DialogDivType.Document)
                return $(window);
            return this.dialogElem;
        }

        public static GetMainDiv(): DialogDiv {
            if (DialogDiv.MainDialogDiv == null) {
                var MainPageContent = $('.BiaNetMainPageContent');
                if (MainPageContent != null && MainPageContent.length == 1) {
                    DialogDiv.MainDialogDiv = new DialogDiv(MainPageContent, null, DialogDivType.MainPageContent);
                    DialogDiv.MainDialogDiv.urlCurrent = AjaxLoading.UniformmizeUrl(window.location.href);
                }
                else {
                    DialogDiv.MainDialogDiv = new DialogDiv($(document), null, DialogDivType.Document);
                    DialogDiv.MainDialogDiv.urlCurrent = AjaxLoading.UniformmizeUrl(window.location.href);
                }
            }
            return DialogDiv.MainDialogDiv;
        }

        public static GetParentDialogDiv(linkElem: JQuery): DialogDiv {
            var dialog = linkElem.closest(".BiaNetDialogDiv");
            if (dialog == null || dialog.length == 0) {
                return DialogDiv.GetMainDiv();
            }
            return DialogDiv.GetDialogDivByJQuery(dialog);
        }

        public static LinkInDialog(scopeElem) {
            let currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
            currentDialogDiv.LinkInDialog(scopeElem);
        };

        public LinkInDialog(scopeElem?: JQuery) {
            if (scopeElem == null) scopeElem = this.dialogElem;
            let currentDialogDiv = this;
            scopeElem.find('[href],[onclick]').not('[BIADialogLink]').not('[target]')
                .each(function (e) {
                    BIA.Net.Dialog.DialogLink.PrepareLinkElement($(this), currentDialogDiv)
                });
        };

        public static LinkToDialog(scopeElem) {
            let currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
            currentDialogDiv.LinkToDialog(scopeElem);
        };

        public LinkToDialog(scopeElem?: JQuery) {
            if (scopeElem == null) scopeElem = this.dialogElem;
            let currentDialogDiv = this;
            scopeElem.find('[BIADialogLink]')
                .each(function (e) {
                    BIA.Net.Dialog.DialogLink.PrepareLinkElement($(this), currentDialogDiv);
                });
        };

        public FormInDialog(scopeElem?: JQuery) {
            if (scopeElem == null) scopeElem = this.dialogElem;
            let currentDialogDiv = this;
            scopeElem.find('form').each(function () {
                var formElem = $(this);
                // this is the id of the form
                formElem.submit(function (e) {
                    e.preventDefault();    //stop form from submitting
                    BIA.Net.Dialog.AjaxLoading.ManageSubmitFormInDialog(currentDialogDiv, $(this));
                });
            });
        };

        public OnDialogLoaded(data, newUrl) {
            this.urlCurrent = newUrl;
            this.dialogElem.html(data);
            this.LinkInDialog();
            this.FormInDialog();
            this.LinkToDialog();

            this.OnResizeDialog();
            this.AddRefreshAction();
            var evt = $.Event('OnBIADialogLoaded');
            evt.dialog = this.dialogElem;
            $(window).trigger(evt);
        }
        public OnResizeDialog() {
            var evt = $.Event('OnBIADialogResize');
            evt.dialog = this.dialogElem;
            $(window).trigger(evt);
        }
        public CleanDialog() {
            this.dialogElem.append("<div id=\"divLoading\"></div>");
            this.dialogElem.css("cursor", "progress");
            //var childList = this.dialogElem.prop("dialogChildList");
            if (this.children != null) {
                this.children.forEach(function (entry) {
                    entry.DisposeDialog();
                    entry.dialogElem.remove();
                });
            }
            this.children = [];
        }

        public DisposeDialog() {
            DialogDiv.RemoveDialogfromArrayByJQuery(this.dialogElem);
            this.CleanDialog();
            //this.dialogElem.prop("dialogChildList", []);
        };

        public AddDialogToChildList(child: DialogDiv) {
            this.children.push(child);
        }
        public static refreshIfRequiered(urlValidated) {
            urlValidated = urlValidated.toLowerCase();
            console.log("refreshIfRequiered");
            let mainDialogDiv = this.GetMainDiv();
            mainDialogDiv.refreshIfRequiered(urlValidated);
        }
        private refreshIfRequiered(urlValidated) {
            var elemToRefresh: DialogDiv = this;
            elemToRefresh.refrechAction.forEach(function (refrechAction) {
                for (var j = 0; j < refrechAction.OnValidatedFormsUrls.length; j++) {
                    if (urlValidated.indexOf(refrechAction.OnValidatedFormsUrls[j]) >= 0) {
                        refrechAction.RefreshContent();
                        break;
                    }
                }
            });
            if (this.children != null) {
                this.children.forEach(function (e) {
                    e.refreshIfRequiered(urlValidated);
                });
            }
        }
        /*
            Object.keys(BIA.Net.Dialog.DialogDiv.DialogManager.hashes).forEach(function (key) {
                let RefrechActionDialog = BIA.Net.Dialog.DialogDiv.DialogManager.hashes[key].RefrechAction;
                for (var i = 0; i < RefrechActionDialog.length; i++) {
                    var elemToRefresh: DialogDiv = RefrechActionDialog[i].ElemToRefresh;
                    var FormsUrls = elemToRefresh.dialogElem.prop("BIADialogRefreshFormsUrls");
                    for (var j = 0; j < FormsUrls.length; j++) {
                        if (urlValidated.indexOf(FormsUrls[j]) >= 0) {
                            elemToRefresh.BIADialogRefreshContent();
                            break;
                        }
                    }
                }
            });
        }*/

        public static AddRefreshAction(scopeElem) {
            let currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
            currentDialogDiv.AddRefreshAction(scopeElem);
        };

        public AddRefreshAction(scopeElem?: JQuery) {
            if (scopeElem == null) scopeElem = this.dialogElem;
            let CurrentDialogDiv: DialogDiv = this;
            let CurrentDialog: JQuery = this.dialogElem;
            let RefrechActionCurrentDialog: RefreshAction[] = [];
            scopeElem.find('[BIADialogRefresh]')
                .each(function (e) {
                    let onFormValidated: string[] = [];
                    let dialogRefreshUrl: string = null;
                    let dialogRefreshFormId: string = null;
                    let dialogRefreshOnlyEvent: boolean = false;
                    let DialogRefreshParams = $(this).attr('BIADialogRefresh').split(';');
                    for (let x = 0; x < DialogRefreshParams.length; x++) {
                        var d = DialogRefreshParams[x].split(":");
                        if (d.length == 2) {
                            var key = d[0].toLowerCase();
                            var val = d[1].toLowerCase();
                            if (key == "onformvalidated") {
                                onFormValidated = val.split("|");
                            }
                            if (key == "url") {
                                dialogRefreshUrl = val;
                            }
                            if (key == "formid") {
                                dialogRefreshFormId = d[1];
                            }
                        }
                        else if (d.length == 1) {
                            var val = d[0].toLowerCase();
                            if (val == "onlyevent") {
                                dialogRefreshOnlyEvent = true;
                            }
                        }
                    }
                    let refrechAction: RefreshAction = new RefreshAction();
                    refrechAction.elemToRefresh = $(this);
                    refrechAction.refreshUrl = dialogRefreshUrl;
                    refrechAction.formToRefresh = dialogRefreshFormId;
                    refrechAction.isOnlyEvent = dialogRefreshOnlyEvent;
                    refrechAction.OnValidatedFormsUrls = onFormValidated;
                    refrechAction.parentDialogDiv = CurrentDialogDiv;

                    /*
                    $(this).prop("BIADialogRefreshUrl", dialogRefreshUrl);
                    $(this).prop("BIADialogRefreshFormId", dialogRefreshFormId);
                    $(this).prop("BIADialogRefreshOnlyEvent", dialogRefreshOnlyEvent);
                    $(this).prop("BIADialogRefreshParent", CurrentDialog);
                    $(this).prop("BIADialogRefreshFormsUrls", onFormValidated);
                    let elemDialogDiv = new DialogDiv($(this), CurrentDialog, DialogDivType.Content);
                    */
                    RefrechActionCurrentDialog.push(refrechAction);
                });
            //RefrechAction.put(CurrentDialog, RefrechActionCurrentDialog);
            /*var DialogSimilarReturnUrlsCurrentDialog = [];
            scopeElem.find('[BIADialogSimilarReturnUrls]')
                .each(function (e) {
                    var SimilarReturnUrls = $(this).attr('BIADialogSimilarReturnUrls').split(';');
                    DialogSimilarReturnUrlsCurrentDialog = DialogSimilarReturnUrlsCurrentDialog.concat(SimilarReturnUrls);
                    ;
                });*/
            //hashDialogSimilarReturnUrls.put(CurrentDialog, DialogSimilarReturnUrlsCurrentDialog);
            CurrentDialogDiv.refrechAction = RefrechActionCurrentDialog
            /*BIA.Net.Dialog.DialogDiv.DialogManager.put(CurrentDialog, {
                RefrechAction: RefrechActionCurrentDialog, SimilarReturnUrls: DialogSimilarReturnUrlsCurrentDialog
            });*/

        };


        public CleanRefreshAction(scopeElem: JQuery) {
            this.refrechAction = this.refrechAction.filter(e => e.elemToRefresh !== scopeElem);

        }

    }
}

