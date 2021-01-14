var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "not found",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';
            if (isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                index.location = signOutUrl;
            }
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
        IsAndroid: function () {
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
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
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
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                alert('Oops... you are here because of a system error, please notify the administrators.');
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
"use strict";
var Nickname = /** @class */ (function () {
    function Nickname($) {
        this.$ = $;
    }
    Nickname.prototype.init = function () {
        this.cacheElements();
        this.bindEvents();
    };
    Nickname.prototype.cacheElements = function () {
        this.nicknameRule = this.$("#nicknameRule").val();
        this.$nickname = this.$("#nickname");
        this.$nicknameLabel = this.$("#nickname-error");
        this.$loginByUsernameOrNickname = this.$("#loginByUsernameOrNickname");
        this.$loginByNickname = this.$("#loginByNickname");
        this.$buttonSubmit = this.$("#buttonSubmit");
        this.mainRootPath = this.$("#mainRootPath").val();
    };
    Nickname.prototype.bindEvents = function () {
        var self = this;
        this.$nickname
            .focus()
            .keyup(function () {
            var nickname = self.$.trim(self.$nickname.val());
            if (nickname.length > 0) {
                var validNickname = self.isValidNickname(nickname);
                if (validNickname) {
                    self.showValidIcon();
                    self.removeErrorMessage();
                }
                else {
                    self.showErrorIcon();
                }
            }
        });
        this.$loginByUsernameOrNickname.change(function () {
            self.$buttonSubmit.prop("disabled", true);
        });
        this.$loginByNickname.change(function () {
            self.$buttonSubmit.prop("disabled", false);
        });
        this.$("#frmNickname").submit(function () {
            self.$.blockUI();
            var nickname = self.$.trim(self.$nickname.val());
            var validNickname = self.isValidNickname(nickname);
            if (!validNickname) {
                self.$.unblockUI();
                self.showErrorIcon();
                self.showErrorMessage(self.nicknameRule);
                self.$nickname.focus();
            }
            return validNickname;
        });
        this.$("#buttonBack").click(alpha.core.signOut);
        this.$(".changelanguage-link").click(function () {
            self.changeLanguage(self.$(this).data("value"));
        });
    };
    Nickname.prototype.isValidNickname = function (nickname) {
        var nicknameRuleRegExp = new RegExp(/^[a-zA-Z0-9]{5,20}$/i);
        return nicknameRuleRegExp.test(nickname);
    };
    Nickname.prototype.showValidIcon = function () {
        this.$nickname.removeClass("error").addClass("valid");
    };
    Nickname.prototype.showErrorIcon = function () {
        this.$nickname.removeClass("valid").addClass("error");
    };
    Nickname.prototype.showErrorMessage = function () {
        this.$nicknameLabel.addClass("error").html(this.nicknameRule);
    };
    Nickname.prototype.removeErrorMessage = function () {
        this.$nicknameLabel.empty().removeClass("error");
    };
    Nickname.prototype.changeLanguage = function (lang) {
        this.$.ajax({
            type: "POST",
            url: this.mainRootPath + "Language/Change",
            data: { languageCode: lang },
            success: function (result) {
                if (result) {
                    window.location.reload(true);
                }
            }
        });
    };
    return Nickname;
}());
$(function () {
    new Nickname($).init();
});
//# sourceMappingURL=create-nickname.js.map