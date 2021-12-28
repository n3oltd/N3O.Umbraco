using System;

namespace N3O.Umbraco.Blocks;

public class BlockParameters<TBlock> {
    public BlockParameters(TBlock content, Guid id, Guid definitionId, Guid layoutId) {
        Content = content;
        Id = id;
        DefinitionId = definitionId;
        LayoutId = layoutId;
    }

    public TBlock Content { get; }
    public Guid Id { get; }
    public Guid DefinitionId { get; }
    public Guid LayoutId { get; }
}
