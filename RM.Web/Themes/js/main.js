var navi = {
    // variables
    aPages : [],
    aLinks : [],
    tween : {},
    oParent : null,
    oPages : null,
    bRunning : null,
    sTargPage : null,
    sCurPage : null,
    bHash : null,
    sOldH : null,
    bUpdateH : true,

    // initialization
    init : function (aParams) {
        this.oPages = document.getElementById(aParams.pages_id);
        this.oParent  = document.getElementById(aParams.parent_id);

        var aAEls = this.oParent.getElementsByTagName('a');
        var i = 0; var p = null;
        while (p = aAEls[i++]) {
            if (p.className && p.className.indexOf('go') >= 0) {
                var sHref = p.href.split('#')[1];
                var oDst = document.getElementById(sHref);
                if (oDst) {
                    // fill-in pages array
                    this.aPages[sHref] = {
                        oObj:  oDst,
                        iXPos: -oDst.offsetLeft,
                        iYPos: -oDst.offsetTop
                    };

                    // fill-in links array
                    this.aLinks.push({a: p, oObj: oDst});

                    p.onclick = function () {
                        navi.goto(this.href.split('#')[1], aParams.duration);
                        return false;
                    }
                }
            }
        }

        this.resize();

        if ('onhashchange' in window) {
            if (location.hash !== '' && location.hash !== '#') {
                this.sOldH = location.hash.substring(1);
                this.goto(this.sOldH, -1);
            } else
                this.goto('page1', -1);
            this.bHash = true;

            window.onhashchange = function() {
                if (location.hash.substring(1) !== navi.sOldH) {
                    navi.sOldH = location.hash.substring(1);
                    if (navi.sOldH == '') {
                        navi.bUpdateH = false;
                    }
                    navi.goto(navi.sOldH, aParams.duration);
                }
                return false;
            }
        }
    },

    // on resize
    resize : function () {
        var iCurW = this.oParent.offsetWidth; // current sizes
        var iCurH = this.oParent.offsetHeight;
        for (var i in this.aPages) { // for each page
            var oPage = this.aPages[i];
            var iNewX = Math.round(oPage.oObj.offsetLeft * iCurW / oPage.oObj.offsetWidth); // new sizes
            var iNewY = Math.round(oPage.oObj.offsetTop  * iCurH / oPage.oObj.offsetHeight);
            oPage.oObj.style.left   = iNewX + 'px';
            oPage.oObj.style.top    = iNewY + 'px';
            oPage.oObj.style.width  = iCurW + 'px';
            oPage.oObj.style.height = iCurH + 'px';
            oPage.iXPos = -iNewX;
            oPage.iYPos = -iNewY;

            if (this.sCurPage)
                this.goto(this.sCurPage, -1);
        }
    },

    goto : function (sHref, iDur) {
        this.tween.iStart = new Date() * 1;
        this.tween.iDur = iDur;
        this.tween.fromX = this.oPages.offsetLeft;
        this.tween.fromY = this.oPages.offsetTop;
        this.tween.iXPos   = this.aPages[sHref].iXPos - this.tween.fromX;
        this.tween.iYPos   = this.aPages[sHref].iYPos - this.tween.fromY;
        this.sTargPage = sHref;

        if (! this.bRunning)
            this.bRunning = window.setInterval(this.animate, 24);
    },

    animate : function () {
        var iCurTime = (new Date() * 1) - navi.tween.iStart;
        if (iCurTime < navi.tween.iDur) {
            var iCur = Math.cos(Math.PI * (iCurTime / navi.tween.iDur)) - 1;
            navi.oPages.style.left = Math.round(-navi.tween.iXPos * .5 * iCur + navi.tween.fromX) + 'px';
            navi.oPages.style.top  = Math.round(-navi.tween.iYPos * .5 * iCur + navi.tween.fromY) + 'px';
        } else {
            navi.oPages.style.left = Math.round(navi.tween.fromX + navi.tween.iXPos) + 'px';
            navi.oPages.style.top  = Math.round(navi.tween.fromY + navi.tween.iYPos) + 'px';

            window.clearInterval(navi.bRunning);
            navi.bRunning = false;
            navi.sCurPage = navi.sTargPage;

            var i = 0; var p = null;
            while (p = navi.aLinks[i++]) {
                if (p.oObj.id == navi.sCurPage) {
                    if (p.a.className.indexOf('visited') >= 0 ) {
                        p.a.className = p.a.className.replace('visited', 'active');
                    } else p.a.className += ' active';
                    p.visited = true;
                } else if (p.visited) {
                    p.a.className = p.a.className.replace('active', 'visited');
                }
            }

            if (navi.bHash) {
                if (navi.bUpdateH) {
                    navi.sOldH = navi.sCurPage;
                    window.location.hash = navi.sCurPage;
                }
                navi.bUpdateH = true;
            }
        }
    }
}

window.onload = function() { // page onload
    navi.init({parent_id: 'container', pages_id: 'pages', duration: 1000});
}
window.onkeydown = function(event){ // keyboard alerts
    switch (event.keyCode) {
        case 37: // Left key
            var iPage = navi.sCurPage.substring(4) * 1;
            iPage--;
            if (iPage < 1) {
                iPage = 1;
            }
            navi.goto('page' + iPage, 1000);
            break;
        case 39: // Right key
            var iPage = navi.sCurPage.substring(4) * 1;
            iPage++;
            if (iPage > 16) {
                iPage = 16;
            }
            navi.goto('page' + iPage, 1000);
            break;
    }
};