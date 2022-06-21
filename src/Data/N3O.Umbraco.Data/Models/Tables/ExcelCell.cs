using N3O.Umbraco.Data.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class ExcelCell<T> : ExcelCell {
    protected internal ExcelCell(DataType type,
                                 T value,
                                 Type targetType,
                                 Dictionary<string, IEnumerable<object>> metadata,
                                 object excelValue,
                                 string comment,
                                 Uri hyperLink,
                                 ExcelFormatting formatting)
        : base(type, value, targetType, metadata, excelValue, comment, hyperLink, formatting) { }
}

public class ExcelCell : Cell {
    public ExcelCell(DataType type,
                     object value,
                     Type targetType,
                     Dictionary<string, IEnumerable<object>> metadata,
                     object excelValue,
                     string comment,
                     Uri hyperLink,
                     ExcelFormatting formatting)
        : base(type, value, targetType, metadata) {
        ExcelValue = excelValue;
        Comment = comment;
        HyperLink = hyperLink;
        Formatting = formatting;
    }

    public object ExcelValue { get; }
    public string Comment { get; }
    public Uri HyperLink { get; }
    public ExcelFormatting Formatting { get; }

    public static ExcelCell<T> FromCell<T>(Cell<T> cell,
                                           object excelValue,
                                           string comment,
                                           Uri hyperLink,
                                           ExcelFormatting formatting) {
        return new ExcelCell<T>(cell.Type,
                                cell.Value,
                                cell.TargetType,
                                cell.Metadata,
                                excelValue,
                                comment,
                                hyperLink,
                                formatting);
    }
}
