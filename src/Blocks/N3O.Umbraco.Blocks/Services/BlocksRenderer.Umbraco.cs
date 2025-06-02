using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public class UmbracoBlocksRenderer : BlocksRenderer<IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>>> {
    private readonly ModelsBuilderSettings _modelBuilderSettings;
    private readonly IServiceProvider _serviceProvider;

    protected UmbracoBlocksRenderer(ModelsBuilderSettings modelBuilderSettings, IServiceProvider serviceProvider) {
        _modelBuilderSettings = modelBuilderSettings;
        _serviceProvider = serviceProvider;
    }

    protected override IEnumerable<IBlockViewModel> GetViewModels(IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blockItems) {
        foreach (var blockItem in blockItems) {
            var blockModelsBuilderType = ModelsHelper.GetOrCreateModelsBuilderType(_modelBuilderSettings.ModelsNamespace,
                                                                                   blockItem.Content.ContentType.Alias);

            var settingsModelsBuilderType = blockItem.Settings.HasValue()
                                                ? ModelsHelper.GetOrCreateModelsBuilderType(_modelBuilderSettings.ModelsNamespace,
                                                                                            blockItem.Settings.ContentType.Alias)
                                                : typeof(None);

            var factoryType = typeof(IBlockViewModelFactory<,>).MakeGenericType(blockModelsBuilderType,
                                                                                settingsModelsBuilderType);

            var factory = (IBlockViewModelFactory) _serviceProvider.GetService(factoryType);

            if (factory == null) {
                factory = BlockViewModelFactory.Default(() => _serviceProvider,
                                                        blockModelsBuilderType,
                                                        settingsModelsBuilderType);
            }

            var viewModel = blockItem.Settings.HasValue()
                                ? factory.Create(blockItem.Content, blockItem.Settings)
                                : factory.Create(blockItem.Content, None.Empty);

            yield return viewModel;
        }
    }

    protected override IEnumerable<string> GetViewPaths(IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blockItems) {
        foreach (var blockItem in blockItems) {
            yield return $"/Views/Partials/Blocks/{blockItem.Content.ContentType.Alias}.cshtml";
        }
    }
}