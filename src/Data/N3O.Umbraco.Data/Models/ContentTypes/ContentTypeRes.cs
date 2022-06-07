using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models {
    public class ContentTypeRes {
        public string Name { get; set; }

        public IEnumerable<UmbracoPropertyRes> Properties { get; set; }
    }
}