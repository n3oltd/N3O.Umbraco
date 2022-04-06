using N3O.Umbraco.Extensions;
using System;
using System.ComponentModel;
using System.Globalization;

namespace N3O.Umbraco.Entities {
    public class EntityIdTypeConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value == null) {
                return null;
            }

            var stringValue = value as string;

            if (stringValue.HasValue() && Guid.TryParse(stringValue, out var guid)) {
                return new EntityId(guid);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
