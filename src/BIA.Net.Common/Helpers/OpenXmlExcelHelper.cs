namespace BIA.Net.Common.Helpers
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// OpenXml Excel Helper
    /// </summary>
    public static class OpenXmlExcelHelper
    {
        /// <summary>
        /// return a new instance of the CellValue (CellValues.String)
        /// </summary>
        /// <param name="text">text value</param>
        /// <returns>excel cell</returns>
        public static Cell GetCell(string text)
        {
            return new Cell
            {
                CellValue = new CellValue(text),
                DataType = CellValues.String
            };
        }

        /// <summary>
        /// Creates the work book.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns>File Byte Array</returns>
        public static byte[] CreateWorkBook(List<Row> rows)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook, true))
                {
                    // Add a WorkbookPart to the document.
                    WorkbookPart workbookpart = spreadSheet.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

                    worksheetPart.Worksheet = new Worksheet(new SheetData(rows));

                    // Add Sheets to the Workbook.
                    Sheets sheets = spreadSheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                    string sheetName = "sheet1";

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadSheet.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = sheetName,
                    };

                    sheets.Append(sheet);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
