using System;

namespace N3O.Umbraco.Data.Models {
    public class ColumnTemplate : Value {
        public ColumnTemplate(string heading, int maxValues, UmbracoPropertyInfo propertyInfo) {
            if (maxValues < 1 || maxValues > DataConstants.Limits.Columns.MaxValues) {
                throw new ArgumentOutOfRangeException(nameof(maxValues), $"Value must be between 1 and {DataConstants.Limits.Columns.MaxValues}");
            }
            
            Heading = heading;
            MaxValues = maxValues;
            PropertyInfo = propertyInfo;
        }

        public string Heading { get; }
        public int MaxValues { get; }
        public UmbracoPropertyInfo PropertyInfo { get; }
    }
}