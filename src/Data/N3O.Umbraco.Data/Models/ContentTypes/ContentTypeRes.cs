using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models {
    public class ContentTypeRes {
        public string Alias { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmbracoPropertyInfoRes> Properties { get; set; }
    }
}