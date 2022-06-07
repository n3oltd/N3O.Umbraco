using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Models {
    public class AddFileToImportReq {
        [Name("Zip File")]
        public StorageToken ZipFile { get; set; }
    }
}