$.fn.PreventDuplicateRequest = function () {
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
};
$(document).ready(function ($) {
    $('.PreventDuplicateRequest').PreventDuplicateRequest();
});
