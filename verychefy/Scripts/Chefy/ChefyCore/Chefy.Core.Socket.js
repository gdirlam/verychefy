/*jshint asi:true, supernew:true */

var Chefy = Chefy || {};
Chefy.Core = Chefy.Core || {};

Chefy.Core.Socket = (function (base) {

    base.Remote = function (url, func) {
        base.Remote = (base.Remote || {})  //callback function here.
        base.Remote.url = url
        base.Remote.Response = {}
        base.Remote.Response.DataType = 'jsonp'
        base.Remote.Success = func //Chefy.Core.Extend( , { responseType: 'jsonp' } )
        var _script = document.createElement('script')
        _script.src = base.Remote.url + '?callback=Chefy.Core.Socket.Remote.Success'
        _script.type = 'text/javascript'
        document.body.appendChild(_script)
        return base.Remote
    }
    base.Xhr = function () {
        if (window.ActiveXObject)
            return new ActiveXObject('Microsoft.XMLHTTP')
        else if (window.XMLHttpRequest)
            return new XMLHttpRequest()
        return false
    };
    base.Domain = function (settings) {
        //debugger;
        if (typeof settings == 'object') {
            settings = Chefy.Core().Extend(base.Domain, settings);
        }
        var func = function (data) {
            console.log(data)
            return true;
        }
        if (Chefy.Core().isFunction(base.Domain.Func)) {
            func = base.Domain.Func
        }

        var Xhr = base.Xhr();
        var PostBody = PostBody || ''
        var Response = { Text: '', DataType: 'json', ContentType: 'text/plain', Data: null }
        Xhr.onreadystatechange = function () {
            if (Xhr.readyState == 4) {
                Response.Text = Xhr.responseText.toString()
                //debugger;
                Response.ContentType = Xhr.getResponseHeader('content-type') || Response.ContentType

                if (Response.ContentType.indexOf('text') > -1)
                    Response.DataType = 'text'
                if (Response.ContentType.indexOf('html') > -1)
                    Response.DataType = 'html'
                if (Response.ContentType.indexOf('xml') > -1)
                    Response.DataType = 'xml'
                if (Response.ContentType.indexOf('json') > -1)
                    Response.DataType = 'json'

                if (Response.DataType === 'json') {
                    Response.Data = (new Function("return " + Response.Text))()
                } else {
                    Response.Data = Response.Text
                }

                func(Response.Data)
            }
        }

        if (base.Domain.PostBody !== "") {
            //debugger;
            Xhr.open((base.Domain.Type || "POST"), base.Domain.Url, true)
            Xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest')
            Xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded')
            //request.setRequestHeader('Connection', 'close')
        } else {
            //debugger;
            Xhr.open((base.Domain.Type || "GET"), base.Domain.Url, true)
        }

        Xhr.send(null)

        return Response


    };

    //Chefy.Core().Loaded['Chefy.Core.Socket'] = base.Web
    return base;

})(Chefy.Core.Socket || {});   
