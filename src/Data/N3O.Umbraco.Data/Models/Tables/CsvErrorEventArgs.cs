using N3O.Umbraco.Data.Lookups;
using System;

namespace N3O.Umbraco.Data.Models {
    public class CsvErrorEventArgs : EventArgs {
        public CsvErrorEventArgs(CsvError error, CsvSelect select, int rowNumber, string rawField) {
            Error = error;
            Select = select;
            RowNumber = rowNumber;
            RawField = rawField;
        }

        public CsvError Error { get; }
        public CsvSelect Select { get; }
        public int RowNumber { get; }
        public string RawField { get; }
    }
}