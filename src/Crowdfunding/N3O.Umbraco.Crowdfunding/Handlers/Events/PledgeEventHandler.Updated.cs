using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    private readonly IContributionRepository _contributionRepository;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ILookups _lookups;
    private readonly IForexConverter _forexConverter;
    private readonly IFormatter _formatter;

    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                IContributionRepository contributionRepository,
                                ILookups lookups,
                                IForexConverter forexConverter,
                                IFormatter formatter,
                                ICrowdfunderRepository crowdfunderRepository) 
        : base(asyncKeyedLocker) {
        _contributionRepository = contributionRepository;
        _lookups = lookups;
        _forexConverter = forexConverter;
        _formatter = formatter;
        _crowdfunderRepository = crowdfunderRepository;
    }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        var tasks = new List<Task>();
        
        tasks.Add(UpdateContributionsAsync(req.Model));
        tasks.Add(UpdateCrowdfunderNonDonationsTotalAsync(req.Model));
        
        await Task.WhenAll(tasks);
    }
    
    private async Task UpdateContributionsAsync(WebhookPledge pledge) {
        var offlineDonations = pledge.Transactions.Donations.Where(x => !x.IsImported);
        
        foreach (var offlineDonation in offlineDonations.OrEmpty()) {
            var value = offlineDonation.Amount.Quote.ToMoney(_lookups);
            var anonymous = !offlineDonation.AccountEmail.HasValue();
            var name = anonymous ? _formatter.Text.Format<Strings>(x => x.Anonymous) : offlineDonation.AccountName;
            var email = anonymous ? _formatter.Text.Format<Strings>(x => x.Anonymous.ToLowerInvariant()) : offlineDonation.AccountEmail;

            await _contributionRepository.AddOfflineContributionAsync(offlineDonation.AllocationReference,
                                                                      offlineDonation.Date,
                                                                      pledge.Crowdfunder,
                                                                      email,
                                                                      name,
                                                                      anonymous,
                                                                      false,
                                                                      offlineDonation.FundDimensionValues.Dimension1,
                                                                      offlineDonation.FundDimensionValues.Dimension2,
                                                                      offlineDonation.FundDimensionValues.Dimension3,
                                                                      offlineDonation.FundDimensionValues.Dimension4,
                                                                      value,
                                                                      GivingTypes.Donation);
        }
        
        _contributionRepository.DeleteOfflineContributionsForCrowdfunder(pledge.Crowdfunder.Id);

        await _contributionRepository.CommitAsync();
    }
    
    private async Task UpdateCrowdfunderNonDonationsTotalAsync(WebhookPledge pledge) {
        var quoteMoney = pledge.Transactions.NonDonationsBalance.ToMoney(_lookups);

        var nonDonationsForex = await _forexConverter.QuoteToBase()
                                                     .FromCurrency(quoteMoney.Currency)
                                                     .ConvertAsync(quoteMoney.Amount);

        await _crowdfunderRepository.UpdateNonDonationsTotalAsync(pledge.Crowdfunder.Id, nonDonationsForex);
    }
    
    public class Strings : CodeStrings {
        public string Anonymous => "Anonymous";
    }
}