// Object.create 
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/create#Polyfill
"function"!=typeof Object.create&&(Object.create=function(e,t){if("object"!=typeof e&&"function"!=typeof e)throw new TypeError("Object prototype may only be an Object: "+e);if(null===e)throw new Error("This browser's implementation of Object.create is a shim and doesn't support 'null' as the first argument.");if(void 0!==t)throw new Error("This browser's implementation of Object.create is a shim and doesn't support a second argument.");function o(){}return o.prototype=e,new o});

// Object.assign
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/assign#Polyfill
"function"!=typeof Object.assign&&Object.defineProperty(Object,"assign",{value:function(e,t){"use strict";if(null==e)throw new TypeError("Cannot convert undefined or null to object");for(var n=Object(e),r=1;r<arguments.length;r++){var o=arguments[r];if(null!=o)for(var c in o)Object.prototype.hasOwnProperty.call(o,c)&&(n[c]=o[c])}return n},writable:!0,configurable:!0});
