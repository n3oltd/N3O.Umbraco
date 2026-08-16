using N3O.Umbraco.ContentTypes;
using System;
using System.Linq.Expressions;

namespace N3O.Umbraco.Extensions;

public static class PropertyContainerBuilderExtensions {
    public static BlockListPropertyTypeBuilder BlockList(this IPropertyContainerBuilder builder,
                                                         string propertyAlias) {
        return builder.Property<BlockListPropertyTypeBuilder>(propertyAlias);
    }

    public static BlockListPropertyTypeBuilder BlockList<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                       Expression<Func<T, TProperty>> expression) {
        return builder.Property<BlockListPropertyTypeBuilder, TProperty>(expression);
    }

    public static ContentPickerPropertyTypeBuilder ContentPicker(this IPropertyContainerBuilder builder,
                                                                 string propertyAlias) {
        return builder.Property<ContentPickerPropertyTypeBuilder>(propertyAlias);
    }

    public static ContentPickerPropertyTypeBuilder ContentPicker<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                               Expression<Func<T, TProperty>> expression) {
        return builder.Property<ContentPickerPropertyTypeBuilder, TProperty>(expression);
    }

    public static DateTimePropertyTypeBuilder DateTime(this IPropertyContainerBuilder builder,
                                                       string propertyAlias) {
        return builder.Property<DateTimePropertyTypeBuilder>(propertyAlias);
    }

    public static DateTimePropertyTypeBuilder DateTime<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                     Expression<Func<T, TProperty>> expression) {
        return builder.Property<DateTimePropertyTypeBuilder, TProperty>(expression);
    }

    public static DropdownPropertyTypeBuilder Dropdown(this IPropertyContainerBuilder builder,
                                                       string propertyAlias) {
        return builder.Property<DropdownPropertyTypeBuilder>(propertyAlias);
    }

    public static DropdownPropertyTypeBuilder Dropdown<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                     Expression<Func<T, TProperty>> expression) {
        return builder.Property<DropdownPropertyTypeBuilder, TProperty>(expression);
    }

    public static NumericPropertyTypeBuilder Numeric(this IPropertyContainerBuilder builder,
                                                     string propertyAlias) {
        return builder.Property<NumericPropertyTypeBuilder>(propertyAlias);
    }

    public static NumericPropertyTypeBuilder Numeric<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                   Expression<Func<T, TProperty>> expression) {
        return builder.Property<NumericPropertyTypeBuilder, TProperty>(expression);
    }

    public static TextareaPropertyTypeBuilder Textarea(this IPropertyContainerBuilder builder,
                                                       string propertyAlias) {
        return builder.Property<TextareaPropertyTypeBuilder>(propertyAlias);
    }

    public static TextareaPropertyTypeBuilder Textarea<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                     Expression<Func<T, TProperty>> expression) {
        return builder.Property<TextareaPropertyTypeBuilder, TProperty>(expression);
    }

    public static TextBoxPropertyTypeBuilder TextBox(this IPropertyContainerBuilder builder,
                                                     string propertyAlias) {
        return builder.Property<TextBoxPropertyTypeBuilder>(propertyAlias);
    }

    public static TextBoxPropertyTypeBuilder TextBox<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                   Expression<Func<T, TProperty>> expression) {
        return builder.Property<TextBoxPropertyTypeBuilder, TProperty>(expression);
    }

    public static TogglePropertyTypeBuilder Toggle(this IPropertyContainerBuilder builder,
                                                   string propertyAlias) {
        return builder.Property<TogglePropertyTypeBuilder>(propertyAlias);
    }

    public static TogglePropertyTypeBuilder Toggle<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                 Expression<Func<T, TProperty>> expression) {
        return builder.Property<TogglePropertyTypeBuilder, TProperty>(expression);
    }
}
