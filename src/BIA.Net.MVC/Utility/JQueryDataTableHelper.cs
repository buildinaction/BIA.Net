namespace BIA.Net.MVC.Utility
{
    using BIA.Net.Business.JQueryDataTable;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Helper class for JQueryDataTable
    /// </summary>
    public static class JQueryDataTableHelper
    {
        /// <summary>
        /// Gets Columns from JQuery DataTable Parameter and Request
        /// </summary>
        /// <param name="param">Parameter provide by DataTables</param>
        /// <param name="request">Curent Http Request Base</param>
        public static void GetColumns(ref JQueryDataTableParameterModel param, HttpRequestBase request)
        {
            param.Columns = new List<JQueryDataTableParameterColumn>();

            for (int i = 0; i < param.iColumns; i++)
            {
                bool orderable = false;
                bool.TryParse(request[string.Format("bSortable_{0}", i.ToString())], out orderable);

                bool searchable = false;
                bool.TryParse(request[string.Format("bSearchable_{0}", i.ToString())], out searchable);

                foreach (string columnName in param.sColumns.Split(',').ElementAt(i).Split(new string[] { "||" }, StringSplitOptions.None))
                {
                    param.Columns.Add(new JQueryDataTableParameterColumn() { Index = i, SName = columnName.Replace(" ", string.Empty), Orderable = orderable, Searchable = searchable });
                }
            }
        }
    }
}