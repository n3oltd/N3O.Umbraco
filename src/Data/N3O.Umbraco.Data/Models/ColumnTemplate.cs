using System;

namespace N3O.Umbraco.Data.Models {
    public class ColumnTemplate : Value {
        private const int MaxValuesLimit = 20;
        
        public ColumnTemplate(string heading, int maxValues, UmbracoPropertyInfo propertyInfo) {
            Heading = heading;
            MaxValues = Math.Min(maxValues == 0 ? MaxValuesLimit : maxValues, MaxValuesLimit);
            PropertyInfo = propertyInfo;
        }

        public string Heading { get; }
        public int MaxValues { get; }
        public UmbracoPropertyInfo PropertyInfo { get; }
    }
}