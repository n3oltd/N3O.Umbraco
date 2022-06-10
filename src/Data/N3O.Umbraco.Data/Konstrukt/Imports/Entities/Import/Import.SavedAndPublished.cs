using System;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void SavedAndPublished(Guid id, string contentSummary) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
             Notices = null;
            Status = ImportStatuses.SavedAndPublished;
        }
    }
}