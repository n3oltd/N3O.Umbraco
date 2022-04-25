using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Converters {
    public class PublishedContentCellConverter : ICellConverter<IPublishedContent> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, IPublishedContent value, Type targetType) {
            return DataTypes.PublishedContent.Cell(value, targetType);
        }
    }
}