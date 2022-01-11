using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Blocks {
    public class BlockParameters<TBlock> {
        public BlockParameters(Func<string, string> getText,
                               TBlock content,
                               Guid id,
                               Guid definitionId,
                               Guid layoutId) {
            GetText = getText;
            Content = content;
            Id = id;
            DefinitionId = definitionId;
            LayoutId = layoutId;
        }

        public Func<string, string> GetText { get; }
        public TBlock Content { get; }
        public Guid Id { get; }
        public Guid DefinitionId { get; }
        public Guid LayoutId { get; }
    }
}
