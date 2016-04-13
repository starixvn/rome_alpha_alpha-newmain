var alpha = alpha || {};

alpha.urls = {
    oldMainRootPath: document.getElementById('oldMainRootPath').value,
    mainRootPath: document.getElementById('mainRootPath').value,
    howToUseRootPath: document.getElementById('howToUseRootPath').value
};

alpha.user =
    {
        onUserId: document.getElementById('onUserId').value,
        passwordHash: document.getElementById('passwordHash').value,
        subPageAccess: document.getElementById('subPageAccess').value
    };

alpha.core = {
    convertReponseToJson: function (reponse) {
        return eval("(" + reponse + ")");
    },

    signOut: function () {
        var index = window;
        while (index != index.parent) {
            index = index.parent;
        }

        index.location = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';

        if (index.location.href.indexOf('https://imb.') == 0 ||
            index.location.href.indexOf('https://int.') == 0) {
            index.close();
        }
    }
};

//// Set value for old main => every share variables should be listed here
top.ph = alpha.user.passwordHash;
top.isNewSite = true;
top.errCodes = [];
top.oldMainRootPath = alpha.urls.oldMainRootPath;
top.mainRootPath = alpha.urls.mainRootPath;

top.AGE = top.AGE || {};
top.AGE.PERMISSIONS = alpha.user.subPageAccess;

//// Functions for Site-old only, do not use these
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
        }

        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }

    return null;
}

function setCookie(name, value, days, domain) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/;" + ((typeof domain != 'undefined') ? ("domain=" + domain) : String.Empty);
}

parent.age = parent.age || {};
window.age.OpenPopUp = function (url, title, width, height, position) {
    title = title || '';
    var popH = height || 360;
    var popW = width || 480;
    var left = position ? position.left : 200;
    var top = position ? position.top : 200;
    ageWnd.open(url, title, left, top, popW, popH);
}

window.age.ClosePopUp = function () {
    ageWnd.close();
}


