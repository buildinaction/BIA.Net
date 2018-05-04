var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
            function DoActionAndRefresh(dollarThis, urlAction) {
                $("body").css("cursor", "progress");
                var ajaxSettings = {
                    type: 'POST',
                    async: true,
                    dataType: 'json',
                    url: urlAction,
                    dollarThis: dollarThis,
                    success: function (data) {
                        RefreshCurrentDialog(this.dollarThis);
                        $("body").css("cursor", "default");
                    },
                    error: function (error) {
                        console.log("Ajax Error " + error.status + " when calling : " + urlAction);
                    }
                };
                $.ajax(ajaxSettings);
            }
            function RefreshCurrentDialog(linkElem) {
                var dialogDiv = BIA.Net.Dialog.DialogDiv.GetParentDialogDiv(linkElem);
                dialogDiv.ReplaceInCurrentDialog(dialogDiv.urlCurrent, false);
            }
            $.fn.submitNoValidation = function (e) {
                $(this).removeData('validator');
                $(this).removeData('unobtrusiveValidation');
                $(this).validate().cancelSubmit = true;
                $(this).submit();
                //e.preventDefault();
                //return false;
            };
            $(document).ready(function () {
                BIA.Net.Dialog.DialogDiv.LinkToDialog($(document));
                BIA.Net.Dialog.DialogDiv.AddRefreshAction($(document));
            });
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
            var xhr;
            var _orgAjax = jQuery.ajaxSettings.xhr;
            jQuery.ajaxSettings.xhr = function () {
                xhr = _orgAjax();
                return xhr;
            };
            var AjaxLoading = (function () {
                function AjaxLoading() {
                }
                AjaxLoading.removeParam = function (keys, sourceURL) {
                    var rtn = sourceURL.split("?")[0], param, params_arr = [], queryString = (sourceURL.indexOf("?") !== -1) ? sourceURL.split("?")[1] : "";
                    if (queryString !== "") {
                        params_arr = queryString.split("&");
                        for (var i = params_arr.length - 1; i >= 0; i -= 1) {
                            param = params_arr[i].split("=")[0];
                            if (keys.indexOf(param.toLowerCase()) !== -1) {
                                params_arr.splice(i, 1);
                            }
                        }
                        if (params_arr.length > 0)
                            rtn = rtn + "?" + params_arr.join("&");
                    }
                    return rtn;
                };
                AjaxLoading.getResponseURL = function (xhr) {
                    /*    if ('responseURL' in xhr) {
                            return xhr.responseURL;
                        }
                        if (/^X-Request-URL:/m.test(xhr.getAllResponseHeaders())) {
                            return xhr.getResponseHeader('X-Request-URL');
                        }*/
                    if (/^bianetdialogredirectedurl:/m.test(xhr.getAllResponseHeaders())) {
                        return xhr.getResponseHeader('bianetdialogredirectedurl');
                    }
                    return;
                };
                AjaxLoading.getAbsoluteUrl = function (url) {
                    AjaxLoading.aForAbsoluteURL = AjaxLoading.aForAbsoluteURL || document.createElement('a');
                    AjaxLoading.aForAbsoluteURL.href = url;
                    return AjaxLoading.aForAbsoluteURL.href;
                };
                AjaxLoading.UniformmizeUrl = function (url) {
                    return AjaxLoading.removeParam(['bianetdialogurlparent', 'bianetdialogdisplayflag', 'bianetdialogredirectedurl'], AjaxLoading.getAbsoluteUrl(url).toLowerCase().replace(/#/g, "").replace(/\/$/, ""));
                };
                AjaxLoading.ManageSubmitFormInDialog = function (dialogDiv, formElem) {
                    if ((!formElem.valid) || (formElem.data("validator") && formElem.data("validator").cancelSubmit) || (formElem.valid())) {
                        //console.log("FormInDialog : submit");
                        var url = formElem.attr('action');
                        var dialog = dialogDiv.dialogElem;
                        var parentUrl = dialogDiv.GetParentUrl();
                        //console.log("FormInDialog : parentUrl ::" + parentUrl);
                        formElem.append('<input type=\'hidden\' name=\'BIANetDialogDisplayFlag\' value=\'' + dialogDiv.type + '\' />');
                        formElem.append('<input type=\'hidden\' name=\'BIANetDialogUrlParent\' value=\'' + parentUrl + '\' />');
                        var dataToSend = formElem.serialize(); // serializes the form's elements.
                        var urlBefore = dialogDiv.urlCurrent;
                        //console.log("FormInDialog : begin ajax");
                        var ajaxSettings = {
                            type: "POST",
                            url: url,
                            urlBefore: urlBefore,
                            dialogDiv: dialogDiv,
                            data: dataToSend,
                            success: function (data, textStatus, xhr) {
                                Dialog.DialogDiv.refreshIfRequiered(this.urlBefore);
                                AjaxLoading.SuccesAjaxReplaceInCurrentDialog(data, this.dialogDiv, this.url, this.url, AjaxLoading.getResponseURL(xhr), this.dialogDiv.IsStandardHistory());
                            },
                            error: function (xhr, textStatus, thrownError) {
                                AjaxLoading.ErrorAjaxReplaceInCurrentDialog(xhr.responseText, this.dialogDiv, this.url, this.url, AjaxLoading.getResponseURL(xhr), xhr.getResponseHeader('location'), this.dialogDiv.IsStandardHistory());
                            }
                        };
                        dialogDiv.dialogElem.append("<div id=\"divLoading\"></div>");
                        dialogDiv.dialogElem.css("cursor", "progress");
                        $.ajax(ajaxSettings);
                    }
                };
                AjaxLoading.buildUrl = function (base, key, value) {
                    var sep = (base.indexOf('?') > -1) ? '&' : '?';
                    return base + sep + key + '=' + value;
                };
                ;
                AjaxLoading.AjaxLoadDialog = function (dialogDiv, url, addHistory) {
                    var DialogType = dialogDiv.type;
                    var url_timed = AjaxLoading.buildUrl(url, 'BIANetDialogDisplayFlag', DialogType.toString());
                    url_timed = AjaxLoading.buildUrl(url_timed, 'BIANetDialogUrlParent', dialogDiv.GetParentUrl());
                    var ajaxSettings = {
                        url: url_timed,
                        urlOrigin: url,
                        dialogDiv: dialogDiv,
                        cache: false,
                        success: function (data, textStatus, xhr) {
                            //alert(xhr.getAllResponseHeaders());
                            AjaxLoading.SuccesAjaxReplaceInCurrentDialog(data, this.dialogDiv, this.urlOrigin, this.url, AjaxLoading.getResponseURL(xhr), addHistory);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            AjaxLoading.ErrorAjaxReplaceInCurrentDialog(xhr.responseText, this.dialogDiv, this.urlOrigin, this.url, AjaxLoading.getResponseURL(xhr), xhr.getResponseHeader('location'), addHistory);
                        }
                    };
                    dialogDiv.dialogElem.append("<div id=\"divLoading\"></div>");
                    dialogDiv.dialogElem.css("cursor", "progress");
                    $.ajax(ajaxSettings);
                };
                ;
                AjaxLoading.SuccesAjaxReplaceInCurrentDialog = function (data, dialogDiv, urlorigin, url, responseURL, addHistory) {
                    //console.log("SuccesAjaxReplaceInCurrentDialog");
                    if (data == "Close Dialog") {
                        //console.log("close detected");
                        //dialogDiv.dialogElem.html("");
                        dialogDiv.dialogElem.dialog("close");
                    }
                    else if (data == "Close Parent Dialog") {
                        //console.log("close detected");
                        if (dialogDiv.parent.type == Dialog.DialogDivType.Popup) {
                            dialogDiv.parent.dialogElem.dialog("close");
                        }
                    }
                    else if (data.indexOf("Action Dialog:") == 0) {
                        //console.log("close detected");
                        var str = data.substr(14);
                        var evt = $.Event('OnBIADialogAction');
                        evt.dialog = dialogDiv.dialogElem;
                        evt.action = str;
                        $(window).trigger(evt);
                    }
                    else {
                        dialogDiv.CleanDialog();
                        var newUrl = "";
                        if (responseURL && (AjaxLoading.UniformmizeUrl(responseURL) != AjaxLoading.UniformmizeUrl(url)))
                            newUrl = AjaxLoading.UniformmizeUrl(responseURL);
                        else
                            newUrl = urlorigin;
                        dialogDiv.OnDialogLoaded(data, newUrl);
                        //Change the URL history
                        if (addHistory) {
                            var titleElems = dialogDiv.dialogElem.find('title');
                            var title = "";
                            if (titleElems != null && titleElems.length > 0) {
                                title = titleElems[0].innerHTML;
                                document.getElementsByTagName('title')[0].innerHTML = title;
                            }
                            window.history.pushState({ ajaxUrl: newUrl, ajaxDivContent: dialogDiv.divContent, ajaxDivScript: dialogDiv.divScript, ajaxDivType: dialogDiv.type }, title, newUrl);
                        }
                    }
                    dialogDiv.dialogElem.css("cursor", "default");
                };
                AjaxLoading.ErrorAjaxReplaceInCurrentDialog = function (data, dialogDiv, urlorigin, url, responseURL, redirectURL, addHistory) {
                    //console.log("ErrorAjaxReplaceInCurrentDialog");
                    if (redirectURL != null && redirectURL != "") {
                        if (addHistory) {
                            var title = "Error";
                            document.getElementsByTagName('title')[0].innerHTML = title;
                            window.history.pushState({ ajaxUrl: urlorigin, ajaxDivContent: dialogDiv.divContent, ajaxDivScript: dialogDiv.divScript, ajaxDivType: dialogDiv.type }, title, urlorigin);
                        }
                        AjaxLoading.AjaxLoadDialog(dialogDiv, redirectURL, false);
                    }
                    else {
                        AjaxLoading.SuccesAjaxReplaceInCurrentDialog(data, dialogDiv, urlorigin, url, responseURL, addHistory);
                    }
                };
                AjaxLoading.aForAbsoluteURL = null;
                return AjaxLoading;
            }());
            Dialog.AjaxLoading = AjaxLoading;
            $(window).on("popstate", function () {
                if (history.state != null && history.state.ajaxUrl != null && history.state.ajaxUrl != "") {
                    BIA.Net.Dialog.DialogDiv.ChangeContent(BIA.Net.Dialog.DialogDiv.GetMainDiv(), false, history.state.ajaxUrl, history.state.ajaxDivContent, history.state.ajaxDivScript, history.state.ajaxDivType);
                }
                else {
                    location.reload();
                }
            });
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
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
                            BIA.Net.Dialog.AjaxLoading.ManageSubmitFormInDialog(currentDialogDiv, $(this));
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
var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
            ///let TestLink = new DialogLink($(document));
            var DialogLinkTarget;
            (function (DialogLinkTarget) {
                DialogLinkTarget[DialogLinkTarget["None"] = 0] = "None";
                DialogLinkTarget[DialogLinkTarget["Blank"] = 1] = "Blank";
                DialogLinkTarget[DialogLinkTarget["Current"] = 2] = "Current";
            })(DialogLinkTarget || (DialogLinkTarget = {}));
            var DialogLink = (function () {
                function DialogLink(linkElem, parent, url) {
                    this.linkElem = linkElem;
                    this.url = url;
                    this.parent = parent;
                    this.dialogDiv = null;
                    this.target = DialogLinkTarget.None;
                }
                DialogLink.PrepareLinkElement = function (linkElem, parent) {
                    var url = linkElem.attr('href');
                    var containLink = false;
                    if (url == null || url == "") {
                        var onclick = linkElem.attr('onclick');
                        if (onclick != null && onclick != "") {
                            var patt = [/location\.href\s*=\s*'(.*?)'/g, /location\.href\s*=\s*"(.*?)"/g, /window\.location\s*=\s*'(.*?)'/g, /window\.location\s*=\s*"(.*?)"/g];
                            for (var i = 0; i < patt.length; i++) {
                                var match = void 0;
                                while (match = patt[i].exec(onclick)) {
                                    url = match[1];
                                    if (url != null && url != "") {
                                        break;
                                    }
                                }
                                if (url != null && url != "") {
                                    break;
                                }
                            }
                            if (url != null && url != "") {
                                containLink = true;
                                linkElem.attr("onclick", "");
                            }
                        }
                    }
                    else {
                        //To avoid change on the this.url in case of use "mailto"
                        if ((url.substr(0, 7) != "mailto:") && (url.substr(0, 11) != "javascript:")) {
                            containLink = true;
                            linkElem.removeAttr("href");
                            linkElem.attr("style", "cursor:pointer");
                        }
                    }
                    //To avoid change on the this.url in case of use "mailto"
                    if (containLink) {
                        var DialogLink = new BIA.Net.Dialog.DialogLink(linkElem, parent, url);
                        linkElem.click(function (e) {
                            DialogLink.ActionClick();
                            e.stopPropagation();
                        });
                    }
                };
                DialogLink.prototype.ReplaceInRelatedDialog = function () {
                    this.dialogDiv.ReplaceInCurrentDialog(this.url, false);
                };
                DialogLink.prototype.PrepareDialogDiv = function () {
                    var parentDialogDiv = this.parent;
                    var parentViewPort = parentDialogDiv.GetViewPort();
                    var urlParent = this.parent.urlCurrent;
                    /*let similarUrls = BIA.Net.Dialog.DialogDiv.DialogManager.get(this.parent).SimilarReturnUrls;
                    if (similarUrls != null && similarUrls != "") {
                        urlParent = urlParent + ";" + similarUrls;
                    }*/
                    var attr = this.linkElem.attr("BIADialogLink");
                    var dialogDivType = Dialog.DialogDivType.Popup;
                    if (attr == null) {
                        if (this.parent.IsMainDialogDiv()) {
                            dialogDivType = Dialog.DialogDivType.MainPageContent;
                        }
                        else {
                            dialogDivType = Dialog.DialogDivType.Content;
                        }
                    }
                    var DivContent = "";
                    var DivScript = "";
                    var Modal = true;
                    var ratioW = 3 / 4;
                    var ratioH = 3 / 4;
                    var Height = parentViewPort.height() * ratioH;
                    var Width = parentViewPort.width() * ratioW;
                    var MinHeight = 300;
                    var MinWidth = 300;
                    if (attr != null) {
                        var MvcDialogLinkParams = attr.split(';');
                        for (var x = 0; x < MvcDialogLinkParams.length; x++) {
                            var d = MvcDialogLinkParams[x].split(":");
                            if (d.length == 2) {
                                var key = d[0].toLowerCase();
                                var val = d[1].toLowerCase();
                                if (key == "ratio") {
                                    ratioW = parseFloat(val);
                                    ratioH = parseFloat(val);
                                }
                                if (key == "ratiowidth") {
                                    ratioW = parseFloat(val);
                                }
                                if (key == "ratioheight") {
                                    ratioH = parseFloat(val);
                                }
                            }
                        }
                        Height = parentViewPort.height() * ratioH;
                        Width = parentViewPort.width() * ratioW;
                        MinHeight = 300;
                        MinWidth = 300;
                        var Id = "";
                        for (var x = 0; x < MvcDialogLinkParams.length; x++) {
                            var d = MvcDialogLinkParams[x].split(":");
                            if (d.length == 2) {
                                var key = d[0].toLowerCase();
                                var val = d[1].toLowerCase();
                                if (key == "id") {
                                    Id = val;
                                }
                                if (key == "type") {
                                    if (val == 'non-modal') {
                                        Modal = false;
                                    }
                                    if (val == 'content') {
                                        dialogDivType = Dialog.DialogDivType.Content;
                                    }
                                    if (val == 'mainpagecontent') {
                                        dialogDivType = Dialog.DialogDivType.MainPageContent;
                                    }
                                }
                                if (key == "divcontent") {
                                    DivContent = d[1];
                                }
                                if (key == "divscript") {
                                    DivScript = d[1];
                                }
                                if (key == "height") {
                                    Height = parseInt(val);
                                }
                                if (key == "width") {
                                    Width = parseInt(val);
                                }
                                if (key == "min-height") {
                                    MinHeight = parseInt(val);
                                }
                                if (key == "min-width") {
                                    MinWidth = parseInt(val);
                                }
                            }
                        }
                    }
                    if (Width < MinWidth)
                        Width = MinWidth;
                    if (Height < MinHeight)
                        Height = MinHeight;
                    if (Width > parentViewPort.width())
                        Width = parentViewPort.width();
                    if (Height > parentViewPort.height())
                        Height = parentViewPort.height();
                    if (dialogDivType == Dialog.DialogDivType.Popup) {
                        this.target = DialogLinkTarget.Blank;
                        var dialog = $('<div class="BiaNetDialogPopup BiaNetDialogDiv"></div>');
                        this.dialogDiv = new Dialog.DialogDiv(dialog, parentDialogDiv, dialogDivType);
                        var url_1 = this.url;
                        var dialogDiv_1 = this.dialogDiv;
                        dialog.dialog({
                            title: '',
                            autoOpen: false,
                            resizable: true,
                            height: Height,
                            width: Width,
                            show: { effect: 'drop', direction: "up" },
                            modal: Modal,
                            draggable: true,
                            open: function (event, ui) {
                                BIA.Net.Dialog.AjaxLoading.AjaxLoadDialog(dialogDiv_1, url_1, false);
                            },
                            resize: function (event, ui) {
                                dialogDiv_1.OnResizeDialog();
                            },
                            focus: function (event, ui) {
                                var dialogCurrent = $(this);
                                var childList = dialogCurrent.prop("dialogChildList");
                                if (childList != null) {
                                    childList.forEach(function (child) {
                                        if (!child[0].hidden) {
                                            child.dialog('moveToTop');
                                        }
                                    });
                                }
                            },
                            close: function (event, ui) {
                                dialogDiv_1.CleanDialog();
                            }
                        });
                        if (Id != "") {
                            dialog.prop("DialogByIds", Id);
                        }
                    }
                    else {
                        this.target = DialogLinkTarget.Current;
                        this.dialogDiv = Dialog.DialogDiv.PrepareContentDiv(parentDialogDiv, DivContent, DivScript, dialogDivType);
                    }
                };
                DialogLink.prototype.ActionClick = function () {
                    //let dialog = this.linkElem.prop("dialogDiv");
                    if (!this.dialogDiv) {
                        this.PrepareDialogDiv();
                    }
                    if (this.target == DialogLinkTarget.Blank) {
                        var parent_1 = this.parent.dialogElem;
                        var DialogByIds = this.dialogDiv.dialogElem.prop("DialogByIds");
                        if (DialogByIds != null && DialogByIds != "") {
                            var childList = parent_1.prop("dialogChildList");
                            if (childList != null) {
                                childList.forEach(function (child) {
                                    if (child.prop("DialogByIds") == DialogByIds) {
                                        if (child.dialog('isOpen')) {
                                            child.dialog('close');
                                        }
                                    }
                                });
                            }
                        }
                        this.dialogDiv.dialogElem.dialog('open');
                    }
                    else if (this.target == DialogLinkTarget.Current) {
                        this.dialogDiv.ReplaceInCurrentDialog(this.url, this.dialogDiv.IsStandardHistory());
                    }
                };
                return DialogLink;
            }());
            Dialog.DialogLink = DialogLink;
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
