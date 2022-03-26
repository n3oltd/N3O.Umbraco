using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content {
    public class ContentPublisher : IContentPublisher {
        private readonly IContentService _contentService;
        private readonly IContent _content;

        public ContentPublisher(IServiceProvider serviceProvider,  IContentService contentService, IContent content) {
            Content = new ContentBuilder(serviceProvider);
            
            _contentService = contentService;
            _content = content;
        }

        public bool HasProperty(string propertyTypeAlias) {
            var property = _content.Properties.SingleOrDefault(x => x.Alias == propertyTypeAlias);

            return property != null;
        }

        public PublishResult SaveAndPublish() {
            var newPropertyValues = Content.Build();

            foreach (var (propertyTypeAlias, value) in newPropertyValues) {
                if (HasProperty(propertyTypeAlias)) {
                    _content.SetValue(propertyTypeAlias, value);
                }
            }

            return _contentService.SaveAndPublish(_content);
        }

        public IContentBuilder Content { get; }
    }
}