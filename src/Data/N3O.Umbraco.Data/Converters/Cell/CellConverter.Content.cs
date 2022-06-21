using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class ContentCellConverter : ICellConverter<IContent> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, IContent value, Type targetType) {
        return OurDataTypes.Content.Cell(value, targetType);
    }
}
