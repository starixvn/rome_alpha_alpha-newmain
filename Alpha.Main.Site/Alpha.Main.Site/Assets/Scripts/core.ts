declare var isMobile: any;

var alpha = {
    urls:
    {
        oldMainRootPath: document.getElementById('oldMainRootPath') != null
            ? (<HTMLInputElement>document.getElementById('oldMainRootPath')).value
            : "not found",
        mainRootPath: document.getElementById('mainRootPath') != null
            ? (<HTMLInputElement>document.getElementById('mainRootPath')).value
            : "not found",
        howToUseRootPath: document.getElementById('howToUseRootPath') != null
            ? (<HTMLInputElement>document.getElementById('howToUseRootPath')).value
            : "not found"
    },
    user:
    {
        onUserId: document.getElementById('onUserId') != null
            ? (<HTMLInputElement>document.getElementById('onUserId')).value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? (<HTMLInputElement>document.getElementById('passwordHash')).value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? (<HTMLInputElement>document.getElementById('subPageAccess')).value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? (<HTMLInputElement>document.getElementById('userLevel')).value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? ((<HTMLInputElement>document.getElementById('isSubAccount')).value.toLowerCase() === "true")
            : "not found"
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
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }

        var currentWindowWidth = $(window).width();

        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }

        alpha.isMobile = isMobile.any;
    }
};
