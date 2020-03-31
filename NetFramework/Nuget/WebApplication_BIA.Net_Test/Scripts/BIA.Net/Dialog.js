$.fn.submitNoValidation = function (e) {
    $(this).removeData('validator');
    $(this).removeData('unobtrusiveValidation');
    $(this).validate().cancelSubmit = true;
    $(this).submit();
    //e.preventDefault();
    //return false;
}

var buildUrl = function (base, key, value) {
    var sep = (base.indexOf('?') > -1) ? '&' : '?';
    return base + sep + key + '=' + value;
}

var getAbsoluteUrl = (function () {
    var a = null;
    return function (url) {
        a = a || document.createElement('a');
        a.href = url;

        return a.href;
    };
})();

var scrollBarWidth = -1;
function getScrollBarWidth() {
    if (scrollBarWidth == -1) {
        var $outer = $('<div>').css({ visibility: 'hidden', width: 100, overflow: 'scroll' }).appendTo('body'),
            widthWithScroll = $('<div>').css({ width: '100%' }).appendTo($outer).outerWidth();
        $outer.remove();
        scrollBarWidth = 100 - widthWithScroll;
    }
    return scrollBarWidth;
};

var scrollBarHeight = -1;
function getScrollBarHeight() {
    if (scrollBarHeight == -1) {
        var $outer = $('<div>').css({ visibility: 'hidden', height: 100, overflow: 'scroll' }).appendTo('body'),
            widthHeightScroll = $('<div>').css({ height: '100%' }).appendTo($outer).outerHeight();
        $outer.remove();
        scrollBarHeight = 100 - widthHeightScroll;
    }
    return scrollBarHeight;
};



function OnResizeDialog(dialog) {
    var evt = $.Event('OnBIADialogResize');
    evt.dialog = dialog;
    $(window).trigger(evt);
}

function GetWindowParent(linkElem) {
    var dialog = linkElem.closest("#dialog");
    if (dialog == null || dialog.length == 0) return $(document);
    return dialog
}

function GetParentViewPort(linkElem) {
    var dialog = linkElem.closest("#dialog");
    if (dialog == null || dialog.length == 0) return $(window);
    return dialog
}

function GetUrlParent(linkElem) {
    var dialog = linkElem.closest("#dialog");
    return dialog.prop("dialogUrlParent")
}

function GetUrlCurrent(linkElem) {
    var dialog = linkElem.closest("#dialog");
    if (dialog == null || dialog.length == 0) return window.location.href.replace('#', '');
    else return dialog.prop("dialogUrlCurrent")
}


function AddDialogToChildList(linkElem, child) {
    var dialog = GetWindowParent(linkElem);
    if (dialog != null && dialog.length > 0) {
        var childList = dialog.prop("dialogChildList");
        if (childList == null) {
            childList = [];
        }
        childList.push(child);
        dialog.prop("dialogChildList", childList);
    }
}

function SuccesAjaxReplaceInCurrentDialog(data, dialog, urlorigin, url, responseURL) {
    console.log("SuccesAjaxReplaceInCurrentDialog");
    if (data == "Close Dialog") {
        console.log("close detected");
        dialog.html("");
        dialog.dialog("close");
    }
    else {
        dialog.CleanRefreshAction()
        if (responseURL && (UniformmizeUrl(responseURL) != UniformmizeUrl(url))) dialog.prop("dialogUrlCurrent", UniformmizeUrl(responseURL))
        else dialog.prop("dialogUrlCurrent", urlorigin)
        dialog.html(data);
        OnDialogLoaded(dialog);
    }
}

var xhr;
var _orgAjax = jQuery.ajaxSettings.xhr;
jQuery.ajaxSettings.xhr = function () {
    xhr = _orgAjax();
    return xhr;
};

function ManageSubmitFormInDailog(formElem) {
    if ((!formElem.valid) || (formElem.data("validator") && formElem.data("validator").cancelSubmit) || (formElem.valid())) {
        console.log("FormInDialog : submit");
        var url = formElem.attr('action');
        dialog = formElem.closest("#dialog");
        var dataToSend = formElem.serialize();// serializes the form's elements.
        var urlBefore = GetUrlCurrent(formElem);

        dialog.html("Loading ...");
        console.log("FormInDialog : begin ajax");
        $.ajax({
            type: "POST",
            url: url,
            urlBefore: urlBefore,
            dialog: dialog,
            data: dataToSend,
            success: function (data) {
                console.log("FormInDialog : success");
                var redirectedURL = xhr.getResponseHeader("BIARedirectedDialogUrl");
                refreshIfRequiered(this.urlBefore);
                SuccesAjaxReplaceInCurrentDialog(data, this.dialog, this.url, this.url, redirectedURL)
            }
        });
    }
}

