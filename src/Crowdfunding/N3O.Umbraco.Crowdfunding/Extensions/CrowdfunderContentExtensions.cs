using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CrowdfunderContentExtensions {
    public static async Task<ForexMoney> GetGoalsTotalAsync(this ICrowdfunderContent crowdfunderContent,
                                                            IForexConverter forexConverter) {
        var total = crowdfunderContent.Goals.Sum(x => x.Amount);

        return await forexConverter.QuoteToBase()
                                   .FromCurrency(crowdfunderContent.Currency)
                                   .ConvertAsync(total);
    }
}