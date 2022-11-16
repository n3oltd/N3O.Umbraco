using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Data.Entities;

public partial class Export : Entity {
    public string ContentType { get; private set; }
    public Guid ContainerId { get; private set; }
    public string Filename { get; private set; }
    public long Number { get; private set; }
    public bool IsComplete { get; private set; }
    public long CollatedRecords { get; private set; }
    public string Stage { get; private set; }
    public string StorageFolderName { get; private set; }
    public WorkbookFormat Format { get; private set; }
}
