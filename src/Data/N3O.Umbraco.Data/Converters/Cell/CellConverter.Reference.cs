using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.References;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters {
    public class ReferenceCellConverter : ICellConverter<Reference> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Reference value, Type targetType) {
            return OurDataTypes.Reference.Cell(value);
        }
    }
}