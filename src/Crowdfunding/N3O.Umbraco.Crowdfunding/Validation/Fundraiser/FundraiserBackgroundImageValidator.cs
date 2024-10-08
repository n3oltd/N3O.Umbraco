using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserBackgroundImageValidator : ContentPropertyValidator<CropperValueReq, CropperConfigurationRes> {
    private const int MaxLength = 1000;
    private readonly IDataTypeService _dataTypeService;
    
    public FundraiserBackgroundImageValidator(IFormatter formatter, IDataTypeService dataTypeService)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Crowdfunder.Properties.BackgroundImage) {
        _dataTypeService = dataTypeService;
    }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, CropperConfigurationRes res) {
        var cropperConfiguration = _dataTypeService.GetDataType(property.DataTypeId)?.ConfigurationAs<CropperConfiguration>();
        
        res.Description = property.Description;
        res.Rectangle = new RectangleCropConfigurationRes();
        res.Rectangle.Height = cropperConfiguration.CropDefinitions.FirstOrDefault().Height;
        res.Rectangle.Width = cropperConfiguration.CropDefinitions.FirstOrDefault().Width;
    }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, CropperValueReq req) {
        var cropperConfiguration = content.Properties.SingleOrDefault(x => x.Alias == propertyAlias).PropertyType.DataType.ConfigurationAs<CropperConfiguration>();
        
        
        
        var topRight = req.Rectangle.TopRight;
        var bottomLeft = req.Rectangle.BottomLeft;
            
        var width = topRight.X.GetValueOrThrow() - bottomLeft.X.GetValueOrThrow();
        var height = topRight.Y.GetValueOrThrow() - bottomLeft.Y.GetValueOrThrow();
        
        if (width < cropperConfiguration.CropDefinitions.FirstOrDefault().Width ||
            height < cropperConfiguration.CropDefinitions.FirstOrDefault().Height) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Body cannot exceed {"{0}".Quote()} characters.";
    }
}