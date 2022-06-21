using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content {
    public class ContentEditor : IContentEditor {
        private readonly IServiceProvider _serviceProvider;
        private readonly IContentService _contentService;

        public ContentEditor(IServiceProvider serviceProvider, IContentService contentService) {
            _serviceProvider = serviceProvider;
            _contentService = contentService;
        }
        
        public IContentPublisher ForExisting(Guid id) {
            var content = _contentService.GetById(id);
            
            return GetContentPublisher(content);
        }
        
        public IContentPublisher New(string name, Guid parentId, string contentTypeAlias, Guid? contentId = null) {
            var content = _contentService.Create(name, parentId, contentTypeAlias);
            
            if (contentId != null) {
                content.Key = contentId.Value;
            } 
            
            return GetContentPublisher(content);
        }

        private IContentPublisher GetContentPublisher(IContent content) {
            return new ContentPublisher(_serviceProvider, _contentService, content);
        }
    }
}