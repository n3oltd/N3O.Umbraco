using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public class AllocationsLookupTypes : ILookupTypesSet {
        [LookupInfo(typeof(AllocationType))]
        public const string AllocationTypes = "allocationTypes";
        
        [LookupInfo(typeof(DonationItem))]
        public const string DonationItems = "donationItems";
        
        [LookupInfo(typeof(DonationType))]
        public const string DonationTypes = "donationTypes";
        
        [LookupInfo(typeof(FundDimension1Option))]
        public const string FundDimension1Options = "fundDimension1Options";
        
        [LookupInfo(typeof(FundDimension2Option))]
        public const string FundDimension2Options = "fundDimension2Options";
        
        [LookupInfo(typeof(FundDimension1Option))]
        public const string FundDimension3Options = "fundDimension3Options";
        
        [LookupInfo(typeof(FundDimension4Option))]
        public const string FundDimension4Options = "fundDimension4Options";
        
        [LookupInfo(typeof(SponsorshipScheme))]
        public const string SponsorshipSchemes = "sponsorshipSchemes";
    }
}