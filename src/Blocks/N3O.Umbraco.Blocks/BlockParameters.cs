using System;

namespace N3O.Umbraco.Blocks;

public class BlockParameters<TBlock, TSettings> {
    public BlockParameters(Func<string, string> getText,
                           TBlock content,
                           TSettings settings,
                           BlockModulesData modulesData,
                           Guid id) {
        GetText = getText;
        Content = content;
        Settings = settings;
        ModulesData = modulesData;
        Id = id;
    }

    public Func<string, string> GetText { get; }
    public TBlock Content { get; }
    public TSettings Settings { get; }
    public BlockModulesData ModulesData { get; }
    public Guid Id { get; }
}
