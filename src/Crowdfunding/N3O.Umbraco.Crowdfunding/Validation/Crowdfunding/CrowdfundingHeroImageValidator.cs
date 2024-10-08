using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingHeroImageValidator : ContentPropertyValidator<CropperValueReq, CropperConfigurationRes> {
    private readonly IDataTypeService _dataTypeService;

    public CrowdfundingHeroImageValidator(IFormatter formatter, IDataTypeService dataTypeService)
        : base(formatter, CrowdfundingConstants.HeroImages.Alias, CrowdfundingConstants.HeroImages.Properties.Image) {
        _dataTypeService = dataTypeService;
    }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, CropperConfigurationRes res) {
        var cropperDefinition = GetCropDefinition(property.DataTypeId);
        
        res.Description = property.Description;
        res.Rectangle = new RectangleCropConfigurationRes();
        res.Rectangle.Height = cropperDefinition.Height;
        res.Rectangle.Width = cropperDefinition.Width;
    }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, CropperValueReq req) {
        var property = content.Properties.SingleOrDefault(x => x.Alias == propertyAlias);
        var cropperDefinition = GetCropDefinition(property.PropertyType.DataType.Id);
        
        var height = CropperShapeExtensions.GetHeight(req.Rectangle.TopRight.Y, req.Rectangle.BottomLeft.Y);
        var width = CropperShapeExtensions.GetWidth(req.Rectangle.TopRight.X, req.Rectangle.BottomLeft.X);
        
        if (width < cropperDefinition.Width || 
            height < cropperDefinition.Height) {
            AddFailure<Strings>(propertyAlias, x => x.MinimumSize);
        }
    }

    private CropDefinition GetCropDefinition(int dataTypeId) {
        var cropperConfiguration = _dataTypeService.GetDataType(dataTypeId)?.ConfigurationAs<CropperConfiguration>();

        return cropperConfiguration.CropDefinitions.FirstOrDefault();
    }
    
    public class Strings : ValidationStrings {
        public string MinimumSize => "Crop does not meet minimum required dimensions.";
    }
}