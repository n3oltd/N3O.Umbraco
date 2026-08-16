using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class FileUploadDataTypeDesigner : DataTypeDesigner {
    private readonly List<string> _fileExtensions = [];

    public FileUploadDataTypeDesigner(IDataTypeService dataTypeService,
                                      PropertyEditorCollection propertyEditors,
                                      IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public FileUploadDataTypeDesigner AllowExtensions(params string[] fileExtensions) {
        _fileExtensions.AddRange(fileExtensions);

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new FileUploadConfiguration();

        configuration.FileExtensions = _fileExtensions.Select((extension, index) => {
                                                          var item = new FileExtensionConfigItem();

                                                          item.Id = index + 1;
                                                          item.Value = extension;

                                                          return item;
                                                      })
                                                      .ToList();

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.UploadField;
}
