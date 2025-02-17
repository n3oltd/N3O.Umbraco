using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public class FormatMoneyAmount : Helper {
    private readonly ILookups _lookups;
    private readonly IFormatter _formatter;

    public FormatMoneyAmount(ILogger<FormatMoney> logger,
                             ILookups lookups,
                             IJsonProvider jsonProvider,
                             IFormatter formatter)
        : base(logger, jsonProvider, 2) {
        _lookups = lookups;
        _formatter = formatter;
    }

    protected override void Execute(EncodedTextWriter writer,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var amount = args.Get<decimal?>(0);
        var currencyCode = args.Get<string>(1);

        var currency = _lookups.FindById<Currency>(currencyCode);

        var output = amount.HasValue()
                         ? _formatter.Number.FormatMoney(amount.GetValueOrThrow(), currency)
                         : null;
        
        writer.Write(output);
    }
    
    public override string Name => "formatMoneyAmount";
}
