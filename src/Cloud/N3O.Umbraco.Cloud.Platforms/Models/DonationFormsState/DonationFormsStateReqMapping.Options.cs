using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using GiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;
using OurGiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class DonationFormsStateReqMapping {
    private DonationFormOptionsReq GetDonationFormOptionsReq(MapperContext ctx,
                                                             DonationFormStateContent formState,
                                                             string notesLabel,
                                                             CampaignContent campaign) {
        var oneTimeSuggestedAmounts = formState.Fund?.OneTimeSuggestedAmounts.OrEmpty().ToList();
        var recurringSuggestedAmounts = formState.Fund?.RecurringSuggestedAmounts.OrEmpty().ToList();

        var options = new DonationFormOptionsReq();

        if (oneTimeSuggestedAmounts.HasAny() || recurringSuggestedAmounts.HasAny()) {
            options.SuggestedAmounts = GetDonationFormSuggestedAmountsReq(ctx,
                                                                          (GiftTypes.OneTime, oneTimeSuggestedAmounts),
                                                                          (GiftTypes.Recurring, recurringSuggestedAmounts));
        }

        SetNotesField(options, notesLabel);
        SetQurbaniOptions(options, campaign);

        return options;
    }

    private void SetNotesField(DonationFormOptionsReq options, string notesLabel) {
        options.NotesField = new DonationFormNotesFieldReq();

        if (notesLabel.HasValue()) {
            options.NotesField.Visible = true;
            options.NotesField.Label = notesLabel;
        } else {
            options.NotesField.Visible = false;
        }
    }

    private void SetQurbaniOptions(DonationFormOptionsReq options, CampaignContent campaign) {
        if (campaign.HasValue() && campaign.Qurbani?.Season?.ContentId.HasValue == true) {
            var seasonContent = _contentLocator.ById<QurbaniSeasonContent>(campaign.Qurbani.Season.ContentId.Value);

            if (seasonContent.HasValue()) {
                options.Qurbani = new DonationFormQurbaniOptionsReq();
                options.Qurbani.Categories = seasonContent.Categories
                                                          .OrEmpty()
                                                          .Select(c => c.Content().Key.ToString())
                                                          .ToList();
            }
        }
    }

    private List<DonationFormSuggestedAmountsReq> GetDonationFormSuggestedAmountsReq(MapperContext ctx,
                                                                                     params (OurGiftType GiftType, IEnumerable<DonationFormStateSuggestedAmountElement> SuggestedAmounts)[] suggestedAmountsElements) {
        var items = new List<DonationFormSuggestedAmountsReq>();

        foreach (var suggestedAmountsElement in suggestedAmountsElements) {
            if (suggestedAmountsElement.SuggestedAmounts.HasAny()) {
                var req = new DonationFormSuggestedAmountsReq();
                req.GiftType = suggestedAmountsElement.GiftType.ToEnum<GiftType>();
                req.Amounts = suggestedAmountsElement.SuggestedAmounts.Select(ctx.Map<DonationFormStateSuggestedAmountElement, DonationFormSuggestedAmountReq>).ToList();

                items.Add(req);
            }
        }

        return items;
    }
}
