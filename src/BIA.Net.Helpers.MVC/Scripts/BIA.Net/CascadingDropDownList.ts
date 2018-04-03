$.fn.cascadingDropDownList = function () {

    const prefixDataAttr: string = "data-";
    const prefixIdAttr: string = "id-";

    function ManageOption(parentId: Element, childList: any) {

        const childOptions: JQuery = $(childList).children();

        let nbOptionShow: number = 0;
        const parentSelectedValue: string = $("#" + parentId).val();

        if (parentSelectedValue != "") {
            childOptions.each(function () {
                // if attribute = the selectedValue
                if ($(this).attr(prefixDataAttr + parentId) != null && $(this).attr(prefixDataAttr + parentId) === parentSelectedValue) {
                    // $(this).show(); does not work on IE
                    nbOptionShow = nbOptionShow + 1;
                } else {
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
        let parentIds: Element[] = [];
        // contains the options of the current select
        const childOptions: JQuery = $(this).children();
        // we store in a variable the options in html format to be able to rebuild our list after some options have been removed
        const childOptionsHtml: string = $(this).html();
        // the current select
        const childList = this[0];

        // if the current select has no option selected
        if (!$(this).val()) {
            // We go through all the selects that have a selected option
            $("select option:selected").each(function () {
                // If this select has for parent the current select
                if ($(this).val() != "" && $(this).attr(prefixIdAttr + childList.id) != null) {
                    // We change the value of the current list
                    const data: string = $(this).attr(prefixDataAttr + childList.id);
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
}