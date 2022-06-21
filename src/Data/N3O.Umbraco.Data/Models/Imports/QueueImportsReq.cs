using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models;

public class QueueImportsReq {
    [Name("Date Pattern")]
    public DatePattern DatePattern { get; set; }
    
    [Name("CSV File")]
    public StorageToken CsvFile { get; set; }
    
    [Name("Zip File")]
    public StorageToken ZipFile { get; set; }
}
