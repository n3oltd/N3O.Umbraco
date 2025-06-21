using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Data.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Data;

public class ImportProcessingQueue : IImportProcessingQueue {
    private readonly IBackgroundJob _backgroundJob;

    public ImportProcessingQueue(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public void Add(Import import) {
        _backgroundJob.EnqueueCommand<ProcessImportCommand>(p => p.Add<ImportId>(import.Id.ToString()));
    }

    public void AddAll(IEnumerable<Import> imports) {
        imports.Do(Add);
    }
}
