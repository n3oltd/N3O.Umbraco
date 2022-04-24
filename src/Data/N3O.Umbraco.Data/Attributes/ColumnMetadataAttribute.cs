using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Attributes {
    public abstract class ColumnMetadataAttribute : Attribute {
        public abstract IEnumerable<(string Key, object Value)> GetMetadata();
    }
}
