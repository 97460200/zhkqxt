(function () {
    var start = navigator.userAgent.indexOf('MSIE') + 4;
    var end = navigator.userAgent.indexOf(';', start);
    var version = start > 3 ? Number(navigator.userAgent.substring(start, end)) : 1 / 0;

    if (version > 8 && navigator.userAgent.indexOf('Trident') != -1) {
        (function (arr) {
            for (var i = 0; i < arr.length; i++) {
                var tap = document.createElement('link');
                tap.setAttribute('href', '/SysSetBase/css/' + arr[i]);
                tap.setAttribute('rel', 'stylesheet');
                tap.setAttribute('type', 'text/css');
                document.getElementsByTagName('head')[0].appendChild(tap)
            };
        })(['IE.css'])
    }
    start = end = version = $body = null;
})()