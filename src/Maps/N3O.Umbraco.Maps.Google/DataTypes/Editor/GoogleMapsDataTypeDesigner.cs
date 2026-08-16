using N3O.Umbraco.DataTypes;
using N3O.Umbraco.Extensions;
using Our.Umbraco.GMaps.Core.Models.Configuration;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Maps.DataTypes;

public class GoogleMapsDataTypeDesigner : DataTypeDesigner {
    private string _apiKey;
    private string _location;
    private string _zoom;

    public GoogleMapsDataTypeDesigner(IDataTypeService dataTypeService,
                                      PropertyEditorCollection propertyEditors,
                                      IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public GoogleMapsDataTypeDesigner ApiKey(string apiKey) {
        _apiKey = apiKey;

        return this;
    }

    public GoogleMapsDataTypeDesigner Location(string coordinates) {
        _location = coordinates;

        return this;
    }

    public GoogleMapsDataTypeDesigner Zoom(int zoom) {
        _zoom = zoom.ToString();

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new Config();

        configuration.ApiKey = _apiKey;

        if (_location.HasValue()) {
            configuration.Location = _location;
        }

        if (_zoom.HasValue()) {
            configuration.Zoom = _zoom;
        }

        return configuration;
    }

    protected override string EditorAlias => "Our.Umbraco.GMaps";
}
