using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters;

public class GuidExcelCellConverter : ExcelCellConverter<Guid?>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(Guid? value, IFormatter formatter) {
        return new StringExcelNumberFormat();
    }
}
