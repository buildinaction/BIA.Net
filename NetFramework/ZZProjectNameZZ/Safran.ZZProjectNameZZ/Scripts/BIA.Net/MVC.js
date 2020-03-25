var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var MVC;
        (function (MVC) {
            var Localisation = /** @class */ (function () {
                function Localisation() {
                }
                //Manage Calendar format
                Localisation.SetCalendarDatePicker = function (root) {
                    if (root.find(".calendarPicker").length > 0) {
                        root.find('.calendarPicker').datepicker({ format: Localisation.cultureFormatDate, language: Localisation.cultureShort });
                    }
                    if ($.validator != null) {
                        $.validator.methods['date'] = function (value, element) {
                            var dpg = $.fn.datepicker.DPGlobal;
                            return this.optional(element) || dpg.parseDate(value, dpg.parseFormat(Localisation.cultureFormatDate)) !== null;
                        };
                    }
                };
                Localisation.SetFloatValidator = function () {
                    if ($.validator != null) {
                        $.validator.methods['range'] = function (value, element, param) {
                            var globalizedValue = value.replace(",", ".");
                            return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
                        };
                        $.validator.methods['number'] = function (value, element) {
                            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
                        };
                    }
                };
                return Localisation;
            }());
            MVC.Localisation = Localisation;
            $(document).ready(function () {
                Localisation.SetCalendarDatePicker($(document));
                Localisation.SetFloatValidator();
                //Calendar format in Dialog
                $(window).on('OnBIADialogLoaded', function (e) {
                    Localisation.SetCalendarDatePicker(e.BIANetDialogData.dialogDiv.dialogElem);
                    Localisation.SetFloatValidator();
                });
                $(window).on('OnBIADialogResize', function (e) {
                    ResizeDialog(e.BIANetDialogData.dialogDiv.dialogElem);
                });
            });
        })(MVC = Net.MVC || (Net.MVC = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var MVC;
        (function (MVC) {
            var PreventDuplicateRequest = /** @class */ (function () {
                function PreventDuplicateRequest() {
                }
                PreventDuplicateRequest.Apply = function (form) {
                    var alreadySubmitted = false;
                    return form.submit(function () {
                        if (alreadySubmitted)
                            return false;
                        else {
                            if ((!$(this).valid) || $(this).valid()) {
                                alreadySubmitted = true;
                            }
                        }
                    });
                };
                return PreventDuplicateRequest;
            }());
            MVC.PreventDuplicateRequest = PreventDuplicateRequest;
            $(document).ready(function ($) {
                PreventDuplicateRequest.Apply($('.PreventDuplicateRequest'));
                $(window).on('OnBIADialogLoaded', function (e) {
                    PreventDuplicateRequest.Apply(e.BIANetDialogData.dialogDiv.dialogElem.find('.PreventDuplicateRequest'));
                });
            });
        })(MVC = Net.MVC || (Net.MVC = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
//# sourceMappingURL=MVC.js.map