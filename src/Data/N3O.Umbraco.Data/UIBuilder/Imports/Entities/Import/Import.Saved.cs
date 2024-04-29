using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.UIBuilder;

public partial class Import {
    public void Saved(IJsonProvider jsonProvider,
                      IClock clock,
                      Guid id,
                      string contentSummary,
                      IEnumerable<string> warnings) {
        ImportedAt = clock.GetCurrentInstant().ToDateTimeUtc();
        ImportedContentId = id;
        ImportedContentSummary = contentSummary;
        Notices = jsonProvider.SerializeObject(new ImportNotices {
            Warnings = warnings
        });
        Status = ImportStatuses.Saved;
    }
}
