/*jshint asi:true, supernew:true */
/*
ie is not firing init on load. has some issue with console.log...
which.target.asset.Name is undefined.
*/
function chefy() {
    var chefy_base = {
        Global: window
        , onReady: function (fn) {
            if (window.Chefy_Globals_Log)
                console.log('Document Ready Fired')

            var timer, ready = false, setup = false, stack = [];

            function stateChange(e) {
                e = e || window.event;
                if (window.Chefy_Globals_Loader_Complete) {
                    if (e && e.type && /DOMContentLoaded|load/.test(e.type)) {
                        Ready()
                    } else if (window.document.readyState) {
                        if (/loaded|complete/.test(window.document.readyState)) {
                            Ready()
                        } else if (window.document.documentElement.doScroll) {
                            try {
                                ready || window.document.documentElement.doScroll('left')
                            } catch (e) {
                                return
                            }
                            Ready() //If no error was thrown, the DOM must be ready
                        }
                    }
                }
            };

            function Ready() {
                if (!ready) {

                    ready = true;

                    // Call the stack of onload functions in given context or window object
                    for (var i = 0, len = stack.length; i < len; i++) {
                        stack[i][0].call(stack[i][1]);
                    }
                    // Clean up after the DOM is ready
                    if (window.document.removeEventListener)
                        window.document.removeEventListener("DOMContentLoaded", stateChange, false)
                    clearInterval(timer);
                    window.document.onreadystatechange = window.onload = timer = null //destroy all
                }
            };
            /*if( ready ){ //I am not a big fan of this block. . . 
            //If the DOM is ready, call the function and return
            fn.call( window );
            return;
            }*/
            if (!setup) {
                //debugger; 
                setup = true
                if (window.document.addEventListener)
                    window.document.addEventListener("DOMContentLoaded", stateChange, false)
                timer = setInterval(stateChange, 5)
                window.document.onreadystatechange = window.onload = stateChange
            }
            stack.push([fn]);
        }

        , Core: function () {
            //debugger; 
            var Global = (Global || window)
            var chefy_core_base = { Settings: {} }

            chefy_core_base.Extend = function (destination, source) {
                for (var member in source) {
                    //debugger; 
                    destination[member] = source[member]
                }
                return destination
            };
            /*Needs to allow setting of defaulted values here.*/
            Global.Chefy_Globals_Prototype = true;
            Global.Chefy_Globals_Site = '/'
            chefy_core_base.init = function () {
                chefy_core_base.Extend(chefy_core_base.Settings, Array.prototype.slice.call(arguments)[0])
                for (var member in chefy_core_base.Settings) {
                    window['Chefy_Globals_' + member] = chefy_core_base.Settings[member]
                }
            };
            chefy_core_base.init(Array.prototype.slice.call(arguments)[0])

            chefy_core_base.Bind = function (caller, object) {
                return function () {
                    return caller.apply(object, [object])
                };
            };
            chefy_core_base.isFunction = function (object) {
                var getType = {};
                return object && getType.toString.call(object) === '[object Function]';

            }

            chefy_core_base.Loaded = [];
            chefy_core_base.Loader = function () {
                var chefy_core_loader_base = {}
                var Global = window //need better way to reference the parent
                //debugger; 

                chefy_core_loader_base.Asset = function (name, location) {
                    var base = {
                        Name: name
                        , Location: location
                        , Url: function () {
                            //debugger; 
                            var uri = Global.Chefy_Globals_Site + location;
                            //only add .js if the file does not end with / (a route)
                            if (/\//.test(location)) {
                                return uri
                            }
                            return uri + ((!/(\.js)$/.test(location)) ? '.js' : '')
                        }
                    };
                    return base
                }
                Global.Chefy_Globals_Assets = [];

                Global.Chefy_Globals_Assets['Chefy.Assert'] = new chefy_core_loader_base.Asset('Chefy.Assert', 'Scripts/Chefy/ChefyAssert/Chefy.Assert.js')

                Global.Chefy_Globals_Assets['Chefy.Core'] = new chefy_core_loader_base.Asset('Chefy.Core', 'Scripts/Chefy/ChefyCore/Chefy.Core.js')
                Global.Chefy_Globals_Assets['Chefy.Core.EcmaCompatibility'] = new chefy_core_loader_base.Asset('Chefy.Core.EcmaCompatibility', 'Scripts/Chefy/ChefyCore/Chefy.Core.EcmaCompatibility.js')

                Global.Chefy_Globals_Assets['Chefy.Core.Socket'] = new chefy_core_loader_base.Asset('Chefy.Core.Socket', 'Scripts/Chefy/ChefyCore/Chefy.Core.Socket.js')
                Global.Chefy_Globals_Assets['Socket'] = Global.Chefy_Globals_Assets['Chefy.Core.Socket']
                Global.Chefy_Globals_Assets['Chefy.Helper.String'] = new chefy_core_loader_base.Asset('Chefy.Helper.String', 'Scripts/Chefy/ChefyHelper/Chefy.Helper.String.js')

                Global.Chefy_Globals_Assets['Chefy.Models.Products.Types'] = new chefy_core_loader_base.Asset('Chefy.Models.Products.Types', 'products/types/viewmodel/')

                Global.Chefy_Globals_Loader_Complete = false;
                //debugger; 



                chefy_core_loader_base.Queue = []

                chefy_core_loader_base.enqueue = function (asset) {
                    chefy_core_loader_base.Queue[chefy_core_loader_base.Queue.length] = asset
                    //Chefy.Core.Loaded[ Chefy.Core.Loaded.length ] = asset
                }
                chefy_core_loader_base.Load = function () {
                    // debugger;
                    if (Global.Chefy_Globals_Log)
                        console.log('Script Loader Loading')
                    do {
                        var asset = chefy_core_loader_base.Queue[0]
                        chefy_core_loader_base.LoadScript(asset)
                        chefy_core_loader_base.Queue.splice(0, 1)
                        //debugger; 
                        if (Chefy_Globals_Log)
                            console.log('Script Loader Loaded', asset)

                    } while (chefy_core_loader_base.Queue.length > 0)

                }
                chefy_core_loader_base.Use = function (library) {
                    chefy_core_loader_base.Use.Success = function (which) {
                        //debugger; 

                        if (Global.Chefy_Globals_Log && which) {
                            console.log('Script Loader Loaded Callback Fired', which.target.asset)
                        } else {
                            console.log('Script Loader Loaded Callback Fired But we do not know which one it was')
                        }
                        /*
                        try {
                        //debugger; 
                            
                        var Fn = Function, ret = new Fn(which.target.asset.Name + '.init()')()
                        } catch (e) {
                        if (Global.Chefy_Globals_Log)
                        console.log('Script Loader Library, init function unavailable', which.target.asset)
                        }
                        */
                        if (chefy_core_loader_base.Queue.length === 0) {
                            Global.Chefy_Globals_Loader_Complete = true
                            if (Global.Chefy_Globals_Log)
                                console.log('Script Loader Complete')
                        }
                    }
                    // debugger; 
                    chefy_core_loader_base.enqueue(Global.Chefy_Globals_Assets[library])

                    return chefy_core_loader_base
                };
                chefy_core_loader_base.LoadScript = function (asset) {
                    //    debugger; 

                    var script = document.createElement('script')
                    ieLoadBugFix = function (scriptElement, callback) {
                        if (scriptElement.readyState == 'loaded' || scriptElement.readyState == 'complete') {
                            scriptElement.asset = asset//this might introduce a bug here, with asset passing in.
                            callback();
                        } else {
                            setTimeout(function () { ieLoadBugFix(scriptElement, callback); }, 100);
                        }
                    }
                    script.src = asset.Url()
                    //script.onload = Chefy.Core.Loader.Requires.Success //does not work in ie 7

                    if (typeof script.addEventListener !== "undefined") {
                        // debugger;
                        script.asset = asset
                        //debugger; 
                        script.addEventListener("load", chefy_core_loader_base.Use.Success, false)
                    } else {
                        script.onreadystatechange = function () {
                            script.onreadystatechange = null;
                            ieLoadBugFix(script, chefy_core_loader_base.Use.Success);
                        }
                    }

                    script.type = 'text/javascript'
                    //debugger;
                    //console.log(script )
                    document.getElementsByTagName('head')[0].appendChild(script)

                    if (Chefy_Globals_Log)
                        console.log('Script Loader Appended Script to Document')
                };
                //chefy_core_loader_base.Use = function(){} 

                return chefy_core_loader_base;
            }

            return chefy_core_base
        }
    }
    return chefy_base
}

/* Establish Chefy Namespace based on chefy class functionality */
var Chefy = new chefy()
var Chefit = Chefy.onReady