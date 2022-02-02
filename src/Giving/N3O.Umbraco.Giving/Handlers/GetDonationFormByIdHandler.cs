using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Handlers {
    public class GetDonationFormByIdHandler : IRequestHandler<GetDonationFormByIdQuery, None, DonationFormRes> {
        private readonly IContentLocator _contentLocator;
        private readonly IUmbracoMapper _mapper;

        public GetDonationFormByIdHandler(IContentLocator contentLocator, IUmbracoMapper mapper) {
            _contentLocator = contentLocator;
            _mapper = mapper;
        }
        
        public Task<DonationFormRes> Handle(GetDonationFormByIdQuery req, CancellationToken cancellationToken) {
            var form = req.DonationFormId.Run(_contentLocator.ById<DonationFormContent>, true);

            var res = _mapper.Map<DonationFormContent, DonationFormRes>(form);

            return Task.FromResult(res);
        }
    }
}