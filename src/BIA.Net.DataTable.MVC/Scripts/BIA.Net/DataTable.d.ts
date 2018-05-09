declare module BIA.Net.DataTable {
    var cultureDataTable: {
        "sEmptyTable": string;
        "sInfo": string;
        "sInfoEmpty": string;
        "sInfoFiltered": string;
        "sInfoPostFix": string;
        "sInfoThousands": string;
        "sLengthMenu": string;
        "sLoadingRecords": string;
        "sProcessing": string;
        "sSearch": string;
        "sZeroRecords": string;
        "oPaginate": {
            "sFirst": string;
            "sLast": string;
            "sNext": string;
            "sPrevious": string;
        };
        "oAria": {
            "sSortAscending": string;
            "sSortDescending": string;
        };
    };
    function InitFullAjax(dataTableId: any, url_GetListData: any, columns: any, displayExportButton: any, getDataExtend: any, url_OnClickRow: any, rowCallback: any): void;
    function InitAjax(dataTableId: any, url_GetListData: any, columns: any, exportButtons: any, formId: any, url_OnClickRow: any): void;
    function ReloadAjax(dataTableId: any): void;
    function InitStandard(dataTableId: any, exportButtons: any): void;
}
