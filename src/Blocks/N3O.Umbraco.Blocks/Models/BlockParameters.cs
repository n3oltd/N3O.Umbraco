using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Blocks;

public class BlockParameters<TBlock, TSettings> {
    public BlockParameters(Func<string, string> getText,
                           TBlock content,
                           TSettings settings,
                           BlockModulesData modulesData,
                           Guid id) {
        GenerateId = x => HtmlIds.GenerateId(x);
        GetText = getText;
        Content = content;
        Settings = settings;
        ModulesData = modulesData;
        Id = id;
    }

    public Func<object[], string> GenerateId { get; }
    public Func<string, string> GetText { get; }
    public TBlock Content { get; }
    public TSettings Settings { get; }
    public BlockModulesData ModulesData { get; }
    public Guid Id { get; }
}
