angular.module("engage.UmbracoForms").service("umsUfRecordSubmissionsApi", [
    "$http",
    function umsUfRecordSubmissionsApi($http) {
        var rootUrl = "/umbraco/backoffice/engageumbracoforms/recordsubmissions/";

        this.getRecordsForVisitor = function (visitorId) {
            return $http.get(rootUrl + "getrecordsforvisitor?visitorId=" + encodeURIComponent(visitorId)).then(function (response) {
                return response.data || [];
            });
        }
    }
]);
