window.umbracoEngage = (function (umbEngage) {
    var isInitialized = false;
    var onInitFns = [];

    function onInit(fn) {
        if (isInitialized) fn();
        else onInitFns.push(fn);
    }

    function init() {
        for (var i = 0; i < onInitFns.length; i++) {
            onInitFns[i]();
        }

        isInitialized = true;
    }

    umbEngage.onInit = onInit;
    umbEngage.init = init;

    return umbEngage;
})(window.umbracoEngage || {});

(function buildBridge() {
    // Original Google "ga" function
    // If window.ga is not initialized yet set it to the standard Google Analytics "setup" function.
    // This ensures we can still send events to Umbraco Engage even when analytics.js is blocked by the client
    // and we are prepared for async loading of Google Analytics (e.g. after accepting cookies).
    var _ga = window.ga || function () { (window.ga.q = window.ga.q || []).push(arguments) };

    function ga() {
        var args = arguments;

        if (typeof _ga === "function") {
            _ga.apply(null, args);
        }

        window.umbracoEngage.onInit(function () {
            if (typeof window.umbEngage === "function") {
                window.umbEngage.apply(null, args);
            }
        });
    }

    Object.defineProperty(window, "ga", {
        get: function () {
            return ga;
        },

        set: function (value) {
            if (value !== ga) {
                _ga = value
            }
        }
    });
})();
