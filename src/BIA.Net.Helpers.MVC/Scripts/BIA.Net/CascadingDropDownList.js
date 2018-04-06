$.fn.cascadingDropDownList = function () {
    var prefixDataAttr = "data-";
    var prefixIdAttr = "id-";
    var separator = "|";
    function ManageOption(parentId, childList) {
        var childOptions = $(childList).children();
        var nbOptionShow = 0;
        var parentSelectedValue = $("#" + parentId).val();
        if (parentSelectedValue != "") {
            childOptions.each(function () {
                // if attribute contains the selectedValue
                if ($(this).attr(prefixDataAttr + parentId) != null && $(this).attr(prefixDataAttr + parentId).split(separator).some(function (x) { return x === parentSelectedValue; })) {
                    // $(this).show(); does not work on IE
                    nbOptionShow = nbOptionShow + 1;
                }
                else {
                    // $(this).hide(); does not work on IE
                    $(this).remove();
                }
            });
        }
        // if the list does not contain any options, then it is disabled. Otherwise, we activate it
        $(childList).prop("disabled", !$(childList).val() && nbOptionShow < 1);
    }
    if ($(this).is("select")) {
        // contains all the id of the parent select of the current select
        var parentIds_1 = [];
        // contains the options of the current select
        var childOptions = $(this).children();
        // we store in a variable the options in html format to be able to rebuild our list after some options have been removed
        var childOptionsHtml_1 = $(this).html();
        // the current select
        var childList_1 = this[0];
        // if the current select has no option selected
        if (!$(this).val()) {
            // We go through all the selects that have a selected option
            $("select option:selected").each(function () {
                // If this select has for parent the current select
                if ($(this).val() != "" && $(this).attr(prefixIdAttr + childList_1.id) != null) {
                    // We change the value of the current list
                    var data = $(this).attr(prefixDataAttr + childList_1.id);
                    $("#" + childList_1.id).val(data);
                }
            });
        }
        // Here, we retrieve all the ids of the parents
        childOptions.each(function () {
            $.each(this.attributes, function () {
                if (this.specified && this.name.indexOf(prefixIdAttr) == 0) {
                    parentIds_1.push(this.value);
                }
            });
        });
        // remove duplicate value
        parentIds_1 = $.unique(parentIds_1);
        parentIds_1.forEach(function (parentId) {
            // we check that it is a select.
            if ($("#" + parentId).is("select")) {
                $("#" + parentId).change(function () {
                    // we delete all the options and we recreate them with the html stored previously.
                    $(childList_1).empty();
                    $(childList_1).append(childOptionsHtml_1);
                    $(childList_1).val("");
                    ManageOption(parentId, childList_1);
                    // we launch the event changes from the list to launch the recursive side.
                    $(childList_1).change();
                });
                ManageOption(parentId, childList_1);
            }
        });
    }
};
