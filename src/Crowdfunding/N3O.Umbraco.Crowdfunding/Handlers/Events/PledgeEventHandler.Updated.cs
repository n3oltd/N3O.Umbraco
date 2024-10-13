using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief;
using NodaTime.Extensions;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    private readonly ICrowdfundingUrlBuilder _urlBuilder;
    private readonly IForexConverter _forexConverter;
    private readonly IJsonProvider _jsonProvider;
    private readonly ILocalClock _localClock;
    private readonly ILookups _lookups;
    private readonly ITaxReliefSchemeAccessor _taxReliefSchemeAccessor;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    
    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                IContentLocator contentLocator,
                                IServiceProvider serviceProvider,
                                ICrowdfundingUrlBuilder urlBuilder,
                                IForexConverter forexConverter,
                                IJsonProvider jsonProvider,
                                ILocalClock localClock,
                                ILookups lookups,
                                ITaxReliefSchemeAccessor taxReliefSchemeAccessor,
                                IUmbracoDatabaseFactory umbracoDatabaseFactory)
        : base(asyncKeyedLocker, contentLocator, serviceProvider) {
        _urlBuilder = urlBuilder;
        _forexConverter = forexConverter;
        _jsonProvider = jsonProvider;
        _localClock = localClock;
        _lookups = lookups;
        _taxReliefSchemeAccessor  = taxReliefSchemeAccessor;
        _umbracoDatabaseFactory  = umbracoDatabaseFactory;
    }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        // TODO Talha
        // Update the webhook donation model to include the email, reference and name
        // Insert these into the contributions table via repository (remember to delete existing entries)
        // Use allocation reference for transaction reference
        // If the donation is anonymous then put name as _formatter("Anonymous") and email as _formatter("anonymous") (lowercase)
        // We also need to update the crowdfunders table to update the non donations balance.
        var crowdfunder = GetCrowdfunderContent(req.Model.Crowdfunder);

        var contributions = new List<Contribution>();

        foreach (var offlineDonation in req.Model.Transactions.Donations) {
            var contribution = await GetContributionAsync(crowdfunder, offlineDonation, GivingTypes.Donation);

            contributions.Add(contribution);
        }

        await CommitOfflineContributionsAsync(contributions);
    }

    private async Task CommitOfflineContributionsAsync(IReadOnlyList<Contribution> contributions) {
        if (contributions.Any()) {
            var toRemove = contributions.Select(x => x.TransactionReference).ToArray();
            var sql = new Sql("DELETE FROM Contribution WHERE Id IN (@0)", toRemove);
            
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await db.DeleteAsync(sql);
                await db.InsertBatchAsync(contributions);
            }
        }
    }

    private async Task<Contribution> GetContributionAsync(ICrowdfunderContent crowdfunder,
                                                          WebhookPledgeDonation donation,
                                                          GivingType givingType) {
        var taxReliefScheme = _taxReliefSchemeAccessor.GetScheme();
        var date = donation.Date.ToDateTimeUnspecified()
                           .ToInstant()
                           .InZone(_localClock.GetZone())
                           .Date;

        var baseValue = GetMoneyValue(donation.Amount.Base.Amount, donation.Amount.Base.Currency.Code);
        var quoteValue = GetMoneyValue(donation.Amount.Quote.Amount, donation.Amount.Base.Currency.Code);

        var crowdfunderForex = (await _forexConverter.BaseToQuote()
                                                     .UsingRateOn(date)
                                                     .ToCurrency(crowdfunder.Currency)
                                                     .ConvertAsync(baseValue.Amount));

        var contribution = new Contribution();
        contribution.Timestamp = donation.Date.ToDateTimeUnspecified();
        contribution.Date = date.ToDateTimeUnspecified();
        contribution.CampaignId = crowdfunder.CampaignId;
        contribution.CampaignName = crowdfunder.CampaignName;
        contribution.TeamId = crowdfunder.TeamId;
        contribution.TeamName = crowdfunder.TeamName;
        contribution.FundraiserId = crowdfunder.FundraiserId;
        contribution.FundraiserUrl = crowdfunder.FundraiserId.HasValue() ? crowdfunder.Url(_urlBuilder) : null;
        contribution.TransactionReference = donation.AllocationReference;
        contribution.GivingTypeId = givingType.Id;
        contribution.CurrencyCode = quoteValue.Currency.Code;
        contribution.QuoteAmount = quoteValue.Amount;
        contribution.BaseAmount = baseValue.Amount;
        contribution.CrowdfunderAmount = crowdfunderForex.Base.Amount;
        contribution.TaxReliefQuoteAmount = 0m;
        contribution.TaxReliefBaseAmount = 0m;
        contribution.TaxReliefCrowdfunderAmount = 0m;
        contribution.Anonymous = !donation.AccountEmail.HasValue();
        contribution.Name = donation.AccountName;
        contribution.Email = donation.AccountEmail;
        contribution.Comment = null;
        contribution.Status = ContributionStatuses.Visible;
        contribution.ContributionType = (int) ContributionType.Offline;
        contribution.AllocationSummary = donation.Summary;
        contribution.FundDimension1 = donation.FundDimensionValues.Dimension1;
        contribution.FundDimension2 = donation.FundDimensionValues.Dimension2;
        contribution.FundDimension3 = donation.FundDimensionValues.Dimension3;
        contribution.FundDimension4 = donation.FundDimensionValues.Dimension4;

        return contribution;
    }

    private Money GetMoneyValue(decimal amount, string currencyCode) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.Code == currencyCode);

        return new Money(amount, currency);
    }
}