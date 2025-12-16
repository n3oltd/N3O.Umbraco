using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class GeneratePublishedCompositionsHandler : IRequestHandler<GeneratePublishedCompositionsCommand, None, None> {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GeneratePublishedCompositionsHandler(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<None> Handle(GeneratePublishedCompositionsCommand req, CancellationToken cancellationToken) {
        var relativeUrlPath = "platforms/compositions";
        var compositionsDirectory = WebRoot.GetDirectory(_webHostEnvironment,
                                                         Path.Combine("platforms", "compositions"));
        
        foreach (var directory in compositionsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly)) {
            var publishedComposition = await GeneratePublishedCompositionAsync(directory, relativeUrlPath);
            
            var json = JsonConvert.SerializeObject(publishedComposition);

            await WebRoot.SaveTextAsync(_webHostEnvironment,
                                        Path.Combine(compositionsDirectory.FullName, $"{directory.Name}.json"),
                                        json);
        }

        return None.Empty;
    }

    private async Task<PublishedPlatformsComposition> GeneratePublishedCompositionAsync(DirectoryInfo directory,
                                                                                        string relativeUrlPath) {
        var compositionFile = new FileInfo(Path.Combine(directory.FullName, "composition.handlebars"));

        if (compositionFile.Exists) {
            var publishedComposition = new PublishedPlatformsComposition();
            publishedComposition.Reference = $"FSC_{directory.Name}";
            publishedComposition.Revision = 1;
            publishedComposition.Alias = directory.Name;
            publishedComposition.Markup = await File.ReadAllTextAsync(compositionFile.FullName);
            publishedComposition.Assets = GetAssets(directory, relativeUrlPath);
            publishedComposition.Layout = await GetLayoutAsync(directory);
            publishedComposition.Partials = await GetPartialsAsync(directory);

            return publishedComposition;
        } else {
            return null;
        }
    }
    private Dictionary<long, PublishedAsset> GetAssets(DirectoryInfo directory, string relativeUrlPath) {
        var publishedAssets = new Dictionary<long, PublishedAsset>();
        
        var assetsDirectory = new DirectoryInfo(Path.Combine(directory.FullName, "assets"));

        if (assetsDirectory.Exists) {
            foreach (var assetDirectory in assetsDirectory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)) {
                var assetNumber = assetDirectory.Name.TryParseAs<long>();

                if (assetNumber.HasValue() && assetDirectory.GetFiles().IsSingle()) {
                    var publishedAsset = new PublishedAsset();
                    publishedAsset.Reference = $"FSA_{assetNumber}";
                    publishedAsset.Number = assetNumber;
                    publishedAsset.Url = $"{relativeUrlPath}/{assetNumber}/{assetsDirectory.GetFiles().Single().Name}";

                    publishedAssets[assetNumber.GetValueOrThrow()] = publishedAsset;
                }
            }
        }
        
        return publishedAssets;
    }
    
    private async Task<PublishedLayout> GetLayoutAsync(DirectoryInfo directory) {
        var layoutFile = new FileInfo(Path.Combine(directory.FullName, "layout.handlebars"));

        if (layoutFile.Exists) {
            var publishedLayout = new PublishedLayout();
            publishedLayout.Markup = await File.ReadAllTextAsync(layoutFile.FullName);

            return publishedLayout;
        } else {
            return null;
        }
    }
    
    private async Task<Dictionary<string, PublishedPartial>> GetPartialsAsync(DirectoryInfo directory) {
        var publishedPartials = new Dictionary<string, PublishedPartial>();
        
        var partialsDirectory = new DirectoryInfo(Path.Combine(directory.FullName, "partials"));

        if (partialsDirectory.Exists) {
            foreach (var partialFile in partialsDirectory.GetFiles("*.handlebars", SearchOption.TopDirectoryOnly)) {
                var alias = Path.GetFileNameWithoutExtension(partialFile.FullName);
                
                var publishedPartial = new PublishedPartial();
                publishedPartial.Alias = alias;
                publishedPartial.Markup = await File.ReadAllTextAsync(partialFile.FullName);

                publishedPartials[alias] = publishedPartial;
            }
        }
        
        return publishedPartials;
    }
}