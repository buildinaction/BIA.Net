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
                                linkElem.attr("onclick", "");
                            }
                        }
                    }
                    else {
                        //To avoid change on the this.url in case of use "mailto"
                        if (url.substr(0, 7) != "mailto:") {
                            linkElem.removeAttr("href");
                            linkElem.attr("style", "cursor:pointer");
                        }
                    }
                    //To avoid change on the this.url in case of use "mailto"
                    if (url != null && url != "" && url.substr(0, 7) != "mailto:") {
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
                        var dialog = $('<div class="BiaNetDialogPopup BiaNetDialogDiv">Loading ...</div>');
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
//# sourceMappingURL=DialogLink.js.map