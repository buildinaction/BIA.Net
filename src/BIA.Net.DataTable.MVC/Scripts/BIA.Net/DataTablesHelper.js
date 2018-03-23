var DataTableSettings = [];

function BIAInitFullAjaxDataTable(dataTableId, url_GetListData, columns, exportButtons, getDataExtend, url_OnClickRow) {

    let dom = 'lfrtip';

    if (exportButtons != null) {
        dom = 'Bfrtip';
    }

    $(window).on('OnBIADialogRefresh', function (e) {
        BIAReloadAjaxDataTable(dataTableId);
    });

    $(dataTableId).dataTable({
        "serverSide": true,
        "language": cultureDataTable,
        "dom": dom,
        "buttons": [{
            text: 'Export',
            action: function (e, dt, node, config) {
                var params = dt.ajax.params();
                window.location.href = url_GetListData + '?dataJson=' + JSON.stringify(params);
            }
        }],
        ajax: {
            url: url_GetListData,
            type: 'POST'
            ,
            "data": function (d) {
                return $.extend({}, d, getDataExtend());
            }
        },
        rowId: 'Id',
        "createdRow": function (row, data, dataIndex) {

            if (url_OnClickRow != null) {
                // event after the creation of a line. Allows you to add events
                $(row).css({ 'cursor': "pointer" });
                $(row).attr('BIADialogLink', "Type:non-modal;Id:" + data.Id);
                $(row).attr('onclick', "window.location='" + url_OnClickRow + "/" + data.Id + "'");
            }
        },
        "drawCallback": function (settings) {
            $(this).LinkToDialog();
            $(this).AddRefreshAction();
        },
        "columns": columns
    });
}

function BIAInitAjaxDataTable(dataTableId, url_GetListData, columns, exportButtons, formId, url_OnClickRow) {

    let dom = 'lfrtip';

    if (exportButtons != null) {
        dom = 'Bfrtip';
    }

    $(window).on('OnBIADialogRefresh', function (e) {
        BIAReloadAjaxDataTable(dataTableId);
    });

    $(dataTableId).dataTable({
        "language": cultureDataTable,
        "dom": dom,
        "buttons": exportButtons,
        ajax: {
            url: url_GetListData,
            "data": function (d) {
                // allows to push the filter data to the controller
                if (formId != null) {
                    return $(formId).serialize();
                }
            },
            "dataSrc": 'data'
        },
        rowId: 'Id',
        "createdRow": function (row, data, dataIndex) {

            if (url_OnClickRow != null) {
                // event after the creation of a line. Allows you to add events
                $(row).css({ 'cursor': "pointer" });
                $(row).attr('BIADialogLink', "Type:non-modal;Id:" + data.Id);
                $(row).attr('onclick', "window.location='" + url_OnClickRow + "/" + data.Id + "'");
            }
        },
        "drawCallback": function (settings) {
            $(this).LinkToDialog();
            $(this).AddRefreshAction();
        },
        "columns": columns
    });
}

function BIAReloadAjaxDataTable(dataTableId) {
    $(dataTableId).DataTable().ajax.reload(null, false);
}

// ExportButtons can be : ['copy', 'csv', 'excel', 'print'] or null
function BIAInitDataTable(dataTableId, exportButtons) {
    if (exportButtons == null) {
        $(document).ready(function () {
            $(dataTableId).DataTable({
                "language": cultureDataTable
            });
        });
    }
    else {
        $(document).ready(function () {
            $(dataTableId).DataTable({
                "language": cultureDataTable,
                "dom": 'Bfrtip',
                "buttons": exportButtons
            });
        });
    }




    DataTableSettings[dataTableId] = null;
    $(window).on('OnBIADialogRefreshing', function (e) {
        var dataTable = e.element.find(dataTableId);

        if (dataTable.length == 1) {
            var tableSettings = dataTable.dataTable().fnSettings();
            var pageLength = tableSettings._iDisplayLength;
            var displayStart = tableSettings._iDisplayStart;
            var search = tableSettings.oPreviousSearch.sSearch;
            var order = tableSettings.aaSorting;
            DataTableSettings[dataTableId] = { pageLength: pageLength, displayStart: displayStart, search: search, order: order };
        }
    });
    if (exportButtons == null) {
        $(window).on('OnBIADialogRefreshed', function (e) {
            var settings = DataTableSettings[dataTableId];
            e.element.find(dataTableId).each(function () {
                $(this).DataTable({
                    "language": cultureDataTable,
                    "pageLength": settings.pageLength,
                    "displayStart": settings.displayStart,
                    "search": { "search": settings.search },
                    "order": settings.order,
                    retrieve: true
                })
            });
        });
    }
    else {
        $(window).on('OnBIADialogRefreshed', function (e) {
            var settings = DataTableSettings[dataTableId];
            e.element.find(dataTableId).each(function () {
                $(this).DataTable({
                    "language": cultureDataTable,
                    "dom": 'Bfrtip',
                    "buttons": exportButtons,
                    "pageLength": settings.pageLength,
                    "displayStart": settings.displayStart,
                    "search": { "search": settings.search },
                    "order": settings.order,
                    retrieve: true
                })
            });
        });
    }
}