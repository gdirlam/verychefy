/*jshint asi:true, supernew:true */
var Chefy_Globals_Assert_ignore = false
var Chefy_Globals_Assert_WARN = false

var Chefy = Chefy || {};

Chefy.assert = function () {
   // debugger;
    this.AssertException = function () {
        var ExceptionBase = {}

        ExceptionBase.message = arguments[1]
        ExceptionBase.valid = arguments[0]
        ExceptionBase.args = Array.prototype.slice.call(arguments)

        ExceptionBase.toString = function () {
            return 'AssertException: ' + this.args.slice(1).join(' : ')
        }
        return ExceptionBase;
    }

    if ((!arguments[0]) && Chefy_Globals_Assert_ignore !== true) {
        if (!Chefy_Globals_Assert_WARN) {
            //throw this.AssertException.apply(this, arguments)
            var ex = this.AssertException.apply(this, arguments)
            console.error(ex, ex.message)

        } else {
            var ex = this.AssertException.apply(this, arguments)
            console.warn(ex.valid, ex.message)
        }
    } else {
        console.info('PASSED', arguments[1])
    }

}

Chefy.Assert = (function (base) {
    var _ignore = function () { return Chefy_Globals_Assert_ignore }

    base.isArray = function (val1, message) {
        if (!base.ignore) {
            return Chefy.assert(Array.isArray(val1), message || "Value is not an Array")
        }
    };

    base.isDate = function (val1, message) {
        if (!base.ignore) {
            return Chefy.assert(val1 instanceof Date && !isNaN(val1.getTime()), message || "Value is not a date")
        }
    };

    base.areEqual = function (val1, val2, message) {
        if (!base.ignore)
            return Chefy.assert(val1 === val2, message || "Values are not equal")
    };

    base.isInstanceOf = function (val, type, message) {
        if (!base.ignore)
            return Chefy.assert(val instanceof type, message || "Value is not an instance of " + type.totoString)
    };

    base.isNumber = function (val1, message) {
        if (!base.ignore)
            return Chefy.assert(typeof val1 === 'number', message || "Value is not a number")
    };

    base.isString = function (val1, message) {
        if (!base.ignore)
            return Chefy.assert(typeof val1 === 'string', message || "Value is not a string")
    };

    base.contains = function (val1, val2, message) {
        if (!base.ignore)
            return Chefy.assert(val1.indexOf(val2) >= 0, message || "The First String Does Not Contains The Second String")
    };
    try {
        //Set Global Assert Ignore
        Object.defineProperty(base, 'ignore', {
            get: function () { return Chefy_Globals_Assert_ignore }
        , set: function (value) { Chefy_Globals_Assert_ignore = value }
        });
    } catch (e) {

    }
    try {
        //Set Global Assert Warn Only
        Object.defineProperty(base, 'warn', {
            get: function () { return Chefy_Globals_Assert_WARN }
        , set: function (value) { Chefy_Globals_Assert_WARN = value }
        });
    } catch (e) {

    }
    base.init = function () {
        if (Chefy_Globals_Log)
            console.log('Assertion Lib Init Event Fired')
    }
    base.init();
    return base;

} (Chefy.Assert || {}));   
