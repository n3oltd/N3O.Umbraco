angular.module("umbraco").component("segmentRuleTelethonOnAirEditor", {
    templateUrl: "/App_Plugins/telethon-on-air-rule/segment-rule-telethon-on-air-editor.html",
    bindings: {
        rule: "<",
        config: "<",
        save: "&",
    },
});
