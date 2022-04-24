using N3O.Umbraco.Data.Models;
using System;

namespace N3O.Umbraco.Data.Converters {
    public abstract class ExcelCellConverter<T> : IExcelCellConverter<T> {
        public ExcelCell<T> GetExcelCell(Column column, Cell<T> cell) {
            var excelValue = GetExcelValue(column, cell.Value);
            var hyperLink = GetHyperlink(column, cell.Value);
            var formatting = GetFormatting(column, cell);

            var excelCell = ExcelCell.FromCell(cell, excelValue, column.Comment, hyperLink, formatting);

            return excelCell;
        }

        public ExcelCell GetExcelCell(Column column, Cell cell) {
            return GetExcelCell(column, (Cell<T>) cell);
        }

        private ExcelFormatting GetFormatting(Column column, Cell<T> cell) {
            var excelFormatting = new ExcelFormatting();

            var numberFormat = GetNumberFormat(column, cell.Value);
            if (numberFormat != null) {
                excelFormatting.NumberFormat = numberFormat;
            }

            ApplyFormatting(column, cell, excelFormatting);

            excelFormatting.ApplyFormattingAttributes(column);

            return excelFormatting;
        }

        protected virtual object GetExcelValue(Column column, T value) {
            return value;
        }

        protected virtual Uri GetHyperlink(Column column, T value) {
            return null;
        }

        protected virtual ExcelNumberFormat GetNumberFormat(Column column, T value) {
            return null;
        }

        protected virtual void ApplyFormatting(Column column, Cell<T> cell, ExcelFormatting formatting) { }
    }
}