using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockViewModel {
    Guid Id { get; }
    BlockModulesData ModulesData { get; }
    IPublishedElement Content { get; }
    
    string GetText(string s);
}

public interface IBlockViewModel<TBlock> : IBlockViewModel where TBlock : IPublishedElement { }

public class BlockViewModel<TBlock> : IBlockViewModel<TBlock> where TBlock : IPublishedElement {
    private readonly Func<string, string> _getText;

    public BlockViewModel(BlockParameters<TBlock> parameters) {
        Id = parameters.Id;
        Content = parameters.Content;
        ModulesData = parameters.ModulesData;

        _getText = parameters.GetText;
    }

    public Guid Id { get; }
    public TBlock Content { get; }
    public BlockModulesData ModulesData { get; }

    public string GetText(string s) => _getText(s);
    
    IPublishedElement IBlockViewModel.Content => Content;
}
