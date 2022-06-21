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
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Blocks;

public class BlocksComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IBlockTypesService, BlockTypesService>();

        foreach (var blockDefinition in BlocksComponent.BlockDefinitions) {
            RegisterDefaultViewModel(builder, blockDefinition.Alias);
        }
        
        RegisterAll(t => t.ImplementsInterface<IBlockModule>(),
                    t => builder.Services.AddTransient(typeof(IBlockModule), t));

        builder.Services.AddTransient<IBlockPipeline, BlockPipeline>();
    
        builder.Components().Append<BlocksComponent>();
    }

    private void RegisterDefaultViewModel(IUmbracoBuilder builder, string contentTypeAlias) {
        var blockType = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                    t.IsSubclassOfType(typeof(PublishedElementModel)))
                                     .SingleOrDefault(t => AliasHelper.ContentTypeAlias(t).EqualsInvariant(contentTypeAlias));

        if (blockType != null) {
            builder.AddDefaultBlockViewModel(blockType);
        }
    }
}

public class BlocksComponent : IComponent {
    public static IReadOnlyList<BlockDefinition> BlockDefinitions { get; }

    private readonly IRuntimeState _runtimeState;
    private readonly Lazy<IBlockTypesService> _blockTypesService;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IContentBlockDefinitionRepository> _blockDefinitionsRepository;
    private readonly Lazy<IContentBlockCategoryRepository> _blockCategoriesRepository;

    static BlocksComponent() {
        BlockDefinitions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                       t.ImplementsInterface<IBlockBuilder>() &&
                                                       t.HasParameterlessConstructor())
                                        .Select(t => (IBlockBuilder) Activator.CreateInstance(t))
                                        .Select(x => x.Build())
                                        .ToList();
    }

    public BlocksComponent(IRuntimeState runtimeState,
                           Lazy<IBlockTypesService> blockTypesService,
                           Lazy<ILookups> lookups,
                           Lazy<IContentBlockDefinitionRepository> blockDefinitionsRepository,
                           Lazy<IContentBlockCategoryRepository> blockCategoriesRepository) {
        _runtimeState = runtimeState;
        _blockTypesService = blockTypesService;
        _lookups = lookups;
        _blockDefinitionsRepository = blockDefinitionsRepository;
        _blockCategoriesRepository = blockCategoriesRepository;
    }

    public void Initialize() {
        if (_runtimeState.Level == RuntimeLevel.Run) {
            var blockCategories = _lookups.Value.GetAll<BlockCategory>().OrderBy(x => x.Order).ToList();

            _blockCategoriesRepository.Value.Remove(Perplex.ContentBlocks.Constants.Categories.Content);
            _blockCategoriesRepository.Value.Remove(Perplex.ContentBlocks.Constants.Categories.Headers);
            blockCategories.Do(x => _blockCategoriesRepository.Value.Add(x));

            BlockDefinitions.Do(x => {
                _blockTypesService.Value.CreateTypes(x);
                _blockDefinitionsRepository.Value.Add(x);
            });
        }
    }

    public void Terminate() { }
}
