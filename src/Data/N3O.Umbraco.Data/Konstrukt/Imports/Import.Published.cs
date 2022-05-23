using System;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Published(Guid id, string contentSummary) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
            Errors = null;
            Status = ImportStatuses.SavedAndPublished;
        }
    }
}