using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Perplex;

public interface IPerplexBlockViewModel : IBlockViewModel, IContentBlockViewModel { }

public interface IPerplexBlockViewModel<TBlock> :
    IPerplexBlockViewModel, IBlockViewModel<TBlock, None>, IContentBlockViewModel<TBlock>
    where TBlock : IPublishedElement { }

public class PerplexBlockViewModel<TBlock> : BlockViewModel<TBlock, None>, IPerplexBlockViewModel<TBlock>
    where TBlock : IPublishedElement {

    public PerplexBlockViewModel(PerplexBlockParameters<TBlock> parameters) : base(parameters){
        DefinitionId = parameters.DefinitionId;
        LayoutId = parameters.LayoutId;
    }
    
    public Guid DefinitionId { get; }
    public Guid LayoutId  { get; }
    
    IPublishedElement IContentBlockViewModel.Content => Content;
}
