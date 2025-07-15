using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static partial class ContentBuilderExtensions {
    public static void SetContentOrPublishedLookupValue<T>(this IContentBuilder builder, string propertyAlias, T value)
        where T : IContentOrPublishedLookup {
        if (value.ContentId.HasValue()) {
            var contentBuilder = builder.Property<ContentPickerPropertyBuilder>(propertyAlias);

            contentBuilder.SetContent(value.ContentId.GetValueOrThrow());
        } else {
            var contentBuilder = builder.Property<DataListPropertyBuilder>(propertyAlias);

            contentBuilder.SetLookups(value);
        }
    }
    
    public static void SetContentOrPublishedLookupValues<T>(this IContentBuilder builder,
                                                            string propertyAlias,
                                                            IEnumerable<T> values)
        where T : IContentOrPublishedLookup {
        if (values.All(x => x.ContentId.HasValue())) {
            var contentBuilder = builder.Property<ContentPickerPropertyBuilder>(propertyAlias);

            contentBuilder.SetContent(values.Select(x => x.ContentId.GetValueOrThrow()));
        } else {
            var contentBuilder = builder.Property<DataListPropertyBuilder>(propertyAlias);

            contentBuilder.SetLookups(values);
        }
    }
}