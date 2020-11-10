namespace BIA.Net.Common.Helpers
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System;
    using System.Collections.Generic;
    using System.Data;
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
                StyleIndex = 0U,
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
                StyleIndex = 0U,
                CellValue = new CellValue(index.ToString()),
                DataType = new EnumValue<CellValues>(CellValues.SharedString)
            };
        }

        /// <summary>
        /// return a new instance of the CellValue (CellValues.Date, display format dd/MM/yyyy)
        /// </summary>
        /// <param name="date">Date value</param>
        /// <returns>excel cell</returns>
        public static Cell GetCell(DateTime? date)
        {
            return new Cell
            {
                StyleIndex = 1U,
                CellValue = new CellValue(date?.ToString("yyyy-MM-dd")),
                DataType = CellValues.Date
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
        /// <param name="useSharedString">True if SharedStringTable is used</param>
        /// <param name="keepCellFormat">True to keep the CellFormat in listSheets cells</param>
        /// <returns>File Byte Array</returns>
        public static byte[] CreateWorkBook(IDictionary<string, List<Row>> listSheets, bool useSharedString = true, bool keepCellFormat = false, double? defaultColumnWidth = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Init the excel file
                using (var spreadSheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook, true))
                {
                    // Add a WorkbookPart to the document.
                    var workbookpart = spreadSheet.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();
                    WorkbookStylesPart workbookStylesPart = workbookpart.AddNewPart<WorkbookStylesPart>();
                    workbookStylesPart.Stylesheet = GenerateStylesheet();

                    // Get the SharedStringTablePart. If it does not exist, create a new one.
                    SharedStringTablePart shareStringPart = null;
                    if (useSharedString)
                    {
                        if (workbookpart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                        {
                            shareStringPart = workbookpart.GetPartsOfType<SharedStringTablePart>().First();
                        }
                        else
                        {
                            shareStringPart = workbookpart.AddNewPart<SharedStringTablePart>();
                        }
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
                                if (keepCellFormat && currentCell.DataType.Value == CellValues.Date)
                                {
                                    listCells.Add(new Cell
                                    {
                                        StyleIndex = 1U,
                                        CellValue = new CellValue(currentCell.InnerText),
                                        DataType = CellValues.Date
                                    });
                                }
                                else
                                {
                                    listCells.Add(useSharedString ? GetCell(currentCell.InnerText, shareStringPart) : GetCell(currentCell.InnerText));
                                }
                            }

                            listRows.Add(new Row(listCells));
                        }

                        // Create the Sheet
                        var sheetReference = InsertWorksheet(spreadSheet.WorkbookPart, currentSheet.Key);

                        Worksheet worksheet = new Worksheet();
                        SheetData sheetData = new SheetData();

                        // Add the data in the sheet
                        sheetData.Append(listRows);
                        if (defaultColumnWidth.HasValue)
                        {
                            Columns columns = worksheet.GetFirstChild<Columns>() ?? new Columns();
                            columns.Append(new Column() { Min = 1, Max = 27, CustomWidth = true, Width = 12 });
                            worksheet.Append(columns);
                        }

                        worksheet.Append(sheetData);
                        sheetReference.Worksheet = worksheet;
                        sheetReference.Worksheet.Save();


                    }
                }

                return memoryStream.ToArray();
            }
        }


        public static DataTable ReadAsDataTable(string fileName)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Generate style for current sheet.
        /// </summary>
        /// <returns>Stylesheet.</returns>
        public static Stylesheet GenerateStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet();

            // Default Font
            var fonts = new Fonts() { Count = 1, KnownFonts = BooleanValue.FromBoolean(true) };
            var font = new Font
            {
                FontSize = new FontSize() { Val = 11 },
                FontName = new FontName() { Val = "Calibri" },
                FontFamilyNumbering = new FontFamilyNumbering() { Val = 2 },
                FontScheme = new FontScheme() { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            };
            fonts.Append(font);
            stylesheet.Append(fonts);

            // Default Fill
            var fills = new Fills() { Count = 1 };
            var fill = new Fill();
            fill.PatternFill = new PatternFill() { PatternType = new EnumValue<PatternValues>(PatternValues.None) };
            fills.Append(fill);
            stylesheet.Append(fills);

            // Default Border
            var borders = new Borders() { Count = 1 };
            var border = new Border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            };
            borders.Append(border);
            stylesheet.Append(borders);

            // Default cell format and a date cell format
            var cellFormats = new CellFormats() { Count = 2 };

            var cellFormatDefault = new CellFormat { NumberFormatId = 0, FormatId = 0, FontId = 0, BorderId = 0, FillId = 0 };
            cellFormats.Append(cellFormatDefault);

            var cellFormatDate = new CellFormat { NumberFormatId = 14, FormatId = 0, FontId = 0, BorderId = 0, FillId = 0, ApplyNumberFormat = BooleanValue.FromBoolean(true) };
            cellFormats.Append(cellFormatDate);

            stylesheet.Append(cellFormats);

            return stylesheet;
        }
    }
}
