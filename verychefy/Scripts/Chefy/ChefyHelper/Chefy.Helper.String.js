/*jshint asi:true, supernew:true */
//in ie Prototype setting from core is not reaching here....
var Chefy_Globals_Prototype = Chefy_Globals_Prototype || true

var Chefy = Chefy || {};

Chefy.Helper = (function (base, Global) {
    base.ignore = Global.Chefy_Globals_Prototype; 
    return base
})((Chefy.Helper || {}), window)

Chefy.Helper.String = {
    format: function () {
        var txt = this;
        for (var i = 0; i < arguments.length; i++) {
            var exp = new RegExp('\\{' + (i) + '\\}', 'gm')
            txt = txt.replace(exp, arguments[i])
        }
        return txt
    }
    , inlineformat: function () {
        for (var i = 1; i < arguments.length; i++) {
            var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm')
            arguments[0] = arguments[0].replace(exp, arguments[i])
        }
        return arguments[0]
    }
   , write: function () {
       if (arguments.length === 0) {
           var txt = this
           document.body.innerHTML += '<br />' + txt + '<br />'
           //document.write( this + '<br />' )
           return ''
       }
       var txt = ''
       for (var i = 0; i < arguments.length; i++)
           txt += arguments[i] + '<br />'

       //document.write(txt)
       document.body.innerHTML += '<br />' + txt + '<br />'
       return ''
   }
   , init: function () {
       if (Chefy_Globals_Log)
           console.log('String Helper Init Event Fired')

       if (Chefy_Globals_Prototype) {
           if (!String.prototype.format)
               String.prototype.format = Chefy.Helper.String.format

           if (!String.format)
               String.format = Chefy.Helper.String.inlineformat

           if (!String.write)
               String.write = Chefy.Helper.String.write

           if (!String.prototype.write)
               String.prototype.write = Chefy.Helper.String.write
       }
   }

};
(function(){ Chefy.Helper.String.init() })()

        