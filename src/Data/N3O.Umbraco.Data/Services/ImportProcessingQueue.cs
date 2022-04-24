using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Services {
    public class ImportProcessingQueue : IImportProcessingQueue {
        private readonly IBackgroundJob _backgroundJob;

        public ImportProcessingQueue(IBackgroundJob backgroundJob) {
            _backgroundJob = backgroundJob;
        }

        public void Add(Import import) {
            _backgroundJob.Enqueue<ProcessImportCommand>($"Process Import {import.Id}",
                                                         p => p.Add<ImportId>(import.Id.ToString()));
        }

        public void AddAll(IEnumerable<Import> imports) {
            imports.Do(Add);
        }
    }
}