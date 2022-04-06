using System;
using System.ComponentModel;
using System.Globalization;

namespace N3O.Umbraco.Entities {
    public class RevisionIdTypeConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value == null) {
                return null;
            }

            var stringValue = value as string;
            var revisionId = RevisionId.TryParse(stringValue);
            
            if (revisionId != null) {
                return revisionId;
            }
            
            return base.ConvertFrom(context, culture, value);
        }
    }
}
