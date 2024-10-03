using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.OpenGraph;
using Smidge;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public abstract class CrowdfundingPage : ICrowdfundingPage {
    protected CrowdfundingPage(IContentLocator contentLocator, ICrowdfundingViewModelFactory viewModelFactory) {
        ContentLocator = contentLocator;
        ViewModelFactory = viewModelFactory;
    }

    public virtual void AddAssets(ISmidgeRequire bundle) { }
    
    public void AddOpenGraph(IOpenGraphBuilder builder,
                             Uri requestUri,
                             IReadOnlyDictionary<string, string> requestQuery) {
        var crowdfundingPath = CrowdfundingPathParser.ParseUri(ContentLocator, requestUri);

        AddOpenGraph(builder, crowdfundingPath, requestQuery);
    }

    public bool IsMatch(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery) {
        var crowdfundingPath = CrowdfundingPathParser.ParseUri(ContentLocator, requestUri);

        if (crowdfundingPath.HasValue()) {
            return IsMatch(crowdfundingPath, requestQuery);
        } else {
            return false;
        }
    }

    public async Task<ICrowdfundingViewModel> GetViewModelAsync(Uri requestUri,
                                                                IReadOnlyDictionary<string, string> requestQuery) {
        var crowdfundingPath = CrowdfundingPathParser.ParseUri(ContentLocator, requestUri);

        return await GetViewModelAsync(crowdfundingPath, requestQuery);
    }

    protected bool IsMatch(string crowdfundingPath, string pattern) {
        return Regex.IsMatch(crowdfundingPath, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
    
    protected Match Match(string crowdfundingPath, string pattern) {
        return Regex.Match(crowdfundingPath, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }

    protected abstract void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query);
    
    protected abstract bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query);
    
    protected abstract Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                      IReadOnlyDictionary<string, string> query);

    public virtual PageAccess Access => PageAccesses.Anonymous;
    public virtual bool NoCache => true;
    public virtual string ViewName => $"~/Views/Partials/Crowdfunding/Pages/{GetType().Name}.cshtml";

    protected IContentLocator ContentLocator { get; }
    protected ICrowdfundingViewModelFactory ViewModelFactory { get; }
    
    protected static string GenerateUrl(IContentLocator contentLocator,
                                        string path,
                                        Action<Url> addQueryParameters = null) {
        var home = contentLocator.Single<HomePageContent>();

        var url = new Url(home.Content().AbsoluteUrl());

        if (path.HasValue()) {
            url.AppendPathSegment(path);
        }

        addQueryParameters?.Invoke(url);

        return url.ToString();
    }
}