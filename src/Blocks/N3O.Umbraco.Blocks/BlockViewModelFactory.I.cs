using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockViewModelFactory<TBlock, TSettings> : IBlockViewModelFactory { }

public interface IBlockViewModelFactory {
    IBlockViewModel Create(IPublishedElement content, object settings);
}