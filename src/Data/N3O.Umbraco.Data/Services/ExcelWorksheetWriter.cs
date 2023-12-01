using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExcelColumn = N3O.Umbraco.Data.Models.ExcelColumn;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using WorkTable = OfficeOpenXml.Table.ExcelTable;

namespace N3O.Umbraco.Data;

public class ExcelWorksheetWriter {
    private enum RenderMode {Table, DataSummary} 
    private const int FirstColumn = 1;
    private const int FirstRow = 1;
    private const int TableNameMaxLength = 255;
    private const int WorksheetNameMaxLength = 31;
    
    private readonly Dictionary<ExcelColumn, ExcelFormatting> _footerFormatting = new();
    private RenderMode _renderMode;
    private IExcelTable _table;
    private int _rowCursor = FirstRow;
    private int _columnCursor = FirstColumn;
    private DataSummary _dataSummary;
    private string _sheetName;

    public void InsertTable(IExcelTable table) {
        _renderMode = RenderMode.Table;
        _table = table;
    }
    
    public void InsertDataSummary(DataSummary dataSummary, string sheetName) {
        _renderMode = RenderMode.DataSummary;
        _dataSummary = dataSummary;
        _sheetName = sheetName;
    }

    public void Write(ExcelWorksheets worksheets, bool formatAsTable) {
        if (_renderMode == RenderMode.Table) {
            var worksheet = worksheets.Add(GetExcelSafeName(_table.Name, WorksheetNameMaxLength));
            WriteTable(worksheet, formatAsTable);    
        } else {
            var worksheet = worksheets.Add(GetExcelSafeName(_sheetName, WorksheetNameMaxLength));
            WriteDataSummary(worksheet, formatAsTable);
        }
    }

    private void WriteDataSummary(ExcelWorksheet worksheet, bool formatAsTable) {
        for (int i = 0; i < _dataSummary.LinesBefore; i++) {
            NextRow();
        }

        foreach (var row in _dataSummary.Rows) {
            var boldText = new ExcelFormatting();
            boldText.NumberFormat = new StringExcelNumberFormat();
            boldText.Bold();
            WriteValue(worksheet, row.Label, boldText);

            foreach (var value in row.Values) {
                WriteValue(worksheet, value.Value,  new ExcelFormatting(){ NumberFormat = value.Formatting });
            }
            NextRow();
        }
        
        for (int i = 0; i < _dataSummary.LinesAfter; i++) {
            NextRow();
        }
    }

    private void WriteTable(ExcelWorksheet worksheet, bool formatAsTable) {
        if (formatAsTable) {
            WriteHeaders(worksheet);
        }

        WriteBody(worksheet);

        if (formatAsTable) {
            var lastRow = _rowCursor - 1;
            var lastColumn = _table.ColumnCount;

            if (lastColumn > FirstColumn) {
                var tableRange = new ExcelAddressBase(FirstRow, FirstColumn, lastRow, lastColumn);
                var tableName = GetExcelSafeName(_table.Name, TableNameMaxLength);

                var table = worksheet.Tables.Add(tableRange, tableName);
                WriteFooters(worksheet, table);
            }
        }

        worksheet.Cells.AutoFitColumns();
    }

    private void WriteHeaders(ExcelWorksheet worksheet) {
        foreach (var column in _table.Columns) {
            var titleCell = ExcelCell.FromCell(OurDataTypes.String.Cell(column.Title, null),
                                               column.Title,
                                               column.Comment,
                                               null,
                                               ExcelFormatting.None);

            WriteCell(worksheet, titleCell);
        }

        NextRow();
    }

    private void WriteBody(ExcelWorksheet worksheet) {
        for (var row = 0; row < _table.RowCount; row++) {
            foreach (var column in _table.Columns) {
                var cell = _table[column, row];

                WriteCell(worksheet, cell);

                if (cell.HasValue(x => x.Formatting)) {
                    UpdateFooterFormatting(column, cell.Formatting);
                }
            }

            NextRow();
        }
    }

    // If all cells have identical formatting, use that formatting for the footer also, otherwise we
    // don't have a single style we can apply to the footer.
    private void UpdateFooterFormatting(ExcelColumn column, ExcelFormatting formatting) {
        var currentFormatting = _footerFormatting.GetOrAdd(column, () => formatting);

        if (currentFormatting != ExcelFormatting.None) {
            if (formatting != currentFormatting) {
                _footerFormatting[column] = ExcelFormatting.None;
            }
        }
    }

    private void WriteFooters(ExcelWorksheet worksheet, WorkTable workTable) {
        if (_table.HasFooters) {
            for (var i = 0; i < _table.ColumnCount; i++) {
                var column = _table.Columns[i];

                workTable.ShowTotal = true;

                if (column.FooterFunction != null) {
                    workTable.Columns[i].TotalsRowFunction = Enum.Parse<RowFunctions>(column.FooterFunction.Id, true);

                    FormatFooter(worksheet, column);
                }

                NextColumn();
            }

            NextRow();
        }
    }

    private void FormatFooter(ExcelWorksheet worksheet, ExcelColumn column) {
        var workCell = GetCurrentCell(worksheet);
        var formatting = _footerFormatting[column];

        workCell.ApplyFormatting(formatting);
    }

    private void WriteCell(ExcelWorksheet worksheet, ExcelCell cell) {
        var workCell = GetCurrentCell(worksheet);

        workCell.Value = cell?.ExcelValue;

        if (cell.HasValue(x => x.Comment)) {
            var comment = workCell.AddComment(cell.Comment);
            comment.Author = "N3O Ltd";
            comment.AutoFit = true;
        }
        
        if (cell.HasValue(x => x.HyperLink)) {
            workCell.SetHyperlink(cell.HyperLink);
        }

        if (cell.HasValue(x => x.Formatting)) {
            workCell.ApplyFormatting(cell.Formatting);
        }

        NextColumn();
    }
    
    private void WriteValue(ExcelWorksheet worksheet, object value, ExcelFormatting formatting) {
        var workCell = GetCurrentCell(worksheet);

        workCell.Value = value;

        if (formatting.HasValue()) {
            workCell.ApplyFormatting(formatting);
        }

        NextColumn();
    }

    private ExcelRange GetCurrentCell(ExcelWorksheet worksheet) {
        var workCell = worksheet.Cells[_rowCursor, _columnCursor];

        return workCell;
    }

    private void NextRow() {
        _rowCursor++;
        _columnCursor = FirstColumn;
    }

    private void NextColumn() {
        _columnCursor++;
    }

    private string GetExcelSafeName(string name, int maxLength) {
        var safeName = Regex.Replace(name, "[^a-z0-9]", "_", RegexOptions.IgnoreCase);

        if (safeName.Length > maxLength) {
            safeName = safeName.Substring(0, maxLength);
        }

        return safeName;
    }
}
