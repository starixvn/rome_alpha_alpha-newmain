declare var isMobile: any;
declare var bootbox: any;

var alpha = {
    urls:
        {
            oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('oldMainRootPath')).value
                : "/ex-main/",
            mainRootPath: top.document.getElementById('mainRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('mainRootPath')).value
                : "not found",
            accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('accountInfoRootPath')).value
                : "not found",
            memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('memberInfoRootPath')).value
                : "not found",
            howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('howToUseRootPath')).value
                : "not found",
            exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('exTotalBetsForecastRootPath')).value
                : "not found",
            messageRootPath: top.document.getElementById('messageRootPath') != null
                ? (<HTMLInputElement>top.document.getElementById('messageRootPath')).value
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
                : "not found",
            isShowIntroduction: document.getElementById('isShowIntroduction') != null
                ? ((<HTMLInputElement>document.getElementById('isShowIntroduction')).value.toLowerCase() === "true")
                : "not found",
            isEnableOtp: document.getElementById('isEnabledOtp') != null
                ? ((<HTMLInputElement>document.getElementById('isEnabledOtp')).value.toLowerCase() === "true")
                : false
        },
    core:
        {
            convertReponseToJson: function (reponse) {
                return eval("(" + reponse + ")");
            },

            signOut: function (errMsg = null) {
                errMsg = errMsg || $('#SessionExpired').val();
                var index: any = window;
                while (index != index.parent) {
                    index = index.parent;
                }

                var isInternal = index.$("#isInternal").val();
                isInternal = isInternal || "0";
                var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx?r=' + errMsg;

                if (isInternal && isInternal.toString() !== "0") {
                    $.ajax(signOutUrl, {
                        success: function () {
                            index.close();
                        }
                    });
                }
                else {
                    try {
                        localStorage.removeItem(indexManager.currentUrlCookieName);
                    }
                    finally {
                        index.location = signOutUrl;
                    }
                }
                sessionStorage.clear();
            },
            signOut2: function () {
                var index: any = window;
                while (index != index.parent) {
                    index = index.parent;
                }

                var isInternal = index.$("#isInternal").val();
                isInternal = isInternal || "0";
                var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut2.ashx';

                if (isInternal && isInternal.toString() !== "0") {
                    $.ajax(signOutUrl, {
                        success: function () {
                            index.close();
                        }
                    });
                }
                else {
                    try {
                        indexManager.removeCookies();
                    }
                    finally {
                        index.location = signOutUrl;
                    }
                }
                sessionStorage.clear();
            },
            isIe: function () {
                return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
            },
            IsIe8OrLower: function () {
                var userAgent = window.navigator.userAgent.toLowerCase();
                return (userAgent.indexOf('msie') != -1)
                    ? (parseInt(userAgent.split('msie')[1]) < 9)
                    : false;
            },

            IsAndroid: () => {
                return /Android/i.test(navigator.userAgent);
            }
        },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }

        var currentWindowWidth = window.screen.width;

        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        } else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        } else {
            alpha.isScreenGreater1024 = true;
        }

        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: (key: string) => {
            return store.enabled ? store.get(key) : $.cookie(key);
        },

        set: (key: string, value: any) => {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },

        check: (key: string): boolean => {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        },
    },
    showMessage: function ($target: any, result: Result) {
        $target.empty();

        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }

        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }

        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },

    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};

$(() => {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", (<HTMLInputElement>antiXSRFToken[0]).value);
            }
        },
        error: function (xhr) {
            if (xhr.status === 401
                || xhr.status === 403)
            {
                alpha.core.signOut();
            }
        }
    });
});

class Result {
    static warningCode: number = -1000;

    code: number;
    message: string;
    isSuccessful: boolean;
    isFailed: boolean;
    isWarning: boolean;

    constructor(data?: any) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
}