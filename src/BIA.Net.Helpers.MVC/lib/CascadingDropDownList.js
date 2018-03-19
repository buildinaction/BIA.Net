"use strict";
$.fn.cascadingDropDownList = function () {
    let prefixDataAttr = "data-";
    let prefixIdAttr = "id-";
    function ManageOption(parentId, childList) {
        childOptions = $(childList).children();
        let nbOptionShow = 0;
        let parentSelectedValue = $("#" + parentId).val();
        if (parentSelectedValue != "") {
            childOptions.each(function () {
                // if attrbute contains the selectedValue
                if ($(this).attr(prefixDataAttr + parentId) != null && $(this).attr(prefixDataAttr + parentId).indexOf(parentSelectedValue) >= 0) {
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
        $(childList).prop("disabled", nbOptionShow < 1);
    }
    $(this).each(function () {
        if ($(this).is("select")) {
            // contains all the id of the parent select of the current select
            let parentIds = [];
            // contains the options of the current select
            let childOptions = $(this).children();
            // we store in a variable the options in html format to be able to rebuild our list after some options have been removed
            let childOptionsHtml = $(this).html();
            // the current select
            let childList = this;
            // if the current select has no option selected
            if (!$(this).val()) {
                // We go through all the selects that have a selected option
                $("select option:selected").each(function () {
                    // If this select has for parent the current select
                    if ($(this).val() != "" && $(this).attr(prefixIdAttr + childList.id) != null) {
                        // We change the value of the current list
                        let data = $(this).attr(prefixDataAttr + childList.id);
                        $("#" + childList.id).val(data);
                    }
                });
            }
            // Here, we retrieve all the ids of the parents
            childOptions.each(function () {
                $.each(this.attributes, function () {
                    if (this.specified && this.name.indexOf(prefixIdAttr) == 0) {
                        parentIds.push(this.value);
                    }
                });
            });
            // remove duplicate value
            parentIds = $.unique(parentIds);
            parentIds.forEach(function (parentId) {
                // we check that it is a select.
                if ($("#" + parentId).is("select")) {
                    $("#" + parentId).change(function () {
                        // we delete all the options and we recreate them with the html stored previously.
                        $(childList).empty();
                        $(childList).append(childOptionsHtml);
                        $(childList).val("");
                        ManageOption(parentId, childList);
                        // we launch the event changes from the list to launch the recursive side.
                        $(childList).change();
                    });
                    ManageOption(parentId, childList);
                }
            });
        }
    });
};
