using N3O.Umbraco.Data.Models;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class GuidExcelCellConverter : ExcelCellConverter<Guid?>, IDefaultExcelCellConverter {
        protected override ExcelNumberFormat GetNumberFormat(Column column, Guid? value) {
            return new StringExcelNumberFormat();
        }
    }
}