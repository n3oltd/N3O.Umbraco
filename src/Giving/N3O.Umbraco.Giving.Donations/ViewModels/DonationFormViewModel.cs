using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.ViewModels {
    public class DonationFormViewModel {
        public DonationFormViewModel(IPublishedContent content) {
            var form = content.As<DonationForm>();
        
            Title = form.Title;
            Options = GetOptions(form);
        }

        private IReadOnlyList<Option> GetOptions(DonationForm form) {
            var list = new List<Option>();
        
            foreach (var (option, index) in form.GetOptions().SelectWithIndex()) {
                if (option.Type == AllocationTypes.Fund) {
                    list.Add(GetOption(option.Fund, index));
                } else if (option == AllocationTypes.Sponsorship) {
                    list.Add(GetOption(option.Sponsorship, index));
                } else {
                    UnrecognisedValueException.For(option.Type);
                }
            }

            return list;
        }

        private Option GetOption(FundDonationOption fundOption, int index) {
            var option = new Option(fundOption.Content.Key,
                                    index,
                                    fundOption.Content.Name,
                                    !fundOption.HideRegular && fundOption.DonationItem.AllowRegularDonations,
                                    !fundOption.HideSingle && fundOption.DonationItem.AllowSingleDonations);

            return option;
        }
    
        private Option GetOption(SponsorshipDonationOption sponsorshipOption, int index) {
            var option = new Option(sponsorshipOption.Content.Key,
                                    index,
                                    sponsorshipOption.Content.Name,
                                    true,
                                    true);

            return option;
        }

        public string Title { get; }
        public IReadOnlyList<Option> Options { get; }

        public class Option {
            public Option(Guid id, int index, string name, bool allowRegular, bool allowSingle) {
                Id = id;
                Index = index;
                Name = name;
                AllowRegular = allowRegular;
                AllowSingle = allowSingle;
            }

            public Guid Id { get; }
            public int Index { get; }
            public string Name { get; }
            public bool AllowRegular { get; }
            public bool AllowSingle { get; }
        }
    }
}
