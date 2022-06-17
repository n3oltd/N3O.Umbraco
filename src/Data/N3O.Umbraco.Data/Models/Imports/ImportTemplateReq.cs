using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Models {
    public class ImportTemplateReq {
        [Name("Import Guid")]
        public bool? ImportGuid { get; set; }
    }
}