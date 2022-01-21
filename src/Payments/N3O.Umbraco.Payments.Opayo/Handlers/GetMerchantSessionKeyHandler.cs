using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class GetMerchantSessionKeyHandler :
        IRequestHandler<GetMerchantSessionKeyCommand, None, MerchantSessionKeyRes> {
        private readonly IOpayoClient _opayoClient;
        private readonly OpayoApiSettings _apiSettings;
        private readonly IUmbracoMapper _mapper;

        public GetMerchantSessionKeyHandler(IOpayoClient opayoClient,
                                            OpayoApiSettings apiSettings,
                                            IUmbracoMapper mapper) {
            _opayoClient = opayoClient;
            _apiSettings = apiSettings;
            _mapper = mapper;
        }

        public async Task<MerchantSessionKeyRes> Handle(GetMerchantSessionKeyCommand req,
                                                        CancellationToken cancellationToken) {
            var apiReq = new ApiMerchantSessionKeyReq();
            apiReq.VendorName = _apiSettings.VendorName;
            
            var merchantSessionKey = await _opayoClient.GetMerchantSessionKeyAsync(apiReq);
            var res = _mapper.Map<IMerchantSessionKey, MerchantSessionKeyRes>(merchantSessionKey);

            return res;
        }
    }
}