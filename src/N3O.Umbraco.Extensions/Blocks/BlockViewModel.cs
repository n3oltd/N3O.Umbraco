using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks {
    public interface IBlockViewModel : IContentBlockViewModel {
        BlockModulesData ModulesData { get; }
        
        string GetText(string s);
    }

    public interface IBlockViewModel<TBlock> : IBlockViewModel, IContentBlockViewModel<TBlock>
        where TBlock : IPublishedElement { }

    public class BlockViewModel<TBlock> : IBlockViewModel<TBlock> where TBlock : IPublishedElement {
        private readonly Func<string, string> _getText;

        public BlockViewModel(BlockParameters<TBlock> parameters) {
            Id = parameters.Id;
            DefinitionId = parameters.DefinitionId;
            LayoutId = parameters.LayoutId;
            Content = parameters.Content;
            ModulesData = parameters.ModulesData;

            _getText = parameters.GetText;
        }
    
        public Guid Id { get; }
        public Guid DefinitionId { get; }
        public Guid LayoutId  { get; }
        public TBlock Content { get; }
        public BlockModulesData ModulesData { get; }

        public string GetText(string s) => _getText(s);
        
        IPublishedElement IContentBlockViewModel.Content => Content;
    }
}