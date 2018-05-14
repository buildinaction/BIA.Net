
module BIA.Net.Dialog {
    var xhr;
    var _orgAjax = jQuery.ajaxSettings.xhr;
    jQuery.ajaxSettings.xhr = function () {
        xhr = _orgAjax();
        return xhr;
    };

    export class AjaxLoading {
        public static removeParam(keys, sourceURL) {
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
        }

        public static getResponseURL(xhr) {
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
        }

        public static aForAbsoluteURL = null;
        public static getAbsoluteUrl(url: string): string {
            AjaxLoading.aForAbsoluteURL = AjaxLoading.aForAbsoluteURL || document.createElement('a');
            AjaxLoading.aForAbsoluteURL.href = url;
            return AjaxLoading.aForAbsoluteURL.href;
        }

        public static UniformmizeUrl(url) {
            return AjaxLoading.removeParam(['bianetdialogurlparent', 'bianetdialogdisplayflag', 'bianetdialogredirectedurl'], AjaxLoading.getAbsoluteUrl(url).toLowerCase().replace(/#/g, "").replace(/\/$/, ""));
        }

        public static ManageSubmitFormInDialog(dialogDiv: DialogDiv, formElem: JQuery) {
            if ((!formElem.valid) || (formElem.data("validator") && formElem.data("validator").cancelSubmit) || (formElem.valid())) {
                //console.log("FormInDialog : submit");
                let url: string = formElem.attr('action');
                var dialog = dialogDiv.dialogElem;
                var parentUrl = dialogDiv.GetParentUrl();
                //console.log("FormInDialog : parentUrl ::" + parentUrl);
                formElem.append('<input type=\'hidden\' name=\'BIANetDialogDisplayFlag\' value=\'' + dialogDiv.type + '\' />');
                formElem.append('<input type=\'hidden\' name=\'BIANetDialogUrlParent\' value=\'' + parentUrl + '\' />');

                var dataToSend = new FormData(<HTMLFormElement>formElem[0]); // serializes the form's elements.
                var urlBefore = dialogDiv.urlCurrent;

                //console.log("FormInDialog : begin ajax");
                var ajaxSettings = {
                    type: "POST",
                    url: url,
                    urlBefore: urlBefore,
                    dialogDiv: dialogDiv,
                    data: dataToSend,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data: any, textStatus: string, xhr: XMLHttpRequest) {
                        DialogDiv.refreshIfRequiered(this.urlBefore);
                        AjaxLoading.SuccesAjaxReplaceInCurrentDialog(data, this.dialogDiv, this.url, this.url, AjaxLoading.getResponseURL(xhr), this.dialogDiv.IsStandardHistory());
                    },
                    error: function (xhr: XMLHttpRequest, textStatus: string, thrownError: string) {
                        AjaxLoading.ErrorAjaxReplaceInCurrentDialog(xhr.responseText, this.dialogDiv, this.url, this.url, AjaxLoading.getResponseURL(xhr), xhr.getResponseHeader('location'), this.dialogDiv.IsStandardHistory())
                    }
                };
                dialogDiv.dialogElem.append("<div id=\"divLoading\"></div>");
                dialogDiv.dialogElem.css("cursor", "progress");
                $.ajax(ajaxSettings);
            }
        }

        private static buildUrl(base: string, key: string, value: string): string {
            var sep = (base.indexOf('?') > -1) ? '&' : '?';
            return base + sep + key + '=' + value;
        };

        public static AjaxLoadDialog(dialogDiv: DialogDiv, url: string, addHistory: boolean) {
            var DialogType = dialogDiv.type;
            var url_timed = AjaxLoading.buildUrl(url, 'BIANetDialogDisplayFlag', DialogType.toString());
            url_timed = AjaxLoading.buildUrl(url_timed, 'BIANetDialogUrlParent', dialogDiv.GetParentUrl());

            var ajaxSettings = {
                url: url_timed,
                urlOrigin: url,
                dialogDiv: dialogDiv,
                cache: false,
                success: function (data: any, textStatus: string, xhr: XMLHttpRequest) {
                    //alert(xhr.getAllResponseHeaders());
                    AjaxLoading.SuccesAjaxReplaceInCurrentDialog(data, this.dialogDiv, this.urlOrigin, this.url, AjaxLoading.getResponseURL(xhr), addHistory);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    AjaxLoading.ErrorAjaxReplaceInCurrentDialog(xhr.responseText, this.dialogDiv, this.urlOrigin, this.url, AjaxLoading.getResponseURL(xhr), xhr.getResponseHeader('location'), addHistory)
                }
            };
            dialogDiv.dialogElem.append("<div id=\"divLoading\"></div>");
            dialogDiv.dialogElem.css("cursor", "progress");
            $.ajax(ajaxSettings);


        };

        public static SuccesAjaxReplaceInCurrentDialog(data: string, dialogDiv: DialogDiv, urlorigin: string, url: string, responseURL: string, addHistory: boolean) {
            //console.log("SuccesAjaxReplaceInCurrentDialog");
            if (data == "Close Dialog") {
                //console.log("close detected");
                //dialogDiv.dialogElem.html("");
                dialogDiv.dialogElem.dialog("close");
            }
            else if (data == "Close Parent Dialog") {
                //console.log("close detected");
                if (dialogDiv.parent.type == DialogDivType.Popup) {
                    dialogDiv.parent.dialogElem.dialog("close");
                }
                //dialogDiv.dialogElem.html("");
                //dialogDiv.dialogElem.dialog("close");
            }
            else if (data.indexOf("Action Dialog:")==0) {
                //console.log("close detected");
                var str = data.substr(14);
                var evt = $.Event('OnBIADialogAction');
                evt.dialogDiv = dialogDiv;
                evt.dialog = dialogDiv.dialogElem;
                evt.action = str;
                $(window).trigger(evt);
                //dialogDiv.dialogElem.html("");
                //dialogDiv.dialogElem.dialog("close");
            }
            else {
                /*dialogDiv.dialogElem.append("<div id=\"divLoading\"></div>");
                dialogDiv.dialogElem.css("cursor", "progress");*/
                dialogDiv.CleanDialog();
                let newUrl = "";
                if (responseURL && (AjaxLoading.UniformmizeUrl(responseURL) != AjaxLoading.UniformmizeUrl(url))) newUrl = AjaxLoading.UniformmizeUrl(responseURL)
                else newUrl = urlorigin;

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
            if (dialogDiv.dialogElem != null) dialogDiv.dialogElem.css("cursor", "default");
        }


        public static ErrorAjaxReplaceInCurrentDialog(data, dialogDiv: DialogDiv, urlorigin, url, responseURL, redirectURL, addHistory) {
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
        }
    }
    $(window).on("popstate", function () {
        if (history.state != null && history.state.ajaxUrl != null && history.state.ajaxUrl != "") {
            BIA.Net.Dialog.ChangeContent(BIA.Net.Dialog.DialogDiv.GetMainDiv(), false, history.state.ajaxUrl, history.state.ajaxDivContent, history.state.ajaxDivScript, history.state.ajaxDivType);
        }
        else {
            location.reload();
        }
    });
}