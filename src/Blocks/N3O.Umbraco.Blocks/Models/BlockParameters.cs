using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Blocks;

public class BlockParameters<TBlock, TSettings> {
    private readonly HtmlIds _htmlIds = new();
    
    public BlockParameters(Func<string, string> getText,
                           TBlock content,
                           TSettings settings,
                           BlockModulesData modulesData,
                           Guid id) {
        GenerateId = x => _htmlIds.GenerateId(x);
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
