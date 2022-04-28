using N3O.Umbraco.Content;

namespace N3O.Umbraco.Extensions {
    public static class ContentBuilderExtensions {
        public static ContentPickerPropertyBuilder ContentPicker(this IContentBuilder builder,
                                                                 string propertyTypeAlias) {
            return builder.Property<ContentPickerPropertyBuilder>(propertyTypeAlias);
        }

        public static DataListPropertyBuilder DataList(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<DataListPropertyBuilder>(propertyTypeAlias);
        }
        
        public static DateTimePropertyBuilder DateTime(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<DateTimePropertyBuilder>(propertyTypeAlias);
        }

        public static NestedPropertyBuilder Nested(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<NestedPropertyBuilder>(propertyTypeAlias);
        }
        
        public static NullPropertyBuilder Null(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<NullPropertyBuilder>(propertyTypeAlias);
        }
        
        public static NumericPropertyBuilder Numeric(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<NumericPropertyBuilder>(propertyTypeAlias);
        }
        
        public static RadioButtonListPropertyBuilder RadioButtonList(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<RadioButtonListPropertyBuilder>(propertyTypeAlias);
        }
        
        public static TextBoxPropertyBuilder TextBox(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<TextBoxPropertyBuilder>(propertyTypeAlias);
        }
        
        public static TogglePropertyBuilder Toggle(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<TogglePropertyBuilder>(propertyTypeAlias);
        }
    }
}