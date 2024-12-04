angular.module("engage.UmbracoForms", ["engage"]);
angular.module("umbraco").requires.push("engage.UmbracoForms");

angular.module("engage.UmbracoForms").run([
    "UMS_ADDONS",
    function (addons) {
        // Register UmbracoForms addon
        addons.umbracoForms = true;
    }
]);
