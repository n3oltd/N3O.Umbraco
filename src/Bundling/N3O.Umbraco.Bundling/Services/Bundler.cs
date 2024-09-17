using N3O.Umbraco.Extensions;
using Smidge;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Bundling;

public class Bundler : IBundler {
    private readonly SmidgeHelper _smidgeHelper;
    private readonly IReadOnlyList<IAssetBundle> _assetBundles;
    private bool _processedAssetBundles;

    public Bundler(SmidgeHelper smidgeHelper, IEnumerable<IAssetBundle> assetBundles) {
        _smidgeHelper = smidgeHelper;
        _assetBundles = assetBundles.ApplyAttributeOrdering();
    }

    public async Task<IEnumerable<string>> GenerateCssUrlsAsync() {
        ProcessAssetBundles();
        
        return await _smidgeHelper.GenerateCssUrlsAsync();
    }

    public async Task<IEnumerable<string>> GenerateJsUrlsAsync() {
        ProcessAssetBundles();
        
        return await _smidgeHelper.GenerateJsUrlsAsync();
    }

    public IBundler RequiresCss(params string[] paths) {
        _smidgeHelper.RequiresCss(paths);

        return this;
    }

    public IBundler RequiresJs(params string[] paths) {
        _smidgeHelper.RequiresJs(paths);

        return this;
    }

    private void ProcessAssetBundles() {
        if (!_processedAssetBundles) {
            foreach (var assetBundle in _assetBundles) {
                assetBundle.Require(_smidgeHelper);
            }
            
            _processedAssetBundles = true;
        }
    }
}