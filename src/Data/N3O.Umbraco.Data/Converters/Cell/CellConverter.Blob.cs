using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class BlobCellConverter : ICellConverter<Blob> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Blob value, Type targetType) {
            return DataTypes.Blob.Cell(value);
        }
    }
}