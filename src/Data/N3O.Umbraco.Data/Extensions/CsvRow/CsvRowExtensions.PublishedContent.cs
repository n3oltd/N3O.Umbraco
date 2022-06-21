using N3O.Umbraco.Data.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static IPublishedContent GetPublishedContent(this CsvRow csvRow, string heading, Guid? parentId = null) {
        return GetPublishedContent(csvRow, CsvSelect.For(heading), parentId);
    }
    
    public static IPublishedContent GetPublishedContent(this CsvRow csvRow, int index, Guid? parentId = null) {
        return GetPublishedContent(csvRow, CsvSelect.For(index), parentId);
    }
    
    public static IPublishedContent GetPublishedContent(this CsvRow csvRow,
                                                        CsvSelect select,
                                                        Guid? parentId = null) {
        return csvRow.ParseField(select, (parser, field) => parser.PublishedContent
                                                                  .Parse(field,
                                                                         OurDataTypes.PublishedContent.GetClrType(),
                                                                         parentId));
    }
}
