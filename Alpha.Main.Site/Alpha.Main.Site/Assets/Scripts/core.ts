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
        },
        isIe: function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
            {
                return true;
            }

            return false;
        }
    },
    init: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
    }
};

$(function () {
    alpha.init();
});

