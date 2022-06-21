using N3O.Umbraco.Data.Konstrukt;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Services;

public interface IImportProcessingQueue {
    void Add(Import import);
    void AddAll(IEnumerable<Import> imports);
}
