using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.Data.Konstrukt {
    public class ImportsConfigurator : IKonstruktConfigurator {
        public void Configure(KonstruktConfigBuilder builder) {
            var dashboard = builder.AddDashboard("Imports");
            dashboard.SetVisibility(cfg => cfg.ShowInSection("content"));

            ConfigureCollection(dashboard);
        }

        private void ConfigureCollection(KonstruktDashboardConfigBuilder dashboard) {
            var collection = dashboard.SetCollection<Import>(x => x.Id,
                                                                "Import",
                                                                "Imports",
                                                                "Data imports",
                                                                "icon-arrow-up",
                                                                "icon-arrow-up");

            collection.SetAlias("pendingImports");
            collection.SetNameProperty(c => c.Reference);
            collection.SetDateCreatedProperty(c => c.QueuedAt);
            collection.AddSearchableProperty(c => c.QueuedByName);
            collection.AddSearchableProperty(c => c.Reference);
            collection.AddSearchableProperty(c => c.ContentTypeName);
            collection.AddSearchableProperty(c => c.ImportedContentSummary);
            collection.SetFilter(x => x.Status == ImportStatuses.Queued || x.Status == ImportStatuses.Error);
            collection.DisableCreate();

            ConfigureListView(collection);

            ConfigureEditor(collection);
        }

        private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<Import> collection) {
            var listView = collection.ListView();
            listView.SetDataViewsBuilder<ImportActionDataViewsBuilder>();
            listView.SetDataViewsBuilder<ImportStatusDataViewsBuilder>();
            listView.AddField(c => c.QueuedAt);
            listView.AddField(c => c.QueuedByName);
            listView.AddField(c => c.ContentTypeName);
            listView.AddField(c => c.Reference);
            listView.AddField(c => c.ImportedContentSummary);
            listView.AddField(c => c.Status);
        }

        private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<Import> collection) {
            var editor = collection.Editor();
            var generalTab = editor.AddTab("General");
            var generalFieldset = generalTab.AddFieldset("General");

            // TODO Add a component that runs at startup and ensures that there are two data types defined:
            // Import Fields Editor
            // Import Errors Viewer
            //
            // Can then wire these up respectively below.
            
            generalFieldset.AddField(c => c.Fields).SetDataType("Textarea");
            
            var errorsFieldSet = generalTab.AddFieldset("Errors");

            errorsFieldSet.AddField(c => c.Errors).SetDataType("Textarea").MakeReadOnly();
        }
    }
}