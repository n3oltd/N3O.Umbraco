using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingRouter : ICrowdfundingRouter {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnumerable<ICrowdfundingPage> _crowdfundingPages;
    private ICrowdfundingPage _currentPage;
    private Uri _requestUri;
    private IReadOnlyDictionary<string, string> _requestQuery;

    public CrowdfundingRouter(IHttpContextAccessor httpContextAccessor,
                              IEnumerable<ICrowdfundingPage> crowdfundingPages) {
        _httpContextAccessor = httpContextAccessor;
        _crowdfundingPages = crowdfundingPages;
    }

    public ICrowdfundingPage CurrentPage {
        get {
            if (_currentPage == null) {
                foreach (var crowdfundingPage in _crowdfundingPages) {
                    if (crowdfundingPage.IsMatch(RequestUri, RequestQuery)) {
                        _currentPage = crowdfundingPage;
                        
                        break;
                    }
                }
            }

            return _currentPage;
        }
    }

    public Uri RequestUri {
        get {
            if (_requestUri == null) {
                _requestUri = _httpContextAccessor.HttpContext.Request.Uri();
            }

            return _requestUri;
        }
    }

    public IReadOnlyDictionary<string, string> RequestQuery {
        get {
            if (_requestQuery == null) {
                _requestQuery = new Dictionary<string, string>(_httpContextAccessor.HttpContext
                                                                                   .Request
                                                                                   .Query
                                                                                   .ToDictionary(x => x.Key,
                                                                                                 x => x.Value.Single()),
                                                               StringComparer.InvariantCultureIgnoreCase);
            }

            return _requestQuery;
        }
    }
}