using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Operations;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.ImageProcessing.TagHelpers;

[HtmlTargetElement(Attributes = $"{SrcAttributeName},{AspectRatioAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(Attributes = $"{SrcAttributeName},{TargetHeightAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(Attributes = $"{SrcAttributeName},{TargetWidthAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(Attributes = $"{SrcAttributeName},{AspectRatioAttributeName},{TargetHeightAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(Attributes = $"{SrcAttributeName},{AspectRatioAttributeName},{TargetWidthAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(Attributes = $"{SrcAttributeName},{AspectRatioAttributeName},{TargetHeightAttributeName},{TargetWidthAttributeName}", TagStructure = TagStructure.WithoutEndTag)]
public class ImageProcessingTagHelper : TagHelper {
    private const string AspectRatioAttributeName = $"{Prefixes.TagHelpers}aspect-ratio";
    private const string TargetHeightAttributeName = $"{Prefixes.TagHelpers}target-height";
    private const string SrcAttributeName = "src";
    private const string TargetWidthAttributeName = $"{Prefixes.TagHelpers}target-width";
    
    private readonly IImagePublisher _imagePublisher;

    public ImageProcessingTagHelper(IImagePublisher imagePublisher) {
        _imagePublisher = imagePublisher;
    }
    
    [HtmlAttributeName(AspectRatioAttributeName)]
    public decimal? AspectRatio { get; set; }
    
    [HtmlAttributeName(SrcAttributeName)]
    public string Src { get; set; }
    
    [HtmlAttributeName(TargetHeightAttributeName)]
    public int? TargetHeight { get; set; }
    
    [HtmlAttributeName(TargetWidthAttributeName)]
    public int? TargetWidth { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var img = _imagePublisher.Publish(GenerateCacheKey, BuildImage, ImageFormats.Gif);
        
        output.Attributes.SetAttribute(SrcAttributeName, img.Url.Path);
        output.Attributes.SetAttribute("width", img.Size.Width);
        output.Attributes.SetAttribute("height", img.Size.Height);
    }

    private void GenerateCacheKey(CacheKeyBuilder cacheKey) {
        cacheKey.Append(Src);
        cacheKey.Append(AspectRatio);
        cacheKey.Append(TargetHeight);
        cacheKey.Append(TargetWidth);
    }
    
    private IFluentImageBuilder BuildImage(IImageBuilder imageBuilder) {
        var image = imageBuilder.Create(Src);

        image.Processor.ApplyOperation(new AutoOrientOperation());
        
        if (AspectRatio.HasValue()) {
            image.Processor.ApplyOperation(new CropToAspectRatioOperation(AspectRatio.GetValueOrThrow()));
        }

        image.Processor.ApplyOperation(new ResizeOperation(TargetWidth, TargetHeight));
        image.Processor.ApplyOperation(new ConstrainSizeOperation(2000, 2000));

        return image;
    }
}
