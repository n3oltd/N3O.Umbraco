using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks {
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
            if (_contentTypeService.Get(definition.Id) != null) {
                return;
            }

            var rootContainer = GetOrCreateContentTypeContainer(UmbracoId.Generate(IdScope.ContentTypeContainer, "Blocks"),
                                                                -1,
                                                                "Blocks");

            var container = rootContainer;

            if (definition.BlockCategories.IsSingle()) {
                var category = definition.BlockCategories.Single();
                var containerId = UmbracoId.Generate(IdScope.ContentTypeContainer, container.Key, category.Id);
                
                container = GetOrCreateContentTypeContainer(containerId, container.Id, category.Name);
            }

            if (definition.Folder.HasValue()) {
                var containerId = UmbracoId.Generate(IdScope.ContentTypeContainer, container.Key, definition.Folder);
                
                container = GetOrCreateContentTypeContainer(containerId, container.Id, definition.Folder);
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
            var id = UmbracoId.Generate(IdScope.ContentType, container.Key);
            var contentType = _contentTypeService.Get(id);

            if (contentType == null) {
                var dataType = _dataTypeService.GetDataType("Textarea");
                
                var propertyType = new PropertyType(_shortStringHelper, dataType);
                propertyType.Alias = "notes";
                propertyType.Name = "Notes";
                propertyType.Description = "Only visible to other editors, not displayed on website";
                propertyType.SortOrder = 999;

                contentType = new ContentType(_shortStringHelper, container.Id);
                contentType.Key = id;
                contentType.Alias = "block";
                contentType.IsElement = true;
                contentType.Name = "Block";
                contentType.Icon = "icon-brick";
                contentType.AddPropertyType(propertyType, "content", "Content");
                
                _contentTypeService.Save(contentType);
            }

            return contentType;
        }

        public void CreateDataTypes(BlockDefinition definition) {
            var dataTypeKey = definition.DataTypeKey.Value;
        
            if (_dataTypeService.GetDataType(dataTypeKey) != null) {
                return;
            }

            if (!_propertyEditors.TryGet(PropertyEditors.Aliases.NestedContent, out var editor)) {
                throw new InvalidOperationException("Nested Content property editor not found");
            }

            var container = GetOrCreateDataTypeContainer(UmbracoId.Generate(IdScope.DataTypeContainer, "Blocks"),
                                                         -1,
                                                         "Blocks");

            if (definition.BlockCategories.IsSingle()) {
                var category = definition.BlockCategories.Single();
                var containerId = UmbracoId.Generate(IdScope.DataTypeContainer, container.Key, category.Id);
                
                container = GetOrCreateDataTypeContainer(containerId, container.Id, category.Name);
            }

            if (definition.Folder.HasValue()) {
                var containerId = UmbracoId.Generate(IdScope.DataTypeContainer, container.Key, definition.Folder);
                
                container = GetOrCreateDataTypeContainer(containerId, container.Id, definition.Folder);
            }
            
            var dataType = new DataType(editor, _configurationEditorJsonSerializer, container.Id);
            
            dataType.Name = $"{definition.Name} Block";
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
                    Template = definition.Name,
                }
            };

            return configuration;
        }
        
        private EntityContainer GetOrCreateContentTypeContainer(Guid id, int parentId, string name) {
            var container = _contentTypeService.GetContainer(id);

            if (container == null) {
                var attempt = _contentTypeService.CreateContainer(parentId, id, name);

                if (!attempt.Success) {
                    throw new Exception($"Failed to create blocks container {name.Quote()}");
                }

                container = attempt.Result.Entity;
            }

            return container;
        }
        
        private EntityContainer GetOrCreateDataTypeContainer(Guid id, int parentId, string name) {
            var container = _dataTypeService.GetContainer(id);

            if (container == null) {
                var attempt = _dataTypeService.CreateContainer(parentId, id, name);

                if (!attempt.Success) {
                    throw new Exception($"Failed to create blocks container {name.Quote()}");
                }

                container = attempt.Result.Entity;
            }

            return container;
        }
    }
}