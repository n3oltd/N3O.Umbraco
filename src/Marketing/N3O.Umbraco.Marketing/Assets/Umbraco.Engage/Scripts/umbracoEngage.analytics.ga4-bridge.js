(function umbracoEngage_GA4_bridge() {
    // If "Enhanced Measurement" is enabled Google sends some events by default which generates a lot of events.
    // This GA4 bridge is meant to send custom client events to Umbraco Engage and we do not want all these Enhanced Measurement events to be sent.
    // https://support.google.com/analytics/answer/9234069?hl=en
    var ENHANCED_MEASUREMENT_EVENTS = [
        "click",
        "file_download",
        "form_start",
        "form_submit",
        "page_view",
        "scroll",
        "video_complete",
        "video_progress",
        "video_start",
        "view_search_results"
    ];
    function bridgeGA4ToUmbEngage(originalReturn, arguments) {
        if (typeof window.umbEngage !== "function" || arguments == null || arguments.length === 0) {
            // We require:
            // * window.umbEngage to be defined, which is initialized by umbracoEngage.analytics.js.
            // * at least 1 argument to sendBeacon or Fetch since that is the URL we inspect.
            return originalReturn;
        }

        var url = arguments[0];
        if (typeof url !== "string" || url.length === 0 ||
            (url.indexOf("google-analytics") === -1 &&
                url.indexOf("analytics.google.com") === -1)) {
            // Only operate on URLs containing "google-analytics" or "analytics.google.com"
            return originalReturn;
        }

        var eventNameRe = /\ben=([^&]+)/;
        var match = url.match(eventNameRe);
        if (match == null || match.length !== 2) {
            // We require a length of exactly 2 containing:
            // * Full match
            // * Capture group 1 (event name)
            return originalReturn;
        }

        let eventName = match[1];
        if (eventName == null || eventName.length === 0 || ENHANCED_MEASUREMENT_EVENTS.indexOf(eventName) > -1) {
            // Do not send empty event names or Enhanced Measurement events to Umbraco Engage
            return originalReturn;
        }

        let category = "GA 4 - Bridging";
        window.umbEngage("send", "event", category, eventName);

        return originalReturn;
    }

    // Check if Navigator.sendBeacon is implemented in this browser.
    if (window.navigator != null &&
        window.navigator.constructor.prototype.toString() === "[object Navigator]" &&
        typeof window.navigator.sendBeacon === "function") {
        
        // Beacon based Analytics Collection (for 'old' GA4 implementation < 25-06-2024)
        let sendBeaconFn = window.navigator.sendBeacon;
        window.navigator.sendBeacon = function sendBeacon_umbEngage_GA4_bridge() {
            // Call original Navigator.sendBeacon
            let originalReturn = sendBeaconFn.apply(this, arguments);
            return bridgeGA4ToUmbEngage(originalReturn, arguments);
        }
    }
    
    // Fetch based Analytics Collection (For new GA4 implementation > 25-06-2024)
    let originalFetch = window.fetch;
    window.fetch = function() {
        // Call original Fetch and bridge the event to Umbraco Engage
        return originalFetch.apply(this, arguments).then(originalReturn => {
            if (arguments[1] && arguments[1].method && arguments[1].method.toUpperCase() === 'POST') {
                // We only want to bridge POST fetch requests
                return bridgeGA4ToUmbEngage(originalReturn, arguments);
            }
            return originalReturn;
        });
    };
})();
