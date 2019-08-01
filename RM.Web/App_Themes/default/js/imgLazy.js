"use strict"

define({
    F_lzy: function () {
        var lazyAll = document.getElementsByTagName('img');
        var lazySrc = [];
        for (var i = 0; i < lazyAll.length; i++) {
            if (!lazyAll[i].getAttribute('lazy-src')) continue;
            lazySrc.push(lazyAll[i]);
        }

        var getoffset = function (ele) {
            var top = ele.offsetTop;
            var paEle = ele.offsetParent;   //当前ele的父元素
            while (paEle != null && paEle.offsetTop) {
                top += paEle.offsetTop;
                paEle = paEle.offsetParent;
            };
            return top;
        }

        var lazyScroll = function () {
            var lazyPos = document.documentElement.scrollTop || document.body.scrollTop;
            var winH = window.innerHeight;
            for (var i = 0; i < lazySrc.length; i++) {
                var lazyTop = getoffset(lazySrc[i]);
                if (lazyTop <= lazyPos + winH && !lazySrc[i].getAttribute('onerror')) {
                    var errSrc = 'this.src="' + lazySrc[i].getAttribute('src') + '"';
                    var newSrc = lazySrc[i].getAttribute('lazy-src');
                    lazySrc[i].setAttribute('onerror', errSrc);
                    lazySrc[i].setAttribute('src', newSrc);
                    lazySrc[i].removeAttribute('lazy-src');
                }
            }
        };
        lazyScroll();      //初始化
        window.addEventListener('scroll', lazyScroll);
    }
})
