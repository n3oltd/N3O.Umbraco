using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlockTypesService : IPerplexBlockTypesService {
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;

    public PerplexBlockTypesService(IShortStringHelper shortStringHelper,
                                    IContentTypeService contentTypeService,
                                    IDataTypeService dataTypeService) {
        _shortStringHelper = shortStringHelper;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
    }

    public async Task CreateTypesAsync(PerplexBlockDefinition definition) {
        await CreateContentTypeAsync(definition);
    }

    private async Task CreateContentTypeAsync(PerplexBlockDefinition definition) {
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

        var compositionType = await GetOrCreateContentTypeCompositionAsync(rootContainer);

        var contentType = new ContentType(_shortStringHelper, container.Id);
        contentType.Key = definition.Id;
        contentType.Alias = definition.Alias;
        contentType.IsElement = true;
        contentType.Name = definition.Name;
        contentType.Icon = definition.Icon;
        contentType.PropertyGroups = [];
        contentType.ContentTypeComposition = [compositionType];

        await _contentTypeService.CreateAsync(contentType, global::Umbraco.Cms.Core.Constants.Security.SuperUserKey);
    }

    private async Task<IContentType> GetOrCreateContentTypeCompositionAsync(EntityContainer container) {
        var alias = "block";
        var name = "Block";
        var contentType = _contentTypeService.Get(alias);

        if (contentType == null) {
            var dataType = await _dataTypeService.GetAsync("Textarea");

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

            await _contentTypeService.CreateAsync(contentType, global::Umbraco.Cms.Core.Constants.Security.SuperUserKey);
        }

        return contentType;
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
}
