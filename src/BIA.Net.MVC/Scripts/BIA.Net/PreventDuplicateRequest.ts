module BIA.Net.MVC {
    export class PreventDuplicateRequest {
        public static Apply(form : JQuery) {
            var alreadySubmitted = false;
            return $(this).submit(function () {
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
    });
}