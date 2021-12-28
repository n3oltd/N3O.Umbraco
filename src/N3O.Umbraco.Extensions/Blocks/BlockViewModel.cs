using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks {
    public interface IBlockViewModel : IContentBlockViewModel { }

    public interface IBlockViewModel<TBlock> : IBlockViewModel, IContentBlockViewModel<TBlock>
        where TBlock : IPublishedElement { }

    public class BlockViewModel<TBlock> : IBlockViewModel<TBlock> where TBlock : IPublishedElement {
        public BlockViewModel(BlockParameters<TBlock> parameters) {
            Id = parameters.Id;
            DefinitionId = parameters.DefinitionId;
            LayoutId = parameters.LayoutId;
            Content = parameters.Content;
        }
    
        public Guid Id { get; }
        public Guid DefinitionId { get; }
        public Guid LayoutId  { get; }
        public TBlock Content { get; }

        IPublishedElement IContentBlockViewModel.Content => Content;
    }
}