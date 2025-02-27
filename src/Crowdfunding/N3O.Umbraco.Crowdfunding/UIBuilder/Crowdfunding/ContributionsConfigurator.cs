﻿using Konstrukt.Configuration.Builders;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.UIBuilder;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

public class ContributionsConfigurator : KonstruktConfigurator {
    public override void Configure(KonstruktConfigBuilder builder) {
        var section = GetContentSection(builder);
        var dashboard = section.AddDashboard("Crowdfunding");
        dashboard.SetVisibility(cfg => cfg.ShowForUserGroup("editor"));

        ConfigureCollection(dashboard);
    }

    private void ConfigureCollection(KonstruktDashboardConfigBuilder dashboard) {
        var collection = dashboard.SetCollection<Contribution>(x => x.Id,
                                                               "Crowdfunding Donation",
                                                               "Crowdfunding Donations",
                                                               "Crowdfunding Donations List",
                                                               "icon-arrow-up",
                                                               "icon-arrow-up");

        collection.SetAlias("allCrowdfundingDonations");
        collection.SetDateCreatedProperty(c => c.Timestamp);
        collection.SetNameProperty(c => c.TransactionReference);
        collection.AddFilterableProperty(c => c.TransactionReference);
        collection.AddSearchableProperty(c => c.Email);
        collection.AddSearchableProperty(c => c.Name);
        collection.AddSearchableProperty(c => c.TeamName);
        collection.AddSearchableProperty(c => c.CampaignName);
        collection.AddSearchableProperty(c => c.FundraiserName);
        collection.AddSearchableProperty(c => c.Comment);
        collection.DisableCreate();
        collection.DisableDelete();
        collection.SetDataViewsBuilder<ContributionStatusDataViewsBuilder>();

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<Contribution> collection) {
        var listView = collection.ListView();
        
        listView.AddField(c => c.CampaignName);
        listView.AddField(c => c.TeamName);
        listView.AddField(c => c.Timestamp);
        listView.AddField(c => c.Status);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<Contribution> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Donation");
        
        var recordFieldset = recordTab.AddFieldset("Donation");
        recordFieldset.AddField(c => c.TransactionReference).MakeReadOnly();
        recordFieldset.AddField(c => c.Name).MakeReadOnly();
        recordFieldset.AddField(c => c.Email).MakeReadOnly();
        recordFieldset.AddField(c => c.CurrencyCode).MakeReadOnly();
        recordFieldset.AddField(c => c.QuoteAmount).MakeReadOnly();
        recordFieldset.AddField(c => c.CrowdfunderUrl).MakeReadOnly();
        recordFieldset.AddField(c => c.CampaignName).MakeReadOnly();
        recordFieldset.AddField(c => c.FundraiserName).MakeReadOnly();
        recordFieldset.AddField(c => c.TeamName).MakeReadOnly();
        recordFieldset.AddField(c => c.TaxReliefQuoteAmount).MakeReadOnly();
        recordFieldset.AddField(c => c.Timestamp).MakeReadOnly();
        recordFieldset.AddField(c => c.Status).MakeReadOnly();
        recordFieldset.AddField(c => c.Comment);
    }
}
