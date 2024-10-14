﻿using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    private readonly IContributionRepository _contributionRepository;
    private readonly ILookups _lookups;

    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                IContributionRepository contributionRepository,
                                ILookups lookups) 
        : base(asyncKeyedLocker) {
        _contributionRepository = contributionRepository;
        _lookups = lookups;
    }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        foreach (var offlineDonation in req.Model.OrEmpty(x => x?.Transactions?.Donations)) {
            var value = GetMoney(offlineDonation.Amount.Quote.Amount, offlineDonation.Amount.Quote.Currency.Code);
            var anonymous = !offlineDonation.AccountEmail.HasValue();

            await _contributionRepository.AddOfflineContributionAsync(offlineDonation.AllocationReference,
                                                                      offlineDonation.Date,
                                                                      req.Model.Crowdfunder,
                                                                      offlineDonation.AccountEmail,
                                                                      offlineDonation.AccountName,
                                                                      anonymous,
                                                                      false,
                                                                      offlineDonation.FundDimensionValues.Dimension1,
                                                                      offlineDonation.FundDimensionValues.Dimension2,
                                                                      offlineDonation.FundDimensionValues.Dimension3,
                                                                      offlineDonation.FundDimensionValues.Dimension4,
                                                                      value,
                                                                      GivingTypes.Donation);
        }

        await _contributionRepository.CommitAsync();
    }

    private Money GetMoney(decimal amount, string currencyCode) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.Code == currencyCode);

        return new Money(amount, currency);
    }
}