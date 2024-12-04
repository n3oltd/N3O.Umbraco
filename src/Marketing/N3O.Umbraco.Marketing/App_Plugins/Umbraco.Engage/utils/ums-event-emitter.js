angular.module("engage").service("umsEventEmitter", ["umsUtilService", function (umsUtilService) {
    var eventEmitter = umsUtilService.createEventEmitter();

    this.on = eventEmitter.on;
    this.once = eventEmitter.once;
    this.onAll = eventEmitter.onAll;
    this.emit = eventEmitter.emit;
    this.emitCancelable = eventEmitter.emitCancelable;
}]);
