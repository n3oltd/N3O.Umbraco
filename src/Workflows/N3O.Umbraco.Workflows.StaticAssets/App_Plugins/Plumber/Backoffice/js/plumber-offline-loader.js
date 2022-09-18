(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";
let files = [
    '/umbraco/lib/jquery/jquery.min.js',
    '/umbraco/lib/angular/angular.min.js',
    '/umbraco/lib/angular-route/angular-route.min.js',
    '/umbraco/lib/angular-sanitize/angular-sanitize.min.js',
    '/umbraco/lib/angular-ui-sortable/sortable.min.js',
    '/umbraco/lib/underscore/underscore-min.js',
    '/umbraco/lib/umbraco/Extensions.js',
    '/umbraco/js/app.min.js',
    '/umbraco/js/umbraco.resources.min.js',
    '/umbraco/js/umbraco.directives.min.js',
    '/umbraco/js/umbraco.filters.min.js',
    '/umbraco/js/umbraco.services.min.js',
    '/umbraco/js/umbraco.interceptors.min.js',
    '/umbraco/js/umbraco.controllers.min.js',
    '/umbraco/js/routes.min.js',
    '/umbraco/js/init.min.js',
    '/App_Plugins/plumber/backoffice/js/plumber-offline.js'
];
LazyLoad.js(files, function () {
    angular.bootstrap(document, ['plumber']);
});

},{}]},{},[1]);

//# sourceMappingURL=plumber-offline-loader.js.map
