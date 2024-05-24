using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static class JsonConverterExtensions {
    public static bool IsNotSecondaryConverter(this IPropertyValueConverter converter) {
        return converter is not MustBeStringValueConverter && converter is not TextStringValueTypeConverter;
    }
}