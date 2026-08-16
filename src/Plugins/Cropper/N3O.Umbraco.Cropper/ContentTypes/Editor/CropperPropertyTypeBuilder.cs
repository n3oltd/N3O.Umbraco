using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Cropper.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.ContentTypes;

public class CropperPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<CropperPropertyTypeBuilder, CropperDataTypeDesigner> {
    private readonly List<(string Alias, string Label, int Width, int Height)> _crops = [];

    private bool _altText;

    public CropperPropertyTypeBuilder(IDataTypeService dataTypeService, CropperDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    public CropperPropertyTypeBuilder AddCrop(string alias, string label, int width, int height) {
        _crops.Add((alias, label, width, height));

        return this;
    }

    public CropperPropertyTypeBuilder WithAltText() {
        _altText = true;

        return this;
    }

    protected override void ConfigureDataType(CropperDataTypeDesigner dataTypeDesigner) {
        foreach (var (alias, label, width, height) in _crops) {
            dataTypeDesigner.AddCrop(alias, label, width, height);
        }

        if (_altText) {
            dataTypeDesigner.WithAltText();
        }
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return BuildInlineDataType(context);
    }
}
