using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class ContentPublisher : IContentPublisher {
    private readonly IContentService _contentService;
    private readonly IContent _content;

    public ContentPublisher(IServiceProvider serviceProvider, IContentService contentService, IContent content) {
        Content = new ContentBuilder(serviceProvider, content.ContentType.Alias);
        
        _contentService = contentService;
        _content = content;
    }

    public bool HasProperty(string propertyTypeAlias) {
        var property = _content.Properties.SingleOrDefault(x => x.Alias == propertyTypeAlias);

        return property != null;
    }

    public void Move(Guid parentId) {
        var newParent = _contentService.GetById(parentId);

        if (newParent == null) {
            throw new Exception($"Move failed as no content found with ID {parentId}");
        }

        _content.ParentId = newParent.Id;
    }

    public PublishResult SaveAndPublish() {
        return Save(() => _contentService.SaveAndPublish(_content));
    }

    public OperationResult SaveUnpublished() {
        return Save(() => _contentService.Save(_content));
    }

    public void SetName(string name) {
        _content.Name = name;
    }

    public void Unpublish() {
        _contentService.Unpublish(_content);
    }

    public IContentBuilder Content { get; }

    private T Save<T>(Func<T> save) {
        var newPropertyValues = Content.Build();

        foreach (var (propertyTypeAlias, value) in newPropertyValues) {
            if (HasProperty(propertyTypeAlias)) {
                _content.SetValue(propertyTypeAlias, value);
            }
        }

        return save();
    }
}
