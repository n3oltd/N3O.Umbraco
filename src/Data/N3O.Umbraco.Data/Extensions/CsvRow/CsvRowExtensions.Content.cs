using N3O.Umbraco.Data.Models;
using System;
using Umbraco.Cms.Core.Models;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static IContent GetContent(this CsvRow csvRow, string heading, Guid? parentId = null) {
        return GetContent(csvRow, CsvSelect.For(heading), parentId);
    }
    
    public static IContent GetContent(this CsvRow csvRow, int index, Guid? parentId = null) {
        return GetContent(csvRow, CsvSelect.For(index), parentId);
    }
    
    public static IContent GetContent(this CsvRow csvRow, CsvSelect select, Guid? parentId = null) {
        return csvRow.ParseField(select, (parser, field) => parser.Content.Parse(field,
                                                                                 OurDataTypes.Content.GetClrType(),
                                                                                 parentId));
    }
}
