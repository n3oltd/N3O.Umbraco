using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class PropertyContentReqExtensions {
    public static bool HasPropertyValue(this PropertyContentReq propertyContent) {
        if (propertyContent.Editor == PropertyEditor.Boolean) {
            return propertyContent.Boolean.HasValue(x => x.Value);
        } else if (propertyContent.Editor == PropertyEditor.DateTime) {
            return propertyContent.DateTime.HasValue(x => x.Value);
        } else if (propertyContent.Editor == PropertyEditor.EditorJs) {
            return propertyContent.EditorJs.HasAny(x => x.Blocks);
        } else if (propertyContent.Editor == PropertyEditor.ImageSimple) {
            return propertyContent.ImageSimple.HasValue(x => x.SourceFile);
        } else if (propertyContent.Editor == PropertyEditor.NestedContentMultiple) {
            return propertyContent.NestedContentMultiple.HasAny(x => x.Elements);
        } else if (propertyContent.Editor == PropertyEditor.NestedContentSingle) {
            return propertyContent.NestedContentSingle.HasValue(x => x.Element);
        } else if (propertyContent.Editor == PropertyEditor.RichText) {
            return propertyContent.RichText.HasValue(x => x.Html);
        } else if (propertyContent.Editor == PropertyEditor.Svg) {
            return propertyContent.Svg.HasValue(x => x.SourceFile);
        } else if (propertyContent.Editor == PropertyEditor.Textarea) {
            return propertyContent.Textarea.HasValue(x => x.Text);
        } else if (propertyContent.Editor == PropertyEditor.Textbox) {
            return propertyContent.Textbox.HasValue(x => x.Text);
        } else if (propertyContent.Editor == PropertyEditor.VideoSimple) {
            return propertyContent.VideoSimple.HasValue(x => x.SourceFile);
        }
        
        return false;
    }
    
}