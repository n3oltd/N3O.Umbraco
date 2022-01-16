using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Donations.Content;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations {
    public class DonationForms : IDonationForms {
        private readonly IContentCache _contentCache;
        private readonly IContentLocator _contentLocator;

        public DonationForms(IContentCache contentCache, IContentLocator contentLocator) {
            _contentCache = contentCache;
            _contentLocator = contentLocator;
        }
    
        public IReadOnlyList<DonationFormContent> All() {
            return _contentCache.All<DonationFormContent>();
        }

        public DonationOptionContent GetOption(Guid id) {
            var content = _contentLocator.ById(id);

            return content.As<DonationOptionContent>();
        }
    }
}
