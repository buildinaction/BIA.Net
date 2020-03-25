declare module BIA.Net.DataTable {
    let cultureDataTable: DataTables.LanguageSettings;
    function InitFullAjax(dataTableId: any, url_GetListData: any, columns: any, displayExportButton: any, getDataExtend: any, url_OnClickRow: any, rowCallback: any): void;
    function InitAjax(dataTableId: any, url_GetListData: any, columns: any, exportButtons: any, formId: any, url_OnClickRow: any, rowCallback: any): void;
    function ReloadAjax(dataTableId: any): void;
    function InitWithFullCustomisableOptions(dataTableId: any, dataTableOptions: any): void;
    function InitStandard(dataTableId: any, exportButtons: any): void;
}
