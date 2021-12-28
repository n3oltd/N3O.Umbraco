using N3O.Umbraco.Constants;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks; 

public class BlockTypesService : IBlockTypesService {
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IContentTypeService _contentTypeService;
    private readonly PropertyEditorCollection _propertyEditors;
    private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
    private readonly IDataTypeService _dataTypeService;

    public BlockTypesService(IShortStringHelper shortStringHelper,
                             IContentTypeService contentTypeService,
                             PropertyEditorCollection propertyEditors,
                             IConfigurationEditorJsonSerializer configurationEditorJsonSerializer,
                             IDataTypeService dataTypeService) {
        _shortStringHelper = shortStringHelper;
        _contentTypeService = contentTypeService;
        _propertyEditors = propertyEditors;
        _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
        _dataTypeService = dataTypeService;
    }
    
    public void CreateTypes(BlockDefinition definition) {
        CreateContentType(definition);
        CreateDataTypes(definition);
    }
    
    private void CreateContentType(BlockDefinition definition) {
        if (_contentTypeService.Get(definition.Alias) != null) {
            return;
        }

        var contentType = new ContentType(_shortStringHelper, -1);
        contentType.Alias = definition.Alias;
        contentType.IsElement = true;
        contentType.Name = definition.Name;
        contentType.PropertyGroups = new PropertyGroupCollection();

        _contentTypeService.Save(contentType);
    }
    
    public void CreateDataTypes(BlockDefinition definition) {
        var dataTypeKey = definition.Id;
        
        if (_dataTypeService.GetDataType(dataTypeKey) != null) {
            return;
        }

        if (!_propertyEditors.TryGet(PropertyEditors.Aliases.NestedContent, out var editor)) {
            throw new InvalidOperationException("Nested Content property editor not found!");
        }

        var dataType = new DataType(editor, _configurationEditorJsonSerializer);

        dataType.Name = $"Block - {definition.Name}";
        dataType.Key = dataTypeKey;
        dataType.Configuration = GetNestedContentConfiguration(definition);

        _dataTypeService.Save(dataType);
    }

    private NestedContentConfiguration GetNestedContentConfiguration(BlockDefinition definition) {
        var configuration = new NestedContentConfiguration();
        
        configuration.ConfirmDeletes = false;
        configuration.HideLabel = true;
        configuration.MinItems = 1;
        configuration.MaxItems = 1;
        configuration.ShowIcons = false;
        configuration.ContentTypes = new[] {
            new NestedContentConfiguration.ContentType {
                Alias = definition.Alias,
                TabAlias = "Content",
                Template = "{{title}}"
            }
        };

        return configuration;
    }
}