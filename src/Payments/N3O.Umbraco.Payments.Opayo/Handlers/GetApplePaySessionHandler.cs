using System;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Handlers;

public class GetApplePaySessionHandler :
    IRequestHandler<GetApplePaySessionCommand, None, ApplePaySessionRes> {
    private readonly IOpayoClient _opayoClient;
    private readonly OpayoApiSettings _apiSettings;
    private readonly IUmbracoMapper _mapper;

    public GetApplePaySessionHandler(IOpayoClient opayoClient,
                                        OpayoApiSettings apiSettings,
                                        IUmbracoMapper mapper) {
        _opayoClient = opayoClient;
        _apiSettings = apiSettings;
        _mapper = mapper;
    }

    public async Task<ApplePaySessionRes> Handle(GetApplePaySessionCommand req,
                                                    CancellationToken cancellationToken) {
        var apiReq = new ApiApplePaySessionReq();
        apiReq.VendorName = _apiSettings.VendorName;
        
        var uri = new Uri(_apiSettings.BaseUrl);
        apiReq.DomainName = uri.Host;
        
        var applePaySession = await _opayoClient.GetApplePaySessionAsync(apiReq);
        
        var res = _mapper.Map<IApplePaySession, ApplePaySessionRes>(applePaySession);

        return res;
    }
}
