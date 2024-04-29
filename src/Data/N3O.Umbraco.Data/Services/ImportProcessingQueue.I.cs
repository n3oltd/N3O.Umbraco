using N3O.Umbraco.Data.UIBuilder;
using System.Collections.Generic;

namespace N3O.Umbraco.Data;

public interface IImportProcessingQueue {
    void Add(Import import);
    void AddAll(IEnumerable<Import> imports);
}
