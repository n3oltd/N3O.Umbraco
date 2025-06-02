using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockViewModel {
    Guid Id { get; }
    BlockModulesData ModulesData { get; }
    IPublishedElement Content { get; }
    object Settings { get; }
    
    string GetText(string s);
}

public interface IBlockViewModel<TBlock, TSettings> : IBlockViewModel where TBlock : IPublishedElement { }

public class BlockViewModel<TBlock, TSettings> : IBlockViewModel<TBlock, TSettings> where TBlock : IPublishedElement {
    private readonly Func<string, string> _getText;

    public BlockViewModel(BlockParameters<TBlock, TSettings> parameters) {
        Id = parameters.Id;
        Content = parameters.Content;
        Settings = parameters.Settings;
        ModulesData = parameters.ModulesData;

        _getText = parameters.GetText;
    }

    public Guid Id { get; }
    public TBlock Content { get; }
    public TSettings Settings { get; }
    public BlockModulesData ModulesData { get; }

    public string GetText(string s) => _getText(s);
    
    IPublishedElement IBlockViewModel.Content => Content;
    object IBlockViewModel.Settings => Settings;
}