HTMLFormElement.prototype.originalsbmit = HTMLFormElement.prototype.submit;
HTMLFormElement.prototype.submit = function () {
    if ($(this).prop('BIA.Net.MVC.FormInDialog')) {
        //to manage javascript submit (this.form.submit())
        ManageSubmitFormInDailog($(this));
    }
    else {
        this.originalsbmit();
    }
}

$.fn.FormInDialog = function () {
    $(this).find('form').each(function () {
        var formElem = $(this);
        formElem.append('<input type=\'hidden\' name=\'displayFlag\' value=\'1\' />');
        var parentUrl = GetUrlParent(formElem);
        console.log("FormInDialog : parentUrl ::" + parentUrl);
        formElem.append('<input type=\'hidden\' name=\'dialogUrlParent\' value=\'' + parentUrl + '\' />');
        formElem.prop('BIA.Net.MVC.FormInDialog', true);

        // this is the id of the form
        formElem.submit(function (e) {
            e.preventDefault();    //stop form from submitting
            ManageSubmitFormInDailog($(this));
        });
    });
};

function removeParam(keys, sourceURL) {
    var rtn = sourceURL.split("?")[0],
        param,
        params_arr = [],
        queryString = (sourceURL.indexOf("?") !== -1) ? sourceURL.split("?")[1] : "";
    if (queryString !== "") {
        params_arr = queryString.split("&");
        for (var i = params_arr.length - 1; i >= 0; i -= 1) {
            param = params_arr[i].split("=")[0];
            if (keys.indexOf(param.toLowerCase()) !== -1) {
                params_arr.splice(i, 1);
            }
        }
        if (params_arr.length > 0) rtn = rtn + "?" + params_arr.join("&");
    }
    return rtn;
}

