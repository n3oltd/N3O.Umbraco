using N3O.Umbraco.Content;

namespace N3O.Umbraco.Extensions;

public static class ContentBuilderExtensions {
    public static BlockListPropertyBuilder BlockList(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<BlockListPropertyBuilder>(propertyAlias);
    }
    
    public static BooleanPropertyBuilder Boolean(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<BooleanPropertyBuilder>(propertyAlias);
    }
    
    public static ContentPickerPropertyBuilder ContentPicker(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<ContentPickerPropertyBuilder>(propertyAlias);
    }

    public static DataListPropertyBuilder DataList(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<DataListPropertyBuilder>(propertyAlias);
    }
    
    public static DateTimePropertyBuilder DateTime(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<DateTimePropertyBuilder>(propertyAlias);
    }
    
    public static DropdownPropertyBuilder Dropdown(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<DropdownPropertyBuilder>(propertyAlias);
    }
    
    public static EmailPropertyBuilder Email(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<EmailPropertyBuilder>(propertyAlias);
    }
    
    public static LabelPropertyBuilder Label(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<LabelPropertyBuilder>(propertyAlias);
    }
    
    public static MultipleTextPropertyBuilder MultipleText(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<MultipleTextPropertyBuilder>(propertyAlias);
    }

    public static NestedPropertyBuilder Nested(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<NestedPropertyBuilder>(propertyAlias);
    }
    
    public static NullPropertyBuilder Null(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<NullPropertyBuilder>(propertyAlias);
    }
    
    public static NumericPropertyBuilder Numeric(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<NumericPropertyBuilder>(propertyAlias);
    }
    
    public static RadioButtonListPropertyBuilder RadioButtonList(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<RadioButtonListPropertyBuilder>(propertyAlias);
    }
    
    public static RawPropertyBuilder Raw(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<RawPropertyBuilder>(propertyAlias);
    }
    
    public static TemplatedLabelPropertyBuilder TemplatedLabel(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<TemplatedLabelPropertyBuilder>(propertyAlias);
    }

    public static TextareaPropertyBuilder Textarea(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<TextareaPropertyBuilder>(propertyAlias);
    }
    
    public static TextBoxPropertyBuilder TextBox(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<TextBoxPropertyBuilder>(propertyAlias);
    }
    
    public static TogglePropertyBuilder Toggle(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<TogglePropertyBuilder>(propertyAlias);
    }
}
