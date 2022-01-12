using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Content {
    public class ContentNode {
        public ContentNode(Guid id,
                           string contentTypeAlias,
                           IEnumerable<ContentProperty> properties,
                           IEnumerable<ContentNode> children) {
            Id = id;
            ContentTypeAlias = contentTypeAlias;
            Properties = properties.OrEmpty().ToList();
            Children = children.OrEmpty().ToList();
        }

        public Guid Id { get; }
        public string ContentTypeAlias { get; }
        public IReadOnlyList<ContentProperty> Properties { get; }
        public IReadOnlyList<ContentNode> Children { get; }
    }
}