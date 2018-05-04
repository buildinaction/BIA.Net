module BIA.Net.Dialog {

    ///let TestLink = new DialogLink($(document));

    enum DialogLinkTarget {
        None = 0,
        Blank = 1,
        Current = 2
    }

    export class DialogLink {
        linkElem: JQuery;
        url: string;
        dialogDiv: DialogDiv;
        parent: DialogDiv;
        private target: DialogLinkTarget;
        constructor(linkElem: JQuery, parent: DialogDiv, url: string) {
            this.linkElem = linkElem;
            this.url = url;
            this.parent = parent;
            this.dialogDiv = null;
            this.target = DialogLinkTarget.None;
        }


        public static PrepareLinkElement(linkElem: JQuery, parent: DialogDiv) {
            var url = linkElem.attr('href');
            let containLink: boolean = false;
            if (url == null || url == "") {
                var onclick = linkElem.attr('onclick');
                if (onclick != null && onclick != "") {
                    var patt = [/location\.href\s*=\s*'(.*?)'/g, /location\.href\s*=\s*"(.*?)"/g, /window\.location\s*=\s*'(.*?)'/g, /window\.location\s*=\s*"(.*?)"/g];
                    for (var i = 0; i < patt.length; i++) {
                        let match;
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
        }

        public ReplaceInRelatedDialog() {
            this.dialogDiv.ReplaceInCurrentDialog(this.url, false);
        }


        public PrepareDialogDiv() {
            const parentDialogDiv = this.parent;
            const parentViewPort = parentDialogDiv.GetViewPort();
            let urlParent = this.parent.urlCurrent;

            /*let similarUrls = BIA.Net.Dialog.DialogDiv.DialogManager.get(this.parent).SimilarReturnUrls;
            if (similarUrls != null && similarUrls != "") {
                urlParent = urlParent + ";" + similarUrls;
            }*/
            let attr = this.linkElem.attr("BIADialogLink");
            let dialogDivType: DialogDivType = DialogDivType.Popup;
            if (attr == null) {
                if (this.parent.IsMainDialogDiv()) {
                    dialogDivType = DialogDivType.MainPageContent;
                }
                else {
                    dialogDivType = DialogDivType.Content;
                }
            }
            let DivContent = "";
            let DivScript = "";
            let Modal = true;
            let ratioW = 3 / 4;
            let ratioH = 3 / 4;
            let Height = parentViewPort.height() * ratioH;
            let Width = parentViewPort.width() * ratioW;
            let MinHeight = 300;
            let MinWidth = 300;
            if (attr != null) {
                let MvcDialogLinkParams = attr.split(';');

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
                                dialogDivType = DialogDivType.Content;
                            }
                            if (val == 'mainpagecontent') {
                                dialogDivType = DialogDivType.MainPageContent;
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

            if (dialogDivType == DialogDivType.Popup) {
                this.target = DialogLinkTarget.Blank;
                let dialog: any = $('<div class="BiaNetDialogPopup BiaNetDialogDiv"></div>');
                this.dialogDiv = new DialogDiv(dialog, parentDialogDiv, dialogDivType);
                let url = this.url;
                let dialogDiv = this.dialogDiv;
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
                        BIA.Net.Dialog.AjaxLoading.AjaxLoadDialog(dialogDiv, url, false);
                    },
                    resize: function (event, ui) {
                        dialogDiv.OnResizeDialog();
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
                        dialogDiv.CleanDialog();
                    }

                });
                if (Id != "") {
                    dialog.prop("DialogByIds", Id);
                }
                /*dialog.on('dialogclose', function (event) {
                    var dialogCurrent = $(this);

                    DisposeDialog(dialogCurrent);
                });*/
            }
            else {
                this.target = DialogLinkTarget.Current;
                this.dialogDiv = DialogDiv.PrepareContentDiv(parentDialogDiv, DivContent, DivScript, dialogDivType)
            }
        }


        public ActionClick() {
            //let dialog = this.linkElem.prop("dialogDiv");

            if (!this.dialogDiv) {
                this.PrepareDialogDiv()
            }
            if (this.target == DialogLinkTarget.Blank) {
                const parent = this.parent.dialogElem;
                var DialogByIds = this.dialogDiv.dialogElem.prop("DialogByIds");
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
                this.dialogDiv.dialogElem.dialog('open');
            }
            else if (this.target == DialogLinkTarget.Current) {
                this.dialogDiv.ReplaceInCurrentDialog(this.url, this.dialogDiv.IsStandardHistory());
                //DialogDiv.ChangeContent(parentDialogDiv, true, this.url, DivContent, DivScript);
            }
        }
    }
}
