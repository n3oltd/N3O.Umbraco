using System;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlockParameters<TBlock> : BlockParameters<TBlock> {
    public PerplexBlockParameters(Func<string, string> getText,
                                  TBlock content,
                                  BlockModulesData modulesData,
                                  Guid id,
                                  Guid definitionId,
                                  Guid layoutId)
        : base(getText, content, modulesData, id) {
        DefinitionId = definitionId;
        LayoutId = layoutId;
    }

    public Guid DefinitionId { get; }
    public Guid LayoutId { get; }
}
