using System;

namespace N3O.Umbraco.Blocks {
    public class BlockParameters<TBlock> {
        public BlockParameters(Func<string, string> getText,
                               TBlock content,
                               BlockModulesData modulesData,
                               Guid id,
                               Guid definitionId,
                               Guid layoutId) {
            GetText = getText;
            Content = content;
            ModulesData = modulesData;
            Id = id;
            DefinitionId = definitionId;
            LayoutId = layoutId;
        }

        public Func<string, string> GetText { get; }
        public TBlock Content { get; }
        public BlockModulesData ModulesData { get; }
        public Guid Id { get; }
        public Guid DefinitionId { get; }
        public Guid LayoutId { get; }
    }
}
