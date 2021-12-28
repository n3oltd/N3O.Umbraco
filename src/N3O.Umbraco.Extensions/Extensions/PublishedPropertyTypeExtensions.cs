using J2N.Collections.Generic;
using N3O.Umbraco.Content;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Extensions {
    public static class PublishedPropertyTypeExtensions {
        public static Type GetNestedContentType(this IPublishedPropertyType propertyType,
                                                string modelsNamespace) {
            var config = propertyType.DataType.ConfigurationAs<NestedContentConfiguration>();
            var contentTypes = config.ContentTypes.ToList();
            string nestedContentAlias;

            if (contentTypes.IsSingle()) {
                nestedContentAlias = contentTypes.First().Alias;
            } else {
                var commonInterfaces = new List<Type>();

                foreach (var contentType in contentTypes) {
                    var type = ModelsHelper.GetOrCreateModelsBuilderType(modelsNamespace, contentType.Alias);

                    var typeInterfaces = type.GetInterfaces().Where(x => x.FullName.StartsWith(modelsNamespace)).ToList();

                    if (contentType == contentTypes.First()) {
                        commonInterfaces.AddRange(typeInterfaces);

                        continue;
                    }

                    commonInterfaces.RemoveAll(x => !typeInterfaces.Contains(x));
                }

                if (commonInterfaces.Count != 1) {
                    return typeof(IPublishedElement);
                }

                nestedContentAlias = commonInterfaces.Single().FullName.Substring(modelsNamespace.Length + 1);
            }

            return ModelsHelper.GetOrCreateModelsBuilderType(modelsNamespace, nestedContentAlias);
        }
    
        public static bool HasEditorAlias(this IPublishedPropertyType propertyType, string alias) {
            return propertyType.EditorAlias.EqualsInvariant(alias);
        }
    }
}
