using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

public class CrowdfundingContributionsConfigurator : IKonstruktConfigurator {
    public void Configure(KonstruktConfigBuilder builder) {
        var section = builder.AddSectionAfter("media", "Crowdfunding");
        var dashboard = section.AddDashboard("Crowdfunding Donations");

        ConfigureCollection(dashboard);
    }

    private void ConfigureCollection(KonstruktDashboardConfigBuilder dashboard) {
        var collection = dashboard.SetCollection<CrowdfundingContribution>(x => x.Id,
                                                                           "Crowdfunding Donation",
                                                                           "Crowdfunding Donations",
                                                                           "Crowdfunding Donations List",
                                                                           "icon-arrow-up",
                                                                           "icon-arrow-up");

        collection.SetAlias("allCrowdfundingDonations");
        collection.SetDateCreatedProperty(c => c.Timestamp);
        collection.SetNameProperty(c => c.CheckoutReference);
        collection.AddFilterableProperty(c => c.CheckoutReference);
        collection.AddSearchableProperty(c => c.Email);
        collection.AddSearchableProperty(c => c.Name);
        collection.AddSearchableProperty(c => c.TeamName);
        collection.AddSearchableProperty(c => c.CampaignName);
        collection.AddSearchableProperty(c => c.Comment);
        collection.DisableCreate();
        collection.DisableDelete();
        collection.SetDataViewsBuilder<CrowdfundingContributionStatusDataViewsBuilder>();

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<CrowdfundingContribution> collection) {
        var listView = collection.ListView();
        
        listView.AddField(c => c.CampaignName);
        listView.AddField(c => c.TeamName);
        listView.AddField(c => c.Timestamp);
        listView.AddField(c => c.Status);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<CrowdfundingContribution> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Donation");
        
        var recordFieldset = recordTab.AddFieldset("Donation");
        recordFieldset.AddField(c => c.CheckoutReference).MakeReadOnly();
        recordFieldset.AddField(c => c.Name).MakeReadOnly();
        recordFieldset.AddField(c => c.Email).MakeReadOnly();
        recordFieldset.AddField(c => c.Currency).MakeReadOnly();
        recordFieldset.AddField(c => c.BaseAmount).MakeReadOnly();
        recordFieldset.AddField(c => c.PageUrl).MakeReadOnly();
        recordFieldset.AddField(c => c.CampaignName).MakeReadOnly();
        recordFieldset.AddField(c => c.TeamName).MakeReadOnly();
        recordFieldset.AddField(c => c.BaseTaxReliefAmount).MakeReadOnly();
        recordFieldset.AddField(c => c.Timestamp).MakeReadOnly();
        recordFieldset.AddField(c => c.Status).MakeReadOnly();
        recordFieldset.AddField(c => c.Comment);
    }
}
