angular.module("engage").service("umsErrorHandler", [
    "notificationsService",
    function umsErrorHandler(notificationsService) {
        this.handle = function handle(err) {
            var errorMessage;

            if (err != null && err.xhrStatus !== undefined) {
                errorMessage = getXhrError(err);
            } else {
                errorMessage = "An unknown error has occurred";
            }

            notificationsService.error(errorMessage);
        }

        function getXhrError(err) {
            if (err.data != null && err.data.Message != null && err.data.Message.length > 0) {
                return err.data.Message;
            } else if (err.statusText != null && err.statusText.length > 0) {
                return err.statusText;
            } else if (err.status != null) {
                return "Server returned an error response with status code " + err.status;
            } else {
                return "Server returned an unknown error response";
            }
        }
    }
]);
