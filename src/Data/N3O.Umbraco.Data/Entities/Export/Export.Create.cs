using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.References;
using N3O.Umbraco.Storage;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Entities;

public partial class Export {
    public static async Task<Export> CreateAsync(ICounters counters,
                                                 Guid id,
                                                 IContentType contentType,
                                                 Guid containerId,
                                                 WorkbookFormat format) {
        var export = Create<Export>(id);

        export.ContentType = contentType.Alias;
        export.ContainerId = containerId;
        export.Number = await counters.NextAsync(nameof(Export), 10001);
        export.Format = format;
        export.StorageFolderName = StorageConstants.StorageFolders.Temp;
        export.Filename = format.AppendFileExtension($"{contentType.Name} Export {export.Number}");
        export.IsComplete = false;
        export.Processed = 0;

        return export;
    }
}
