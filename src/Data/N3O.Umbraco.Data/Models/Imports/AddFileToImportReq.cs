using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Models;

public class AddFileToImportReq {
    [Name("File")]
    public StorageToken File { get; set; }
}