function UniformmizeUrl(url) {
    return removeParam(['dialogrefreshtimestamp', 'dialogurlparent', 'displayflag'], getAbsoluteUrl(url).toLowerCase().replace(/#/g, "").replace(/\/$/, ""));
}

function RefreshCurrentDialog(linkElem) {
    var dialog = linkElem.closest("#dialog");
    if (dialog == null || dialog.length == 0) location.reload();
    else {
        var url = dialog.prop("dialogUrlCurrent");
        ReplaceInCurrentDialog(linkElem, url)
    }
}

function ReplaceInCurrentDialog(linkElem, url) {
    var dialog = linkElem.closest("#dialog");
    var urlsParent = GetUrlParent(linkElem);
    var aUrlsParent = urlsParent.split(';');
    var urlToTest = UniformmizeUrl(url);
    for (var i = 0 ; i < aUrlsParent.length; i++) {
        if (urlToTest == UniformmizeUrl(aUrlsParent[i])) {
            dialog.dialog("close");
            return;
        }
    }

    var url_timed = buildUrl(url, 'displayFlag', '1');
    url_timed = buildUrl(url_timed, 'dialogUrlParent', GetUrlParent(linkElem));
    url_timed = buildUrl(url_timed, 'dialogRefreshTimeStamp', new Date().getTime());
    dialog.html("Loading ...");
    $.ajax({
        url: url_timed,
        urlOrigin: url,
        dialog: dialog,
        success: function (data) {
            SuccesAjaxReplaceInCurrentDialog(data, this.dialog, this.urlOrigin, this.url, xhr.responseURL)
        }
    });
}

function ExctractUrlAndCleanElem(ElemToTest, FuncToCall) {
    var url = ElemToTest.attr('href');
    if (url == null || url == "") {
        var onclick = ElemToTest.attr('onclick');
        if (onclick != null && onclick != "") {
            var patt = [/location\.href\s*=\s*'(.*?)'/g, /location\.href\s*=\s*"(.*?)"/g, /window\.location\s*=\s*'(.*?)'/g, /window\.location\s*=\s*"(.*?)"/g];
            for (var i = 0; i < patt.length; i++) {
                while (match = patt[i].exec(onclick)) {
                    url = match[1];
                    if (url != null && url != "") { break; }
                }
                if (url != null && url != "") { break; }
            }
            if (url != null && url != "") {
                ElemToTest.attr("onclick", "");
            }
        }
    }
    else {
        //To avoid change on the url in case of use "mailto"
        if (url.substr(0, 7) != "mailto:") {
            ElemToTest.removeAttr("href");
            ElemToTest.attr("style", "cursor:pointer");
        }
    }
    //To avoid change on the url in case of use "mailto"
    if (url != null && url != "" && url.substr(0, 7) != "mailto:") {
        ElemToTest.prop("dialogUrl", url);
        ElemToTest.prop("dialogFuncToCall", FuncToCall);
        //ElemToTest.attr("onclick", FuncToCall + "($(this), '" + url + "');");
        ElemToTest.click(function (e) {
            var ElemToTest = $(e.currentTarget);
            var url = ElemToTest.prop("dialogUrl");
            var FuncToCall = ElemToTest.prop("dialogFuncToCall");
            //alert("click: " + url);
            window[FuncToCall](ElemToTest, url);
            e.stopPropagation();

        });
    }
    return url;
}

$.fn.LinkInDialog = function () {
    $(this).find('[href],[onclick]').not('[BIADialogLink]').not('[target]')
        .each(function (e) {
            var url = ExctractUrlAndCleanElem($(this), "ReplaceInCurrentDialog");
        })
};


function OnDialogLoaded(dialog) {
    dialog.LinkInDialog();
    dialog.FormInDialog();
    dialog.LinkToDialog();
    OnResizeDialog(dialog);
    dialog.AddRefreshAction();

    var evt = $.Event('OnBIADialogLoaded');
    evt.dialog = dialog;
    $(window).trigger(evt);
}

function OpenDialog(linkElem) {
    linkElem.prop("dialogDiv").dialog('open');
}



function OpenDialog(linkElem, url) {
    var dialog = linkElem.prop("dialogDiv");
    var parent = GetWindowParent(linkElem);
    var parentViewPort = GetParentViewPort(linkElem);
    if (!dialog) {
        urlParent = GetUrlCurrent(linkElem);

        var similarUrls = DialogManager.get(parent).SimilarReturnUrls;
        if (similarUrls != null && similarUrls != "") {
            urlParent = urlParent + ";" + similarUrls
        }


        var MvcDialogLinkParams = linkElem.attr("BIADialogLink").split(';');
        var Modal = true;
        var ratioW = 3 / 4;
        var ratioH = 3 / 4;
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
        var Height = parentViewPort.height() * ratioH;
        var Width = parentViewPort.width() * ratioW;
        var MinHeight = 300;
        var MinWidth = 300;
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
        if (Width < MinWidth) Width = MinWidth;
        if (Height < MinHeight) Height = MinHeight;


        if (Width > parentViewPort.width()) Width = parentViewPort.width();
        if (Height > parentViewPort.height()) Height = parentViewPort.height();


        dialog = $('<div id="dialog">Loading ...</div>');
        dialog.prop("dialogUrlParent", urlParent);
        linkElem.prop("dialogDiv", dialog);
        //alert($(this).prop("dialogDiv"));
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
                var dialogCurrent = $(this);
                dialogCurrent.prop("dialogUrlCurrent", url)
                var url_timed = buildUrl(url, 'displayFlag', '1');
                url_timed = buildUrl(url_timed, 'dialogUrlParent', urlParent);
                url_timed = buildUrl(url_timed, 'dialogRefreshTimeStamp', new Date().getTime());
                $(this).load(url_timed, function () {
                    OnDialogLoaded(dialogCurrent);
                });
            },
            resize: function (event, ui) {
                var dialogCurrent = $(this);
                OnResizeDialog(dialogCurrent);
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
            }
        });
        if (Id != "") {
            dialog.prop("DialogByIds", Id)
        }
        AddDialogToChildList(linkElem, dialog);
        dialog.on('dialogclose', function (event) {
            var dialogCurrent = $(this);
            dialogCurrent.CleanRefreshAction()
            dialogCurrent.html("Loading ...");
            var childList = dialogCurrent.prop("dialogChildList");
            if (childList != null) {
                childList.forEach(function (entry) {
                    entry.remove();
                });
            }
            dialogCurrent.prop("dialogChildList", []);
        });
    }
    var DialogByIds = dialog.prop("DialogByIds");
    if (DialogByIds != null && DialogByIds != "") {
        var childList = parent.prop("dialogChildList");
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

    dialog.dialog('open');
}

$.fn.LinkToDialog = function () {
    $(this).find('[BIADialogLink]')
        .each(function (e) {
            var url = ExctractUrlAndCleanElem($(this), "OpenDialog");
            //$(this).attr("onclick", "OpenDialog($(this))");
        });
};

function HashTableHtmlElem() {
    this.hashes = {},
    this.id = 0;
}

HashTableHtmlElem.prototype = {
    constructor: HashTableHtmlElem,

    put: function (obj, value) {
        obj.attr('__id__', this.id);
        this.hashes[this.id] = value;
        this.id++;
    },

    get: function (obj) {
        return this.hashes[obj.attr('__id__')];
    },
    remove: function (obj) {
        delete this.hashes[obj.attr('__id__')];
    },
};


