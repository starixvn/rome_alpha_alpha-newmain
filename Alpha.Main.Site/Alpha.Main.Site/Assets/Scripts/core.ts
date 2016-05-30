var alpha = {
    urls:
    {
        oldMainRootPath: (<HTMLInputElement>document.getElementById('oldMainRootPath')).value,
        mainRootPath: (<HTMLInputElement>document.getElementById('mainRootPath')).value,
        howToUseRootPath: (<HTMLInputElement>document.getElementById('howToUseRootPath')).value
    },
    user:
    {
        onUserId: (<HTMLInputElement>document.getElementById('onUserId')).value,
        passwordHash: (<HTMLInputElement>document.getElementById('passwordHash')).value,
        subPageAccess: (<HTMLInputElement>document.getElementById('subPageAccess')).value
    },
    core:
    {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },

        signOut: function () {
            var index: Window = window;
            while (index != index.parent) {
                index = index.parent;
            }

            index.location.href = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';

            if (index.location.href.indexOf('https://imb.') == 0 ||
                index.location.href.indexOf('https://int.') == 0) {
                index.close();
            }
        }
    }
};


var tzdragg = function () {
    return {
        getElement: function (el) {
            return document.getElementById(el);
        },
        move: function (divid, xpos, ypos) {
            var a = tzdragg.getElement(divid);
            tzdragg.getElement(divid).style.left = xpos + 'px';
            tzdragg.getElement(divid).style.top = ypos + 'px';
        },
        startMoving: function (evt) {
            evt = evt || window.event;
            var posX = evt.clientX,
                posY = evt.clientY,
                a = tzdragg.getElement('elem'),
                divTop: any = a.style.top,
                divLeft: any = a.style.left;
            divTop = divTop.replace('px', '');
            divLeft = divLeft.replace('px', '');
            var diffX = posX - divLeft,
                diffY = posY - divTop;
            document.onmousemove = function (evt) {
                evt = evt || <MouseEvent>window.event;
                var posX = evt.clientX,
                    posY = evt.clientY,
                    aX = posX - diffX,
                    aY = posY - diffY;
                tzdragg.move('elem', aX, aY);
            }
        },
        stopMoving: function () {
            var a = document.createElement('script');
            document.onmousemove = function () { }
        }
    }
} ();