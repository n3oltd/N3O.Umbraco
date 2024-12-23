using N3O.Umbraco.Content;

namespace N3O.Umbraco.Extensions;

public static class ContentBuilderExtensions {
    public static BlockListPropertyBuilder BlockList(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<BlockListPropertyBuilder>(propertyTypeAlias);
    }
    
    public static BooleanPropertyBuilder Boolean(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<BooleanPropertyBuilder>(propertyTypeAlias);
    }
    
    public static ContentPickerPropertyBuilder ContentPicker(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<ContentPickerPropertyBuilder>(propertyTypeAlias);
    }

    public static DataListPropertyBuilder DataList(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<DataListPropertyBuilder>(propertyTypeAlias);
    }
    
    public static DateTimePropertyBuilder DateTime(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<DateTimePropertyBuilder>(propertyTypeAlias);
    }
    
    public static DropdownPropertyBuilder Dropdown(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<DropdownPropertyBuilder>(propertyTypeAlias);
    }
    
    public static EmailPropertyBuilder Email(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<EmailPropertyBuilder>(propertyTypeAlias);
    }
    
    public static LabelPropertyBuilder Label(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<LabelPropertyBuilder>(propertyTypeAlias);
    }
    
    public static MultipleTextPropertyBuilder MultipleText(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<MultipleTextPropertyBuilder>(propertyTypeAlias);
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
    
    public static RadioButtonListPropertyBuilder RadioButtonList(this IContentBuilder builder,
                                                                 string propertyTypeAlias) {
        return builder.Property<RadioButtonListPropertyBuilder>(propertyTypeAlias);
    }
    
    public static RawPropertyBuilder Raw(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<RawPropertyBuilder>(propertyTypeAlias);
    }
    
    public static TemplatedLabelPropertyBuilder TemplatedLabel(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<TemplatedLabelPropertyBuilder>(propertyTypeAlias);
    }

    public static TextareaPropertyBuilder Textarea(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<TextareaPropertyBuilder>(propertyTypeAlias);
    }
    
    public static TextBoxPropertyBuilder TextBox(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<TextBoxPropertyBuilder>(propertyTypeAlias);
    }
    
    public static TogglePropertyBuilder Toggle(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<TogglePropertyBuilder>(propertyTypeAlias);
    }
}
