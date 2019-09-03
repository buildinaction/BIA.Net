namespace BIA.Net.Common.Helpers
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
        /// return a new instance of the CellValue (CellValues.String)
        /// </summary>
        /// <param name="text">text value</param>
        /// <param name="shareStringPart">string table shared to storage text</param>
        /// <returns>excel cell</returns>
        private static Cell GetCell(string text, SharedStringTablePart shareStringPart)
        {
            int index = InsertSharedStringItem(text, shareStringPart);

            return new Cell
            {
                CellValue = new CellValue(index.ToString()),
                DataType = new EnumValue<CellValues>(CellValues.SharedString)
            };
        }

        /// <summary>
        /// Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text
        /// and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        /// </summary>
        /// <param name="text">Test to insert in the string table shared</param>
        /// <param name="shareStringPart">string table shared to sotrage the text</param>
        /// <returns>Return the index id from the table where the text is storaged</returns>
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (var item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        /// <summary>
        /// Function to insert a sheet in the workbook
        /// </summary>
        /// <param name="workbookPart">current workbook</param>
        /// <param name="name">Name of the sheet</param>
        /// <returns>Return the sheet created</returns>
        private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart, string name = "")
        {
            // Add a new worksheet part to the workbook.
            var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            string sheetName = name;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet" + sheetId;
            }

            // Append the new worksheet and associate it with the workbook.
            var sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        /// <summary>
        /// Creates the work book.
        /// </summary>
        /// <param name="listSheets">The rows.</param>
        /// <returns>File Byte Array</returns>
        public static byte[] CreateWorkBook(IDictionary<string, List<Row>> listSheets)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Init the excel file
                using (var spreadSheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook, true))
                {
                    // Add a WorkbookPart to the document.
                    var workbookpart = spreadSheet.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    // Get the SharedStringTablePart. If it does not exist, create a new one.
                    SharedStringTablePart shareStringPart;
                    if (workbookpart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        shareStringPart = workbookpart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        shareStringPart = workbookpart.AddNewPart<SharedStringTablePart>();
                    }

                    // Add Sheets to the Workbook.
                    var sheets = workbookpart.Workbook.AppendChild<Sheets>(new Sheets());
                    foreach (var currentSheet in listSheets)
                    {
                        // Convert the rows with the shareStringPart
                        var listRows = new List<Row>();
                        foreach (var currentRow in currentSheet.Value)
                        {
                            var listCells = new List<Cell>();
                            foreach (var currentCell in currentRow.Elements<Cell>())
                            {
                                listCells.Add(GetCell(currentCell.InnerText, shareStringPart));
                            }

                            listRows.Add(new Row(listCells));
                        }

                        // Create the Sheet
                        var sheetReference = InsertWorksheet(spreadSheet.WorkbookPart, currentSheet.Key);

                        // Add the data in the sheet
                        sheetReference.Worksheet = new Worksheet(new SheetData(listRows));
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
