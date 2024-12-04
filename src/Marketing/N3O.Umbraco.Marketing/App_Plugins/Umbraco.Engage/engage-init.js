// Register with Angular
angular.module("engage", ["rzSlider", "as.sortable"]);

// Register as dependency for Umbraco
angular.module("umbraco").requires.push("engage");
