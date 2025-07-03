using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExcelColumn = N3O.Umbraco.Data.Models.ExcelColumn;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using WorkTable = OfficeOpenXml.Table.ExcelTable;

namespace N3O.Umbraco.Data;

public class ExcelWorksheetWriter {
    private enum RenderMode { Table, SummaryFields }
    
    private const int FirstColumn = 1;
    private const int FirstRow = 1;
    private const int TableNameMaxLength = 255;
    private const int WorksheetNameMaxLength = 31;
    
    private readonly Dictionary<ExcelColumn, ExcelFormatting> _footerFormatting = new();
    private readonly List<(RenderMode, object)> _toRender = [];
    private readonly IExcelCellFormatter _excelCellFormatter;
    private readonly IFormatter _formatter;
    private int _rowCursor = FirstRow;
    private int _columnCursor = FirstColumn;
    private string _sheetName;

    public ExcelWorksheetWriter(IExcelCellFormatter excelCellFormatter, IFormatter formatter) {
        _excelCellFormatter = excelCellFormatter;
        _formatter = formatter;
    }

    public void InsertSummaryFields(SummaryFields summaryFields) {
        _toRender.Add((RenderMode.SummaryFields, summaryFields));
    }
    
    public void InsertTable(IExcelTable table) {
        _toRender.Add((RenderMode.Table, table));
    }

    public void SetSheetName(string sheetName) {
        _sheetName = sheetName;
    }

    public void Write(ExcelWorksheets worksheets) {
        var worksheet = worksheets.Add(_sheetName.Left(WorksheetNameMaxLength));

        foreach (var (renderMode, target) in _toRender) {
            if (renderMode == RenderMode.SummaryFields) {
                WriteSummaryFields(worksheet, (SummaryFields) target);
            } else if (renderMode == RenderMode.Table) {
                WriteTable(worksheet, (IExcelTable) target);
            } else {
                throw UnrecognisedValueException.For(renderMode);
            }
            NextRow();
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void WriteSummaryFields(ExcelWorksheet worksheet, SummaryFields summaryFields) {
        for (int i = 0; i < summaryFields.LinesBefore; i++) {
            NextRow();
        }

        foreach (var field in summaryFields.Fields) {
            var labelExcelCell = _excelCellFormatter.FormatCell(OurDataTypes.String.Cell(field.Label), _formatter);
            labelExcelCell.Formatting.Bold();
            
            WriteCell(worksheet, labelExcelCell);

            foreach (var cell in field.Cells) {
                var fieldExcelCell = _excelCellFormatter.FormatCell(cell, _formatter);
                
                WriteCell(worksheet, fieldExcelCell);
            }
            
            NextRow();
        }
        
        for (int i = 0; i < summaryFields.LinesAfter; i++) {
            NextRow();
        }
    }

    private void WriteTable(ExcelWorksheet worksheet, IExcelTable table, bool formatAsTable = true) {
        var startRow = _rowCursor;
        var startColumn = _columnCursor;
        
        if (formatAsTable) {
            WriteHeaders(worksheet, table);
        }

        WriteBody(worksheet, table);

        if (formatAsTable) {
            var lastRow = _rowCursor - 1;
            var lastColumn = table.ColumnCount;

            if (lastColumn > startColumn) {
                var tableRange = new ExcelAddressBase(startRow, startColumn, lastRow, lastColumn);
                var tableName = GetTableSafeName(table.Name, TableNameMaxLength);

                var excelTable = worksheet.Tables.Add(tableRange, tableName);
                WriteFooters(worksheet, table, excelTable);
            }
        }
    }

    private void WriteHeaders(ExcelWorksheet worksheet, IExcelTable table) {
        foreach (var column in table.Columns) {
            var titleCell = ExcelCell.FromCell(OurDataTypes.String.Cell(column.Title),
                                               column.Title,
                                               column.Comment,
                                               null,
                                               ExcelFormatting.None);

            WriteCell(worksheet, titleCell);
        }

        NextRow();
    }

    private void WriteBody(ExcelWorksheet worksheet, IExcelTable table) {
        for (var row = 0; row < table.RowCount; row++) {
            foreach (var column in table.Columns) {
                var cell = table[column, row];

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

    private void WriteFooters(ExcelWorksheet worksheet, IExcelTable table, WorkTable workTable) {
        if (table.HasFooters) {
            for (var i = 0; i < table.ColumnCount; i++) {
                var column = table.Columns[i];

                workTable.ShowTotal = true;

                if (column.FooterFunction != null) {
                    workTable.Columns[i].TotalsRowFunction = column.FooterFunction
                                                                   .Id
                                                                   .ToEnum<RowFunctions>()
                                                                   .GetValueOrThrow();

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

    private string GetTableSafeName(string name, int maxLength) {
        var safeName = Regex.Replace(name, "[^a-z0-9]", "_", RegexOptions.IgnoreCase);

        if (safeName.Length > maxLength) {
            safeName = safeName.Substring(0, maxLength);
        }

        return safeName;
    }
}
