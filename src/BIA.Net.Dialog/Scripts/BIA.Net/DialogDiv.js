var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
            $(document).ready(function () {
                BIA.Net.Dialog.DialogDiv.LinkToDialog(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
                BIA.Net.Dialog.DialogDiv.AddRefreshAction(BIA.Net.Dialog.DialogDiv.GetMainDiv().dialogElem);
            });
            (function (DialogDivType) {
                DialogDivType[DialogDivType["Popup"] = 1] = "Popup";
                DialogDivType[DialogDivType["MainPageContent"] = 2] = "MainPageContent";
                DialogDivType[DialogDivType["Document"] = 3] = "Document";
                DialogDivType[DialogDivType["Content"] = 4] = "Content";
            })(Dialog.DialogDivType || (Dialog.DialogDivType = {}));
            var DialogDivType = Dialog.DialogDivType;
            var RefreshAction = (function () {
                function RefreshAction() {
                }
                RefreshAction.prototype.RefreshContent = function () {
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
                ;
                return RefreshAction;
            }());
            Dialog.RefreshAction = RefreshAction;
            var DialogDiv = (function () {
                function DialogDiv(dialogElem, parent, type) {
                    this.dialogElem = dialogElem;
                    this.children = [];
                    if (parent != null)
                        parent.AddDialogToChildList(this);
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
                DialogDiv.prototype.IsMainDialogDiv = function () {
                    return (this.parent == null);
                };
                DialogDiv.prototype.IsStandardHistory = function () {
                    return (this.type == DialogDivType.Document || this.type == DialogDivType.MainPageContent);
                };
                DialogDiv.prototype.GetParentUrl = function () {
                    if (this.parent == null)
                        return null;
                    else
                        return this.parent.urlCurrent;
                };
                DialogDiv.GetDialogDivByJQuery = function (dialog) {
                    var dialogDiv = null;
                    for (var _i = 0, _a = DialogDiv.AllDialogDiv; _i < _a.length; _i++) {
                        var entry = _a[_i];
                        if (entry.dialogElem.attr('id') == dialog.attr('id')) {
                            dialogDiv = entry;
                            break;
                        }
                    }
                    return dialogDiv;
                };
                DialogDiv.RemoveDialogfromArrayByJQuery = function (dialog) {
                    DialogDiv.AllDialogDiv.forEach(function (entry, index) {
                        if (entry.dialogElem == dialog)
                            DialogDiv.AllDialogDiv.splice(index, 1);
                    });
                };
                DialogDiv.PrepareContentDiv = function (parent, DivContent, DivScript, DivType) {
                    var dialog;
                    if (parent == null)
                        parent = DialogDiv.GetMainDiv();
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
                    var dialogDiv = DialogDiv.GetDialogDivByJQuery(dialog);
                    if (dialogDiv == null)
                        dialogDiv = new DialogDiv(dialog, parent, DivType);
                    dialogDiv.divContent = DivContent;
                    dialogDiv.divScript = DivScript;
                    return dialogDiv;
                };
                DialogDiv.ChangeContent = function (parent, addHistory, url, DivContent, DivScript, DivType) {
                    if (DivScript === void 0) { DivScript = ""; }
                    if (DivType === void 0) { DivType = DialogDivType.Content; }
                    var dialogDiv = DialogDiv.PrepareContentDiv(parent, DivContent, DivScript, DivType);
                    dialogDiv.ReplaceInCurrentDialog(url, addHistory);
                };
                DialogDiv.prototype.ReplaceInCurrentDialog = function (url, addHistory) {
                    var urlsParent = this.GetParentUrl();
                    if (urlsParent != null) {
                        var aUrlsParent = urlsParent.split(';');
                        var urlToTest = Dialog.AjaxLoading.UniformmizeUrl(url);
                        for (var i = 0; i < aUrlsParent.length; i++) {
                            if (urlToTest == Dialog.AjaxLoading.UniformmizeUrl(aUrlsParent[i])) {
                                this.dialogElem.dialog("close");
                                return;
                            }
                        }
                    }
                    BIA.Net.Dialog.AjaxLoading.AjaxLoadDialog(this, url, addHistory);
                };
                DialogDiv.prototype.GetViewPort = function () {
                    if (this.type == DialogDivType.MainPageContent || this.type == DialogDivType.Document)
                        return $(window);
                    return this.dialogElem;
                };
                DialogDiv.GetMainDiv = function () {
                    if (DialogDiv.MainDialogDiv == null) {
                        var MainPageContent = $('.BiaNetMainPageContent');
                        if (MainPageContent != null && MainPageContent.length == 1) {
                            DialogDiv.MainDialogDiv = new DialogDiv(MainPageContent, null, DialogDivType.MainPageContent);
                            DialogDiv.MainDialogDiv.urlCurrent = Dialog.AjaxLoading.UniformmizeUrl(window.location.href);
                        }
                        else {
                            DialogDiv.MainDialogDiv = new DialogDiv($(document), null, DialogDivType.Document);
                            DialogDiv.MainDialogDiv.urlCurrent = Dialog.AjaxLoading.UniformmizeUrl(window.location.href);
                        }
                    }
                    return DialogDiv.MainDialogDiv;
                };
                DialogDiv.GetParentDialogDiv = function (linkElem) {
                    var dialog = linkElem.closest(".BiaNetDialogDiv");
                    if (dialog == null || dialog.length == 0) {
                        return DialogDiv.GetMainDiv();
                    }
                    return DialogDiv.GetDialogDivByJQuery(dialog);
                };
                DialogDiv.LinkInDialog = function (scopeElem) {
                    var currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
                    currentDialogDiv.LinkInDialog(scopeElem);
                };
                ;
                DialogDiv.prototype.LinkInDialog = function (scopeElem) {
                    if (scopeElem == null)
                        scopeElem = this.dialogElem;
                    var currentDialogDiv = this;
                    scopeElem.find('[href],[onclick]').not('[BIADialogLink]').not('[target]')
                        .each(function (e) {
                        BIA.Net.Dialog.DialogLink.PrepareLinkElement($(this), currentDialogDiv);
                    });
                };
                ;
                DialogDiv.LinkToDialog = function (scopeElem) {
                    var currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
                    currentDialogDiv.LinkToDialog(scopeElem);
                };
                ;
                DialogDiv.prototype.LinkToDialog = function (scopeElem) {
                    if (scopeElem == null)
                        scopeElem = this.dialogElem;
                    var currentDialogDiv = this;
                    scopeElem.find('[BIADialogLink]')
                        .each(function (e) {
                        BIA.Net.Dialog.DialogLink.PrepareLinkElement($(this), currentDialogDiv);
                    });
                };
                ;
                DialogDiv.prototype.FormInDialog = function (scopeElem) {
                    if (scopeElem == null)
                        scopeElem = this.dialogElem;
                    var currentDialogDiv = this;
                    scopeElem.find('form').each(function () {
                        var formElem = $(this);
                        // this is the id of the form
                        formElem.submit(function (e) {
                            e.preventDefault(); //stop form from submitting
                            BIA.Net.Dialog.AjaxLoading.ManageSubmitFormInDailog(currentDialogDiv, $(this));
                        });
                    });
                };
                ;
                DialogDiv.prototype.OnDialogLoaded = function (data, newUrl) {
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
                };
                DialogDiv.prototype.OnResizeDialog = function () {
                    var evt = $.Event('OnBIADialogResize');
                    evt.dialog = this.dialogElem;
                    $(window).trigger(evt);
                };
                DialogDiv.prototype.CleanDialog = function () {
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
                };
                DialogDiv.prototype.DisposeDialog = function () {
                    DialogDiv.RemoveDialogfromArrayByJQuery(this.dialogElem);
                    this.CleanDialog();
                    //this.dialogElem.prop("dialogChildList", []);
                };
                ;
                DialogDiv.prototype.AddDialogToChildList = function (child) {
                    this.children.push(child);
                };
                DialogDiv.refreshIfRequiered = function (urlValidated) {
                    urlValidated = urlValidated.toLowerCase();
                    console.log("refreshIfRequiered");
                    var mainDialogDiv = this.GetMainDiv();
                    mainDialogDiv.refreshIfRequiered(urlValidated);
                };
                DialogDiv.prototype.refreshIfRequiered = function (urlValidated) {
                    var elemToRefresh = this;
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
                };
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
                DialogDiv.AddRefreshAction = function (scopeElem) {
                    var currentDialogDiv = DialogDiv.GetParentDialogDiv(scopeElem);
                    currentDialogDiv.AddRefreshAction(scopeElem);
                };
                ;
                DialogDiv.prototype.AddRefreshAction = function (scopeElem) {
                    if (scopeElem == null)
                        scopeElem = this.dialogElem;
                    var CurrentDialogDiv = this;
                    var CurrentDialog = this.dialogElem;
                    var RefrechActionCurrentDialog = [];
                    scopeElem.find('[BIADialogRefresh]')
                        .each(function (e) {
                        var onFormValidated = [];
                        var dialogRefreshUrl = null;
                        var dialogRefreshFormId = null;
                        var dialogRefreshOnlyEvent = false;
                        var DialogRefreshParams = $(this).attr('BIADialogRefresh').split(';');
                        for (var x = 0; x < DialogRefreshParams.length; x++) {
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
                        var refrechAction = new RefreshAction();
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
                    CurrentDialogDiv.refrechAction = RefrechActionCurrentDialog;
                    /*BIA.Net.Dialog.DialogDiv.DialogManager.put(CurrentDialog, {
                        RefrechAction: RefrechActionCurrentDialog, SimilarReturnUrls: DialogSimilarReturnUrlsCurrentDialog
                    });*/
                };
                ;
                DialogDiv.prototype.CleanRefreshAction = function (scopeElem) {
                    this.refrechAction = this.refrechAction.filter(function (e) { return e.elemToRefresh !== scopeElem; });
                };
                DialogDiv.AllDialogDiv = [];
                DialogDiv.MainDialogDiv = null;
                return DialogDiv;
            }());
            Dialog.DialogDiv = DialogDiv;
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
//# sourceMappingURL=DialogDiv.js.map