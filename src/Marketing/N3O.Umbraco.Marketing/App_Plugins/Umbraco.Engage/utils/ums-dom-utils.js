angular.module("engage").service("umsDomUtils", [
    "$rootScope",
    umsDomUtils
]);

function umsDomUtils($rootScope) {
    this.onClassChange = (function() {
        // DOM node -> { observer: <MutationObserver>, callbacks: [] }
        var map = new Map();

        function startObserver(element, className, callbacks) {
            var classRegex = new RegExp("(^| )" + className + "( |$)", "i");

            var observer = new MutationObserver(function onMutation(mutations) {
                for (var i = 0; i < mutations.length; i++) {
                    var mutation = mutations[i];
                    var hasClass = mutation.target.classList.contains(className);
                    var hadClass = classRegex.test(mutation.oldValue);
                    var isChange = hasClass !== hadClass;

                    if (isChange) {
                        for (var i = 0; i < callbacks.length; i++) {
                            callbacks[i](hasClass);
                        }

                        // Notify AngularJS
                        $rootScope.$applyAsync();
                    }
                }
            });

            observer.observe(element, {
                attributes: true,
                attributeFilter: ["class"],
                attributeOldValue: true,
            });

            return observer;
        }

        function unsubscribe(element, callback) {
            var subscription = map.get(element);

            if (subscription != null) {
                var idx = subscription.callbacks.indexOf(callback);
                if (idx > -1) {
                    subscription.callbacks.splice(idx, 1);
                    if (subscription.callbacks.length === 0) {
                        subscription.observer.disconnect();
                        map.delete(element);
                    }
                }
            }
        }

        return function onClassChange(element, className, callback) {
            if (element == null) {
                throw new Error("element should be a DOM node");
            }

            if (typeof callback !== "function") {
                throw new Error("callback should be a Function");
            }

            var subscription = map.get(element);

            if (subscription == null) {
                var callbacks = [callback];
                var observer = startObserver(element, className, callbacks);
                subscription = {
                    observer: observer,
                    callbacks: callbacks,
                };

                map.set(element, subscription);
            } else {
                subscription.callbacks.push(callback);
            }

            return unsubscribe.bind(null, element, callback);
        };
    })();
}
