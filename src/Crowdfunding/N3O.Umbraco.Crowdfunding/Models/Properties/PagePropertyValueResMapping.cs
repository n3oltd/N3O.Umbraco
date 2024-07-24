using N3O.Umbraco.Cropper;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PagePropertyValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, PagePropertyValueRes>((_, _) => new PagePropertyValueRes(), Map);
    }

    private void Map(IPublishedProperty src, PagePropertyValueRes dest, MapperContext ctx) {
        switch (src.PropertyType.EditorAlias) {
            case UmbracoPropertyEditors.Aliases.Boolean:
                dest.Boolean = ctx.Map<IPublishedProperty, BooleanValueRes>(src);
                break;
            
            case CropperConstants.PropertyEditorAlias:
                dest.Cropper = ctx.Map<IPublishedProperty, CropperValueRes>(src);
                break;
            
            case UmbracoPropertyEditors.Aliases.Decimal:
                dest.Numeric = ctx.Map<IPublishedProperty, NumericValueRes>(src);
                break;
            
            case UmbracoPropertyEditors.Aliases.DateTime:
                dest.DateTime = ctx.Map<IPublishedProperty, DateTimeValueRes>(src);
                break;
            
            case UmbracoPropertyEditors.Aliases.TinyMce:
                dest.Raw = ctx.Map<IPublishedProperty, RawValueRes>(src);
                break;
            
            case UmbracoPropertyEditors.Aliases.TextArea:
                dest.Textarea = ctx.Map<IPublishedProperty, TextareaValueRes>(src);
                break;
            
            case UmbracoPropertyEditors.Aliases.TextBox:
                dest.TextBox = ctx.Map<IPublishedProperty, TextBoxValueRes>(src);
                break;
        }
    }
}