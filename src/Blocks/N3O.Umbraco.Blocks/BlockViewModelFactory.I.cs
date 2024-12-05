using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockViewModelFactory<TBlock> : IBlockViewModelFactory { }

public interface IBlockViewModelFactory {
    IBlockViewModel Create(IPublishedElement content, Guid id);
}