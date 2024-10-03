using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Cropper.Notifications;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderOpenGraphImageCropperHandler : OpenGraphImageCropperHandler {
    private readonly Lazy<IContentHelper> _contentHelper;
    private readonly Lazy<IUrlBuilder> _urlBuilder;
    
    public CrowdfunderOpenGraphImageCropperHandler(Lazy<IContentHelper> contentHelper,
                                                   Lazy<IImageCropper> imageCropper,
                                                   Lazy<IUrlBuilder> urlBuilder) 
        : base(contentHelper, imageCropper) {
        _contentHelper = contentHelper;
        _urlBuilder = urlBuilder;
    }

    protected override bool ShouldExecute(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias) ||
               content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);
    }

    protected override CroppedImage GetImage(ContentProperties contentProperties) {
        var heroImages = contentProperties.NestedContentProperties
                                         .Single(x => x.Alias == CrowdfundingConstants.Crowdfunder.Properties.HeroImages);
        
        var heroImage = heroImages.Value
                                   .First(x => x.ContentTypeAlias == CrowdfundingConstants.HeroImages.Alias)
                                   .GetPropertyByAlias(CrowdfundingConstants.HeroImages.Properties.Image);
        
        var croppedImage = _contentHelper.Value.GetCroppedImage(heroImage);

        return croppedImage;
    }

    protected override void PopulateImagePath(IContent content, string imagePath) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.OpenGraphImageUrl,
                         _urlBuilder.Value.Root().AppendPathSegment(imagePath));
    }
}