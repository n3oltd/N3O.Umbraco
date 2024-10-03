using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Cropper.Notifications;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderOpenGraphImageCropperHandler : OpenGraphImageCropperHandler {
    private readonly Lazy<IContentHelper> _contentHelper;
    
    public CrowdfunderOpenGraphImageCropperHandler(Lazy<IContentHelper> contentHelper, Lazy<IImageCropper> imageCropper) 
        : base(contentHelper, imageCropper) {
        _contentHelper = contentHelper;
    }

    protected override bool ShouldExecute(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias) ||
               content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);
    }

    protected override CroppedImage GetImage(ContentProperties contentProperties) {
        var backgroundImage = contentProperties.GetPropertyByAlias(CrowdfundingConstants.Crowdfunder.Properties.BackgroundImage);
        
        var croppedImage = _contentHelper.Value.GetCroppedImage(backgroundImage);

        return croppedImage;
    }

    protected override void PopulateImagePath(IContent content, string imagePath) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.OpenGraphImagePath, imagePath);
    }
}