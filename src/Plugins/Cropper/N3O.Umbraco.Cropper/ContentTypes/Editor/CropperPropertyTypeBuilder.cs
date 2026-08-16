using Humanizer;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Cropper.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.ContentTypes;

public class CropperPropertyTypeBuilder : PropertyTypeBuilder<CropperPropertyTypeBuilder> {
    private readonly CropperDataTypeDesigner _dataTypeDesigner;
    private readonly List<(string Alias, string Label, int Width, int Height)> _crops = [];

    private bool _altText;

    public CropperPropertyTypeBuilder(IDataTypeService dataTypeService, CropperDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService) {
        _dataTypeDesigner = dataTypeDesigner;
    }

    public CropperPropertyTypeBuilder AddCrop(string alias, string label, int width, int height) {
        _crops.Add((alias, label, width, height));

        return this;
    }

    public CropperPropertyTypeBuilder WithAltText() {
        _altText = true;

        return this;
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        _dataTypeDesigner.SetName($"{context.ContentTypeAlias.Titleize()} {context.PropertyAlias.Titleize()}");
        _dataTypeDesigner.WithoutNameAdoption();

        foreach (var (alias, label, width, height) in _crops) {
            _dataTypeDesigner.AddCrop(alias, label, width, height);
        }

        if (_altText) {
            _dataTypeDesigner.WithAltText();
        }

        if (context.UseDeterministicIds) {
            _dataTypeDesigner.WithDeterministicId($"{context.ContentTypeAlias}_{context.PropertyAlias}");
        }

        return _dataTypeDesigner.Save();
    }
}
