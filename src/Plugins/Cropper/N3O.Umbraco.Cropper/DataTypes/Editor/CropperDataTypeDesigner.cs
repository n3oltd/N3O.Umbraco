using N3O.Umbraco.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.DataTypes;

public class CropperDataTypeDesigner : DataTypeDesigner {
    private readonly List<CropDefinition> _crops = [];

    private bool _altText;

    public CropperDataTypeDesigner(IDataTypeService dataTypeService,
                                   PropertyEditorCollection propertyEditors,
                                   IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public CropperDataTypeDesigner AddCrop(string alias, string label, int width, int height) {
        var crop = new CropDefinition();

        crop.Alias = alias;
        crop.Label = label;
        crop.Width = width;
        crop.Height = height;

        _crops.Add(crop);

        return this;
    }

    public CropperDataTypeDesigner WithAltText() {
        _altText = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new CropperConfiguration();

        configuration.AltText = _altText;
        configuration.CropDefinitions = _crops;

        return configuration;
    }

    protected override string EditorAlias => CropperConstants.PropertyEditorAlias;
}
