using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Donations.ViewModels;

public class PriceHandleViewModel {
    public PriceHandleViewModel(int index, MoneyRes value, string description) {
        Index = index;
        Value = value;
        Description = description;
    }

    public int Index { get; }
    public MoneyRes Value { get; }
    public string Description { get; }
}
