using N3O.Umbraco.DataTypes;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Uploader.DataTypes;

public class UploaderDataTypeDesigner : DataTypeDesigner {
    private string _allowedExtensions;
    private bool _altTextRequired;
    private bool _imagesOnly;
    private string _maxFileSizeMb;

    public UploaderDataTypeDesigner(IDataTypeService dataTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public UploaderDataTypeDesigner AllowExtensions(params string[] extensions) {
        _allowedExtensions = extensions.ToCsv();

        return this;
    }

    public UploaderDataTypeDesigner ImagesOnly() {
        _imagesOnly = true;

        return this;
    }

    public UploaderDataTypeDesigner MaxFileSizeMb(int maxFileSizeMb) {
        _maxFileSizeMb = maxFileSizeMb.ToString();

        return this;
    }

    public UploaderDataTypeDesigner RequireAltText() {
        _altTextRequired = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new UploaderConfiguration();

        configuration.AltTextRequired = _altTextRequired;
        configuration.ImagesOnly = _imagesOnly;

        if (_allowedExtensions.HasValue()) {
            configuration.AllowedExtensions = _allowedExtensions;
        }

        if (_maxFileSizeMb.HasValue()) {
            configuration.MaxFileSizeMb = _maxFileSizeMb;
        }

        return configuration;
    }

    protected override string EditorAlias => UploaderConstants.PropertyEditorAlias;
}
