using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlockTypesService : IPerplexBlockTypesService {
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IContentTypeService _contentTypeService;
    private readonly PropertyEditorCollection _propertyEditors;
    private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
    private readonly IDataTypeService _dataTypeService;

    public PerplexBlockTypesService(IShortStringHelper shortStringHelper,
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

    public void CreateTypes(PerplexBlockDefinition definition) {
        CreateContentType(definition);
        CreateDataTypes(definition);
    }

    private void CreateContentType(PerplexBlockDefinition definition) {
        if (_contentTypeService.Get(definition.Alias) != null) {
            return;
        }

        var rootContainer = GetOrCreateContentTypeContainer("Blocks");

        var container = rootContainer;

        if (definition.BlockCategories.IsSingle()) {
            var category = definition.BlockCategories.Single();
            
            container = GetOrCreateContentTypeContainer(category.Name, rootContainer.Name);
        }

        if (definition.Folder.HasValue()) {
            if (container == rootContainer) {
                container = GetOrCreateContentTypeContainer(definition.Folder, rootContainer.Name);
            } else {
                container = GetOrCreateContentTypeContainer(definition.Folder, container.Name, rootContainer.Name);
            }
        }

        var compositionType = GetOrCreateContentTypeComposition(rootContainer);

        var contentType = new ContentType(_shortStringHelper, container.Id);
        contentType.Key = definition.Id;
        contentType.Alias = definition.Alias;
        contentType.IsElement = true;
        contentType.Name = definition.Name;
        contentType.Icon = definition.Icon;
        contentType.PropertyGroups = new PropertyGroupCollection();
        contentType.ContentTypeComposition = new[] { compositionType };

        _contentTypeService.Save(contentType);
    }

    private IContentType GetOrCreateContentTypeComposition(EntityContainer container) {
        var alias = "block";
        var name = "Block";
        var contentType = _contentTypeService.Get(alias);

        if (contentType == null) {
            var dataType = _dataTypeService.GetDataType("Textarea");
            
            var propertyType = new PropertyType(_shortStringHelper, dataType);
            propertyType.Alias = "notes";
            propertyType.Name = "Notes";
            propertyType.Description = "Only visible to other editors, not displayed on website";
            propertyType.SortOrder = 999;

            contentType = new ContentType(_shortStringHelper, container.Id);
            contentType.Key = UmbracoId.Generate(IdScope.ContentType, name);
            contentType.Alias = alias;
            contentType.IsElement = true;
            contentType.Name = name;
            contentType.Icon = "icon-brick";
            contentType.AddPropertyType(propertyType, "general", "General");
            
            _contentTypeService.Save(contentType);
        }

        return contentType;
    }

    private void CreateDataTypes(PerplexBlockDefinition definition) {
        var dataTypeKey = definition.DataTypeKey.Value;
    
        if (_dataTypeService.GetDataType(dataTypeKey) != null) {
            return;
        }

        if (!_propertyEditors.TryGet(global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.NestedContent,
                                     out var editor)) {
            throw new InvalidOperationException("Nested Content property editor not found");
        }

        var rootContainer = GetOrCreateDataTypeContainer("Blocks");

        var container = rootContainer;
        
        if (definition.BlockCategories.IsSingle()) {
            var category = definition.BlockCategories.Single();
            
            container = GetOrCreateDataTypeContainer(category.Name, rootContainer.Name);
        }

        if (definition.Folder.HasValue()) {
            if (container == rootContainer) {
                container = GetOrCreateDataTypeContainer(definition.Folder, rootContainer.Name);
            } else {
                container = GetOrCreateDataTypeContainer(definition.Folder, container.Name, rootContainer.Name);
            }
        }
        
        var dataType = new DataType(editor, _configurationEditorJsonSerializer, container.Id);
        
        dataType.Name = $"{definition.Name} Block";
        dataType.Key = dataTypeKey;
        dataType.Configuration = GetNestedContentConfiguration(definition);

        _dataTypeService.Save(dataType);
    }

    private NestedContentConfiguration GetNestedContentConfiguration(PerplexBlockDefinition definition) {
        var configuration = new NestedContentConfiguration();
    
        configuration.ConfirmDeletes = false;
        configuration.HideLabel = true;
        configuration.MinItems = 1;
        configuration.MaxItems = 1;
        configuration.ShowIcons = false;
        configuration.ContentTypes = new[] {
            new NestedContentConfiguration.ContentType {
                Alias = definition.Alias,
                TabAlias = "General",
                Template = definition.Name,
            }
        };

        return configuration;
    }
    
    private EntityContainer GetOrCreateContentTypeContainer(string name, params string[] path) {
        var container = default(EntityContainer);
        
        foreach (var element in path.Concat(name)) {
            EntityContainer elementContainer;
            
            if (container == null) {
                elementContainer = _contentTypeService.GetContainers(element, 1).SingleOrDefault();
            } else {
                elementContainer = _contentTypeService.GetContainers(element, container.Level + 1)
                                                      .SingleOrDefault(x => x.ParentId == container.Id);
            }
            
            if (elementContainer == null) {
                var attempt = _contentTypeService.CreateContainer(container?.Id ?? -1,
                                                                  UmbracoId.Generate(IdScope.ContentTypeContainer, name),
                                                                  name);

                if (!attempt.Success) {
                    throw new Exception($"Failed to create blocks container {name.Quote()}");
                }

                container = attempt.Result.Entity;
            } else {
                container = elementContainer;
            }
        }

        return container;
    }
    
    private EntityContainer GetOrCreateDataTypeContainer(string name, params string[] path) {
        var container = default(EntityContainer);
        
        foreach (var element in path.Concat(name)) {
            EntityContainer elementContainer;
            
            if (container == null) {
                elementContainer = _dataTypeService.GetContainers(element, 1).SingleOrDefault();
            } else {
                elementContainer = _dataTypeService.GetContainers(element, container.Level + 1)
                                                   .SingleOrDefault(x => x.ParentId == container.Id);
            }
            
            if (elementContainer == null) {
                var attempt = _dataTypeService.CreateContainer(container?.Id ?? -1,
                                                               UmbracoId.Generate(IdScope.DataTypeContainer, name),
                                                               name);

                if (!attempt.Success) {
                    throw new Exception($"Failed to create blocks container {name.Quote()}");
                }

                container = attempt.Result.Entity;
            } else {
                container = elementContainer;
            }
        }

        return container;
    }
}
