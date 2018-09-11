var BIA;
(function (BIA) {
    var Net;
    (function (Net) {
        var DataTable;
        (function (DataTable) {
            var DataTableSettings = [];
            DataTable.cultureDataTable = {
                emptyTable: "No data available in table",
                info: "Showing _START_ to _END_ of _TOTAL_ entries",
                infoEmpty: "Showing 0 to 0 of 0 entries",
                infoFiltered: "(filtered from _MAX_ total entries)",
                infoPostFix: "",
                thousands: ",",
                lengthMenu: "Show _MENU_ entries",
                loadingRecords: "Loading...",
                processing: "Processing...",
                search: "Search:",
                zeroRecords: "No matching records found",
                paginate: {
                    first: "First",
                    last: "Last",
                    next: "Next",
                    previous: "Previous"
                },
                aria: {
                    sortAscending: ": activate to sort column ascending",
                    sortDescending: ": activate to sort column descending"
                }
            };
            function InitFullAjax(dataTableId, url_GetListData, columns, displayExportButton, getDataExtend, url_OnClickRow, rowCallback) {
                var dom = 'lfrtip';
                var button = null;
                if (displayExportButton === true) {
                    dom = 'Bfrtip';
                    button = [{
                            text: 'Export',
                            action: function (e, dt, node, config) {
                                var params = dt.ajax.params();
                                window.location.href = url_GetListData + '?dataJson=' + JSON.stringify(params).replace(',"search":{"value":"","regex":false}', ''); // replace otherwise url too long for a GET
                            }
                        }];
                }
                $(window).on('OnBIADialogRefresh', function (e) {
                    ReloadAjax(dataTableId);
                });
                var dataTableOption = {
                    "serverSide": true,
                    "language": DataTable.cultureDataTable,
                    "dom": dom,
                    "buttons": button,
                    ajax: {
                        url: url_GetListData,
                        type: 'POST',
                        "data": function (d) {
                            if (getDataExtend != null) {
                                return $.extend({}, d, getDataExtend());
                            }
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
                        BIA.Net.Dialog.LinkToDialog($(this));
                        BIA.Net.Dialog.AddRefreshAction($(this));
                    },
                    "columns": columns,
                    "rowCallback": rowCallback
                };
                $(dataTableId).dataTable(dataTableOption);
            }
            DataTable.InitFullAjax = InitFullAjax;
            function InitAjax(dataTableId, url_GetListData, columns, exportButtons, formId, url_OnClickRow, rowCallback) {
                var dom = 'lfrtip';
                if (exportButtons != null) {
                    dom = 'Bfrtip';
                }
                $(window).on('OnBIADialogRefresh', function (e) {
                    ReloadAjax(dataTableId);
                });
                var dataTableOption = {
                    "language": DataTable.cultureDataTable,
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
                        BIA.Net.Dialog.LinkToDialog($(this));
                        BIA.Net.Dialog.AddRefreshAction($(this));
                    },
                    "columns": columns,
                    "rowCallback": rowCallback
                };
                $(dataTableId).dataTable(dataTableOption);
            }
            DataTable.InitAjax = InitAjax;
            function ReloadAjax(dataTableId) {
                $(dataTableId).DataTable().ajax.reload(null, false);
            }
            DataTable.ReloadAjax = ReloadAjax;
            function InitWithFullCustomisableOptions(dataTableId, dataTableOptions) {
                dataTableOptions.language = DataTable.cultureDataTable;
                $(dataTableId).DataTable(dataTableOptions);
                $(window).on('OnBIADialogRefreshed', function () {
                    $(dataTableId).DataTable(dataTableOptions);
                });
            }
            DataTable.InitWithFullCustomisableOptions = InitWithFullCustomisableOptions;
            // ExportButtons can be : ['copy', 'csv', 'excel', 'print'] or null
            function InitStandard(dataTableId, exportButtons) {
                if (exportButtons == null) {
                    $(document).ready(function () {
                        var dataTableOption = {
                            "language": DataTable.cultureDataTable
                        };
                        $(dataTableId).DataTable(dataTableOption);
                    });
                }
                else {
                    $(document).ready(function () {
                        var dataTableOption = {
                            "language": DataTable.cultureDataTable,
                            "dom": 'Bfrtip',
                            "buttons": exportButtons
                        };
                        $(dataTableId).DataTable(dataTableOption);
                    });
                }
                DataTableSettings[dataTableId] = null;
                $(window).on('OnBIADialogRefreshing', function (e) {
                    var dataTable = e.BIANetDialogData.targetElem.find(dataTableId);
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
                        e.BIANetDialogData.targetElem.find(dataTableId).each(function () {
                            $(this).DataTable({
                                "language": DataTable.cultureDataTable,
                                "pageLength": settings.pageLength,
                                "displayStart": settings.displayStart,
                                "search": { "search": settings.search },
                                "order": settings.order,
                                retrieve: true
                            });
                        });
                    });
                }
                else {
                    $(window).on('OnBIADialogRefreshed', function (e) {
                        var settings = DataTableSettings[dataTableId];
                        e.BIANetDialogData.targetElem.find(dataTableId).each(function () {
                            var dataTableOption = {
                                "language": DataTable.cultureDataTable,
                                "dom": 'Bfrtip',
                                "buttons": exportButtons,
                                "pageLength": settings.pageLength,
                                "displayStart": settings.displayStart,
                                "search": { "search": settings.search },
                                "order": settings.order,
                                retrieve: true
                            };
                            $(this).DataTable(dataTableOption);
                        });
                    });
                }
            }
            DataTable.InitStandard = InitStandard;
        })(DataTable = Net.DataTable || (Net.DataTable = {}));
    })(Net = BIA.Net || (BIA.Net = {}));
})(BIA || (BIA = {}));
