using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.Crowdfunding.Konstrukt;

public class CrowdfundingContributionsConfigurator : IKonstruktConfigurator {
    public void Configure(KonstruktConfigBuilder builder) {
        var section = builder.WithSection("crowdfunding");
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
        collection.DisableCreate();
        collection.SetDataViewsBuilder<CrowdfundingContributionStatusDataViewsBuilder>();

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<CrowdfundingContribution> collection) {
        var listView = collection.ListView();
        
        listView.AddField(c => c.Status);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<CrowdfundingContribution> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Donation");
        
        var recordFieldset = recordTab.AddFieldset("Donation");
        recordFieldset.AddField(c => c.Timestamp).MakeReadOnly();
        recordFieldset.AddField(c => c.Status).MakeReadOnly();
    }
}
