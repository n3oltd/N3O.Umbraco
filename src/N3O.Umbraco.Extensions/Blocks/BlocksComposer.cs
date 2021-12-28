using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using Perplex.ContentBlocks.Categories;
using Perplex.ContentBlocks.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks {
    public class BlocksComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IBlockTypesService, BlockTypesService>();

            foreach (var blockDefinition in BlocksComponent.BlockDefinitions) {
                RegisterDefaultViewModel(builder, blockDefinition.Alias);
            }
        
            builder.Components().Append<BlocksComponent>();
        }

        private void RegisterDefaultViewModel(IUmbracoBuilder builder, string contentTypeAlias) {
            var blockType = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                        t.IsSubclassOfType(typeof(PublishedElementModel)))
                                         .SingleOrDefault(t => AliasHelper.ForContentType(t).EqualsInvariant(contentTypeAlias));

            if (blockType != null) {
                builder.AddDefaultBlockViewModel(blockType);
            }
        }
    }

    public class BlocksComponent : IComponent {
        public static IReadOnlyList<BlockDefinition> BlockDefinitions { get; }

        private readonly IBlockTypesService _blockTypesService;
        private readonly ILookups _lookups;
        private readonly IContentBlockDefinitionRepository _blockDefinitionsRepository;
        private readonly IContentBlockCategoryRepository _blockCategoriesRepository;

        static BlocksComponent() {
            BlockDefinitions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                           t.ImplementsInterface<IBlockBuilder>() &&
                                                           t.HasParameterlessConstructor())
                                            .Select(t => (IBlockBuilder) Activator.CreateInstance(t))
                                            .Select(x => x.Build())
                                            .ToList();
        }

        public BlocksComponent(IBlockTypesService blockTypesService,
                               ILookups lookups,
                               IContentBlockDefinitionRepository blockDefinitionsRepository,
                               IContentBlockCategoryRepository blockCategoriesRepository) {
            _blockTypesService = blockTypesService;
            _lookups = lookups;
            _blockDefinitionsRepository = blockDefinitionsRepository;
            _blockCategoriesRepository = blockCategoriesRepository;
        }
    
        public void Initialize() {
            var blockCategories = _lookups.GetAll<BlockCategory>();

            _blockCategoriesRepository.Remove(Perplex.ContentBlocks.Constants.Categories.Content);
            _blockCategoriesRepository.Remove(Perplex.ContentBlocks.Constants.Categories.Headers);
            blockCategories.Do(x => _blockCategoriesRepository.Add(x));
        
            BlockDefinitions.Do(x => {
                _blockTypesService.CreateTypes(x);
                _blockDefinitionsRepository.Add(x);
            });
        }
    
        public void Terminate() { }
    }
}
