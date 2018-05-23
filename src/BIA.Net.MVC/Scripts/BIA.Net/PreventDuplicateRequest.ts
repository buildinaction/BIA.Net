module BIA.Net.MVC {
    export class PreventDuplicateRequest {
        public static Apply(form : JQuery) {
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
        }
    }
    $(document).ready(function ($) {
        PreventDuplicateRequest.Apply($('.PreventDuplicateRequest'));
        $(window).on('OnBIADialogLoaded', function (e) {
            PreventDuplicateRequest.Apply(e.BIANetDialogData.dialogDiv.dialogElem.find('.PreventDuplicateRequest'));
        });
    });
}