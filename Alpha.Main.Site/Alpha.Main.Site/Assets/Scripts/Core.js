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