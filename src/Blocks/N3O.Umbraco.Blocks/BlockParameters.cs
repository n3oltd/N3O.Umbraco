using System;

namespace N3O.Umbraco.Blocks;

public class BlockParameters<TBlock> {
    public BlockParameters(Func<string, string> getText,
                           TBlock content,
                           BlockModulesData modulesData,
                           Guid id) {
        GetText = getText;
        Content = content;
        ModulesData = modulesData;
        Id = id;
    }

    public Func<string, string> GetText { get; }
    public TBlock Content { get; }
    public BlockModulesData ModulesData { get; }
    public Guid Id { get; }
}
