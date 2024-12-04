angular.module("engage.UmbracoForms").component("umsUfRecordSubmissions", {
    templateUrl: "/App_Plugins/Umbraco.Engage.Forms/ums-uf-record-submissions.html",
    bindings: {
        visitorId: "<?",
    },
    controller: ["umsUfRecordSubmissionsApi", function umsUfSubmissions(api) {
        this.state = {
            records: [],
            record: null,
            loading: false,
        };

        this.$onInit = function () {
            if (this.visitorId != null) {
                this.state.loading = true;

                api.getRecordsForVisitor(this.visitorId).then(function (records) {
                    this.state.records = records || [];
                }.bind(this)).finally(function () {
                    this.state.loading = false;
                }.bind(this));
            }
        }
    }]
});
