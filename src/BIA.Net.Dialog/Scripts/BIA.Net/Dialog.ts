module BIA.Net.Dialog {

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
}
