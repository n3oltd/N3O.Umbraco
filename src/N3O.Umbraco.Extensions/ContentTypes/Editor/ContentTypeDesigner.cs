using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.ContentTypes;

public abstract class ContentTypeDesigner : IContentTypeDesigner {
    private readonly IContentTypeService _contentTypeService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly bool _isElement;
    private readonly List<string> _compositions = [];
    private readonly List<PropertyContainerBuilder> _containers = [];

    private string _description;
    private bool _deterministic;
    private string[] _folderPath = [];
    private string _icon;
    private Guid? _id;
    private string _name;
    private bool _overwriteDescription;
    private bool _overwriteIcon;
    private bool _overwriteName;
    private bool _varyByCulture;
    private bool _varyBySegment;

    protected ContentTypeDesigner(IServiceProvider serviceProvider,
                                  IContentTypeService contentTypeService,
                                  IShortStringHelper shortStringHelper,
                                  string name,
                                  string alias,
                                  bool isElement) {
        ServiceProvider = serviceProvider;
        _contentTypeService = contentTypeService;
        _shortStringHelper = shortStringHelper;
        _name = name;
        Alias = alias;
        _isElement = isElement;
    }

    public void AddComposition(string contentTypeAlias) {
        _compositions.Add(contentTypeAlias);
    }

    public void AddComposition<T>() {
        AddComposition(AliasHelper.ContentTypeAlias(typeof(T)));
    }

    public IPropertyContainerBuilder Group(string name) {
        var container = new PropertyContainerBuilder(ServiceProvider, _shortStringHelper, name, false, null);

        RegisterContainer(container);

        return container;
    }

    public void InFolder(params string[] path) {
        _folderPath = path;
    }

    public IContentType Save() {
        var existing = FindExisting();
        var contentType = existing ?? Create();

        // The kind decides where the type's content can live, so a site that holds it as the other one is
        // not converged by reassigning the flag; that would convert the type under everything already using
        // it. Refusing here names the disagreement and leaves the rest of the type untouched
        if (existing == null) {
            contentType.IsElement = _isElement;
        } else if (existing.IsElement != _isElement) {
            throw new ContentTypeKindMismatchException(Alias, existing.IsElement);
        }

        if (existing == null || _overwriteName) {
            contentType.Name = _name;
        }

        if (_icon.HasValue() && (existing == null || _overwriteIcon)) {
            contentType.Icon = _icon;
        }

        if (_description.HasValue() && (existing == null || _overwriteDescription)) {
            contentType.Description = _description;
        }

        if (_varyByCulture) {
            contentType.Variations |= ContentVariation.Culture;
        }

        if (_varyBySegment) {
            contentType.Variations |= ContentVariation.Segment;
        }

        ApplyCompositions(contentType);
        ApplyContainers(contentType);
        ApplyKind(contentType);

        _contentTypeService.Save(contentType);

        return contentType;
    }

    public void SetDescription(string description, bool overwriteExisting = false) {
        _description = description;
        _overwriteDescription = overwriteExisting;
    }

    public void SetIcon(string icon, bool overwriteExisting = false) {
        _icon = icon;
        _overwriteIcon = overwriteExisting;
    }

    public void SetName(string name, bool overwriteExisting = false) {
        _name = name;
        _overwriteName = overwriteExisting;
    }

    public IPropertyContainerBuilder Tab(string name) {
        var container = new PropertyContainerBuilder(ServiceProvider, _shortStringHelper, name, true, null);

        RegisterContainer(container);

        return container;
    }

    public void VaryByCulture() {
        _varyByCulture = true;
    }

    public void VaryBySegment() {
        _varyBySegment = true;
    }

    public void WithDeterministicId() {
        _deterministic = true;
        _id = UmbracoId.Deterministic(IdScope.ContentType, Alias);
    }

    public void WithId(Guid id) {
        _deterministic = true;
        _id = id;
    }

    public string Alias { get; }

    protected virtual void ApplyKind(IContentType contentType) { }

    protected void RegisterContainer(PropertyContainerBuilder container) {
        _containers.Add(container);
    }

    protected IContentType ResolveContentType(string alias) {
        var contentType = _contentTypeService.Get(alias);

        if (contentType == null) {
            throw new ContentTypeNotFoundException(alias);
        }

        return contentType;
    }

    protected IServiceProvider ServiceProvider { get; }
    protected IShortStringHelper ShortStringHelper => _shortStringHelper;

    private void ApplyCompositions(IContentType contentType) {
        foreach (var alias in _compositions) {
            if (!contentType.ContentTypeCompositionExists(alias)) {
                contentType.AddContentType(ResolveContentType(alias));
            }
        }
    }

