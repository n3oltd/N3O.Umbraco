using NodaTime;
using System;

namespace N3O.Umbraco.Data.Konstrukt;

public partial class Import {
    public void SavedAndPublished(IClock clock, Guid id, string contentSummary) {
        ImportedAt = clock.GetCurrentInstant().ToDateTimeUtc();
        ImportedContentId = id;
        ImportedContentSummary = contentSummary;
        Notices = null;
        Status = ImportStatuses.SavedAndPublished;
    }
}
