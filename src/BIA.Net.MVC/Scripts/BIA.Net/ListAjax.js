var DataTable_ListAjax = {
    Init: function (tableId, columns, url, languageUrl, urlRedirect) {

        $(tableId).DataTable({
            "bProcessing": true,
            "bServerSide": true,
            "filter": true,
            "sAjaxSource": url,
            "language": { "url": languageUrl },
            "aoColumnDefs": [{ "sClass": "hide_column", "aTargets": [0] }],
            "aoColumns": columns
        });
        if (urlRedirect != null) {
            TablesUtility.InitTableList(tableId, urlRedirect);
        }
    },
}