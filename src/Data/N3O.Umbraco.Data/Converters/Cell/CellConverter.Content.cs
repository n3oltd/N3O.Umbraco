using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters {
    public class ContentCellConverter : ICellConverter<IContent> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, IContent value, Type targetType) {
            return DataTypes.Content.Cell(value, targetType);
        }
    }
}