var DialogManager = new HashTableHtmlElem();
//var RefrechAction = new HashTableHtmlElem();
//var hashDialogSimilarReturnUrls = new HashTableHtmlElem();

$.fn.AddRefreshAction = function () {
    var CurrentDialog = $(this);
    var RefrechActionCurrentDialog = [];
    $(this).find('[BIADialogRefresh]')
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
            $(this).prop("BIADialogRefreshUrl", dialogRefreshUrl);
            $(this).prop("BIADialogRefreshFormId", dialogRefreshFormId);
            $(this).prop("BIADialogRefreshOnlyEvent", dialogRefreshOnlyEvent);
            $(this).prop("BIADialogRefreshParent", CurrentDialog);
            $(this).prop("BIADialogRefreshFormsUrls", onFormValidated);

            RefrechActionCurrentDialog.push({ ElemToRefresh: $(this) });
        });
    //RefrechAction.put(CurrentDialog, RefrechActionCurrentDialog);

    var DialogSimilarReturnUrlsCurrentDialog = [];
    $(this).find('[BIADialogSimilarReturnUrls]')
        .each(function (e) {
            var SimilarReturnUrls = $(this).attr('BIADialogSimilarReturnUrls').split(';');
            DialogSimilarReturnUrlsCurrentDialog = DialogSimilarReturnUrlsCurrentDialog.concat(SimilarReturnUrls);;
        });
    //hashDialogSimilarReturnUrls.put(CurrentDialog, DialogSimilarReturnUrlsCurrentDialog);

    DialogManager.put(CurrentDialog, { RefrechAction: RefrechActionCurrentDialog, SimilarReturnUrls: DialogSimilarReturnUrlsCurrentDialog });
}

$.fn.CleanRefreshAction = function () {
    DialogManager.remove($(this));
}

String.prototype.startsWith = function (needle) {
    return (this.indexOf(needle) == 0);
};

function refreshIfRequiered(urlValidated) {
    urlValidated = urlValidated.toLowerCase();
    console.log("refreshIfRequiered");
    Object.keys(DialogManager.hashes).forEach(function (key) {
        var RefrechActionDialog = DialogManager.hashes[key].RefrechAction;
        for (var i = 0; i < RefrechActionDialog.length; i++) {
            var elemToRefresh = RefrechActionDialog[i].ElemToRefresh;
            var FormsUrls = elemToRefresh.prop("BIADialogRefreshFormsUrls");
            for (var j = 0; j < FormsUrls.length; j++) {
                if (urlValidated.indexOf(FormsUrls[j]) >= 0) {
                    elemToRefresh.BIADialogRefreshContent();
                    break;
                }
            }
        }
    });
}

$.fn.BIADialogRefreshContent = function () {
    var formElem = null;
    var dataToSend = null;
    var formId = $(this).prop("BIADialogRefreshFormId");
    if (formId != null && formId != "") {
        console.log("BIADialogRefresh : Searh form to refresh : " + formId);
        formElem = $(formId);
        if (formElem.length == 0) formElem = null;

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

    var refreshUrl = $(this).prop("BIADialogRefreshUrl");
    if (refreshUrl == null || refreshUrl == "") {
        if (formElem != null) {
            refreshUrl = formElem.attr('action');
        }
        else {
            refreshUrl = GetUrlCurrent($(this));
        }
    }
    var ParentDialog = $(this).prop("BIADialogRefreshParent");

    var onlyEvent = $(this).prop("BIADialogRefreshOnlyEvent");
    if (onlyEvent) {
        var evt = $.Event('OnBIADialogRefresh');
        evt.dialog = ParentDialog;
        evt.element = $(this);
        $(window).trigger(evt);
    }
    else {
        var evt = $.Event('OnBIADialogRefreshing');
        evt.dialog = ParentDialog;
        evt.element = $(this);
        $(window).trigger(evt);

        $.ajax({
            type: "POST",
            url: refreshUrl,
            data: dataToSend,
            ParentDialog: ParentDialog,
            ElemToRefresh: $(this),
            success: function (data) {
                this.ParentDialog.CleanRefreshAction()
                this.ElemToRefresh.html(data);
                this.ElemToRefresh.LinkInDialog();
                this.ElemToRefresh.FormInDialog();
                this.ElemToRefresh.LinkToDialog();
                this.ParentDialog.AddRefreshAction();
                var evt = $.Event('OnBIADialogRefreshed');
                evt.dialog = this.ParentDialog;
                evt.element = this.ElemToRefresh;
                $(window).trigger(evt);
            },
            error: function (error) {
                console.log("Ajax Error " + error.status + " when calling : " + refreshUrl);
            }
        });
    }
};

$(document).ready(function () {
    $(this).LinkToDialog();
    $(this).AddRefreshAction();
});