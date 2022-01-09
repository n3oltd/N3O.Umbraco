using N3O.Umbraco.Extensions;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Utilities {
    public enum IdScope {
        Block,
        BlockCategory,
        BlockDataType,
        BlockLayout,
        ContentType,
        ContentTypeContainer,
        DataTypeContainer
    }
    
    public static class UmbracoId {
        public static Guid Generate(IdScope scope, params object[] seeds) {
            var id = $"{scope}_{string.Join("_", seeds)}".GetDeterministicHashCode(true).ToGuid();

            return id;
        }
    }
}