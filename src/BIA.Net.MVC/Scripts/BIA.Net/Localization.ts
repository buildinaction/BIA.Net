interface ResizeDialogInterface {
    (dialog: JQuery): any;
}
declare var ResizeDialog: ResizeDialogInterface;

module BIA.Net.MVC {
    export class Localisation {
        static cultureFormatDate: string;
        static cultureShort: string;
        //Manage Calendar format
        public static SetCalendarDatePicker(root) {
            if (root.find(".calendarPicker").length > 0) {
                root.find('.calendarPicker').datepicker({ format: Localisation.cultureFormatDate, language: Localisation.cultureShort });
            }
           
            if ($.validator != null) {
                $.validator.methods['date'] = function (value, element) {
                    var dpg = $.fn.datepicker.DPGlobal;
                    return this.optional(element) || dpg.parseDate(value, dpg.parseFormat(Localisation.cultureFormatDate)) !== null;
                }
            }
        }
        
        public static SetFloatValidator() {
            if ($.validator != null) {
                $.validator.methods['range'] = function (value, element, param) {
                    var globalizedValue = value.replace(",", ".");
                    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
                }

                $.validator.methods['number'] = function (value, element) {
                    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
                }
            }
        }
    }

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
}