    private void ApplyContainer(IContentType contentType, PropertyContainerBuilder container, int sortOrder) {
        var containerType = container.IsTab ? PropertyGroupType.Tab : PropertyGroupType.Group;

        // ToSafeAlias preserves the casing of the name, but Umbraco validates groups by name and a site's
        // existing groups are usually camel cased, so a case sensitive match here adds a second group with
        // the same name and the save is then rejected for the duplicate. The kind has to match as well: a
        // site that nests its groups holds both a "general" tab and a "general/general" group, and binding
        // a group to that empty tab places the property above the group rather than inside it
        var group = contentType.PropertyGroups
                               .FirstOrDefault(x => x.Alias.EqualsInvariant(container.Alias) &&
                                                    x.Type == containerType);

        // Tab nesting is encoded in the alias, so a site that put this group under a tab holds it as
        // "tab/group". Adopting that keeps a new property beside its siblings instead of creating a second
        // group with the same leaf name alongside the one the site already uses
        if (group == null) {
            var nested = contentType.PropertyGroups
                                    .Where(x => x.Type == containerType &&
                                                x.Alias.EndsWith($"/{container.Alias}",
                                                                 StringComparison.InvariantCultureIgnoreCase))
                                    .ToList();

            if (nested.Count == 1) {
                group = nested.Single();
            }
        }

        // A site already holding every property this container declares has arranged them its own way, so
        // creating the container would add an empty group beside the ones the site actually uses
        var isNeeded = container.Properties.Any(x => !contentType.PropertyTypeExists(x.Alias));

        if (group == null && isNeeded) {
            // A tab already holding this alias means the site nests its groups, so the group this designer
            // creates goes under that tab instead of colliding with the tab's own alias
            var nestUnderTab = !container.IsTab &&
                               contentType.PropertyGroups
                                          .Any(x => x.Alias.EqualsInvariant(container.Alias) &&
                                                    x.Type == PropertyGroupType.Tab);

            var alias = nestUnderTab ? $"{container.Alias}/{container.Alias}" : container.Alias;

            contentType.AddPropertyGroup(alias, container.Name);

            group = contentType.PropertyGroups.First(x => x.Alias.EqualsInvariant(alias));

            if (_deterministic) {
                group.Key = UmbracoId.Deterministic(IdScope.ContentTypeContainer, Alias, alias);
            }

            // Only a group this designer created gets its type and position set. Rewriting an existing one
            // turns a site's tab into a group, and Umbraco then rejects every composition that shares the
            // alias because the same alias must be the same type across all of them. For the same reason a
            // new group takes the type a composition already gives that alias, in preference to our own
            var composed = contentType.ContentTypeComposition
                                      .SelectMany(x => x.PropertyGroups.OrEmpty())
                                      .FirstOrDefault(x => x.Alias.EqualsInvariant(alias));

            group.Type = composed?.Type ?? containerType;
            group.SortOrder = sortOrder;
        }

        var propertySortOrder = 0;

        // The adopted group keeps its own alias, so properties must be placed using that and not the
        // alias this designer derived, or they land in a group that does not exist
        foreach (var (propertyAlias, builder) in container.Properties) {
            ApplyProperty(contentType, container, group, propertyAlias, builder, propertySortOrder++);
        }
    }

    private void ApplyContainers(IContentType contentType) {
        var sortOrder = 0;

        foreach (var container in _containers) {
            ApplyContainer(contentType, container, sortOrder++);

            foreach (var child in container.Children) {
                ApplyContainer(contentType, child, sortOrder++);
            }
        }
    }

    private void ApplyProperty(IContentType contentType,
                               PropertyContainerBuilder container,
                               PropertyGroup group,
                               string propertyAlias,
                               IPropertyTypeBuilder builder,
                               int sortOrder) {
        var context = new PropertyTypeContext(Alias, propertyAlias);
        var existing = contentType.PropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias);

        if (existing != null) {
            var dataType = builder.ResolveDataType(context);
            var currentContainer = contentType.PropertyGroups
                                              .FirstOrDefault(x => x.PropertyTypes.OrEmpty()
                                                                    .Any(y => y.Alias == propertyAlias));

            existing.DataTypeId = dataType.Id;
            existing.DataTypeKey = dataType.Key;

            builder.Apply(existing, context, false);

            // Where a property sits is how a site's editors have arranged the type, so the order given here
            // is only applied to a property this designer creates. One the site already placed in a group of
            // its own likewise stays there, and only one that is somewhere else entirely gets moved, so a
            // site's own layout survives a re-seed
            if (currentContainer == null && group != null) {
                contentType.MovePropertyType(propertyAlias, group.Alias);
            }
        } else if (group != null && !contentType.PropertyTypeExists(propertyAlias)) {
            var dataType = builder.ResolveDataType(context);
            var propertyType = new PropertyType(_shortStringHelper, dataType);

            propertyType.Alias = propertyAlias;
            propertyType.DataTypeKey = dataType.Key;
            propertyType.SortOrder = sortOrder;

            if (_deterministic) {
                propertyType.Key = UmbracoId.Deterministic(IdScope.PropertyType, Alias, propertyAlias);
            }

            builder.Apply(propertyType, context, true);

            contentType.AddPropertyType(propertyType, group.Alias, group.Name);
        }
    }

    private IContentType Create() {
        var contentType = new ContentType(_shortStringHelper, GetOrCreateFolder());

        contentType.Alias = Alias;

        if (_id.HasValue()) {
            contentType.Key = _id.GetValueOrThrow();
        }

        return contentType;
    }

    private IContentType FindExisting() {
        var contentType = default(IContentType);

        if (_id.HasValue()) {
            contentType = _contentTypeService.Get(_id.GetValueOrThrow());
        }

        contentType ??= _contentTypeService.Get(Alias);

        return contentType;
    }

    private int GetOrCreateFolder() {
        var container = default(EntityContainer);
        var walkedPath = new List<string>();

        foreach (var element in _folderPath) {
            walkedPath.Add(element);

            var elementContainer = default(EntityContainer);

            if (container == null) {
                elementContainer = _contentTypeService.GetContainers(element, 1).SingleOrDefault();
            } else {
                elementContainer = _contentTypeService.GetContainers(element, container.Level + 1)
                                                      .SingleOrDefault(x => x.ParentId == container.Id);
            }

            if (elementContainer == null) {
                var key = UmbracoId.Deterministic(IdScope.ContentTypeFolder, walkedPath.ToArray());
                var attempt = _contentTypeService.CreateContainer(container?.Id ?? -1, key, element);

                if (!attempt.Success) {
                    throw new Exception($"Failed to create content type folder {element.Quote()}", attempt.Exception);
                }

                container = attempt.Result.Entity;
            } else {
                container = elementContainer;
            }
        }

        return container?.Id ?? -1;
    }
}
