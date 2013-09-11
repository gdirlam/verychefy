/*jshint asi:true, supernew:true */

var Chefy = Chefy || {};
Chefy.Core = Chefy.Core || {};

Chefy.Core.EcmaCompatability = (function (base) {
    base.array = (base.array || {})
    base.array.forEach = function () {
        if (!Array.prototype.forEach) {
            Array.prototype.forEach = function (fn, scope) {
                for (var i = 0, len = this.length; i < len; ++i) {
                    fn.call(scope, this[i], i, this);
                }
            }
        }
    }
    base.array.map = function () {
        if (!Array.prototype.map) {
            Array.prototype.map = function (callback, thisArg) {
                var T, A, k;
                if (this == null) {
                    throw new TypeError(" this is null or not defined");
                }
                var O = Object(this);
                var len = O.length >>> 0;
                if ({}.toString.call(callback) != "[object Function]") {
                    throw new TypeError(callback + " is not a function");
                }
                if (thisArg) {
                    T = thisArg;
                }
                A = new Array(len);
                k = 0;
                while (k < len) {
                    var kValue, mappedValue;
                    if (k in O) {
                        kValue = O[k];
                        mappedValue = callback.call(T, kValue, k, O);
                        A[k] = mappedValue;
                    }
                    k++;
                }
                return A;
            };
        }
    }
    base.array.reduce = function () {
        if (!Array.prototype.reduce) {
            Array.prototype.reduce = function reduce(accumulator) {
                if (this === null || this === undefined) throw new TypeError("Object is null or undefined");
                var i = 0, l = this.length >> 0, curr;
                if (typeof accumulator !== "function")
                    throw new TypeError("First argument is not callable");
                if (arguments.length < 2) {
                    if (l === 0) throw new TypeError("Array length is 0 and no second argument");
                    curr = this[0];
                    i = 1;
                }
                else
                    curr = arguments[1];
                while (i < l) {
                    if (i in this) curr = accumulator.call(undefined, curr, this[i], i, this);
                    ++i;
                }
                return curr;
            };
        }
    }
    base.array.filter = function () {
        if (!Array.prototype.filter) {
            Array.prototype.filter = function (fun /*, thisp */) {
                "use strict";
                if (this == null)
                    throw new TypeError();
                var t = Object(this);
                var len = t.length >>> 0;
                if (typeof fun != "function")
                    throw new TypeError();
                var res = [];
                var thisp = arguments[1];
                for (var i = 0; i < len; i++) {
                    if (i in t) {
                        var val = t[i];
                        if (fun.call(thisp, val, i, t))
                            res.push(val);
                    }
                }
                return res;
            };
        }
    }
    base.array.isArray = function () {
        if (!Array.isArray) {
            Array.isArray = function (vArg) {
                return Object.prototype.toString.call(vArg) === "[object Array]";
            };
        }
    }


    base.array.forEach()
    base.array.map()
    base.array.reduce()
    base.array.filter()
    base.array.isArray()

    return base
} (Chefy.Core.EcmaCompatability || {}));