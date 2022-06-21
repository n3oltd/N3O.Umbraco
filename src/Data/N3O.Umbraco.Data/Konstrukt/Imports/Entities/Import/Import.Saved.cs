using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Saved(Guid id, string contentSummary, IEnumerable<string> warnings, IJsonProvider jsonProvider) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
            Notices = jsonProvider.SerializeObject(new ImportNotices {
                Warnings = warnings
            });
            Status = ImportStatuses.Saved;
        }
    }
}