using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Extensions {
    public static class ContentNodeExtensions {
        public static IEnumerable<ContentNode> Flatten(this ContentNode node) {
            var list = new List<ContentNode>();

            list.Add(node);
            
            foreach (var childNode in node.Children.OrEmpty()) {
                list.Add(childNode);

                list.AddRange(Flatten(childNode));
            }

            return list;
        }
    }
}