var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var Dialog;
        (function (Dialog) {
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
        })(Dialog = Net.Dialog || (Net.Dialog = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
//# sourceMappingURL=Dialog.js.map