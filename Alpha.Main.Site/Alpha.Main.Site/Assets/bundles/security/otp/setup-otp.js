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
/// <reference path="../../core.ts" />
var SetupOtp = /** @class */ (function () {
    function SetupOtp($, bootbox, clipboard) {
        var _this = this;
        this.resources = {
            removeOtpWarning: document.getElementById('RemoveOtpWarning').value,
            removeOtpSuccessMessage: document.getElementById('RemoveOtpSuccessMessage').value,
            otpIsInvalidOrExpired: document.getElementById('OtpIsInvalidOrExpired').value,
            authenticatorAppVerified: document.getElementById('AuthenticatorAppVerified').value,
            thereIsNoChange: document.getElementById('ThereIsNoChange').value,
            thisFieldRequired: document.getElementById('ThisFieldRequired').value
        };
        this.init = function () {
            _this.cachedDomElements();
            _this.registerEvents();
            _this.isAlreadySetup = _this.$cbkEnableOtp.data("is-already-setup");
            _this.previousStatus = _this.$cbkEnableOtp.data("is-checked");
        };
        this.cachedDomElements = function () {
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOtp").val("");
            _this.$registerPanel = _this.$("#register-panel");
            _this.$removeSecretKeyPanel = _this.$("#remove-secret-key-panel");
            _this.$btnRemoveOtp = _this.$("#btn-remove-otp");
            _this.$cbkEnableOtp = _this.$("#cbkEnableOtp");
            _this.$btnChangeStatus = _this.$("#btn-change-status");
            _this.$changeStatusMessage = _this.$("#change-status-message");
            _this.$removeOtpMessage = _this.$("#remove-otp-message");
            _this.$setupOtpMessage = _this.$("#setup-otp-message");
            _this.$btnSubmitOtp = _this.$("#btnSubmitOtp");
            _this.$usernameForm = _this.$("#username-form");
            _this.$usernameMessage = _this.$("#username-message");
            _this.$txtUsername = _this.$("#username");
            _this.$btnChangeUsername = _this.$("#btn-change-username");
            _this.$copyIcon = _this.$("#copy-icon");
        };
        this.registerEvents = function () {
            _this.$otpForm.submit(_this.setUpEvent);
            _this.$usernameForm.submit(_this.changeUsernameEvent);
            _this.$btnRemoveOtp.click(_this.removeOtpEvent);
            _this.$btnChangeStatus.click(_this.changeStatusEvent);
            _this.$cbkEnableOtp.change(_this.onChangedOtpStatusEvent);
            _this.$btnChangeUsername.click(_this.changeUsernameButtonEvent);
            var that = _this;
            if (window.document.queryCommandSupported
                && window.document.queryCommandSupported("copy")) {
                _this.clipboard
                    .on("success", function (e) {
                    that.$(e.trigger).tooltip("show");
                })
                    .on("error", function (e) {
                    that.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
            }
            else {
                _this.$copyIcon.remove();
            }
        };
        this.changeUsernameButtonEvent = function () {
            _this.$usernameForm.toggle(200);
        };
        this.changeUsernameEvent = function (e) {
            var username = _this.$txtUsername.val();
            if (username.length === 0) {
                alpha.showMessage(_this.$usernameMessage, new Result({ Code: 1, Message: _this.resources.thisFieldRequired }));
                _this.$txtUsername.focus();
                e.preventDefault();
            }
        };
        this.setUpEvent = function (e) {
            e.preventDefault();
            var otp = _this.$txtOtp.val();
            if (otp.length === 0) {
                _this.$txtOtp.focus();
                alpha.showMessage(_this.$setupOtpMessage, new Result({ Code: 1, Message: _this.resources.thisFieldRequired }));
                return;
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/SetUpOtp",
                data: { otp: otp },
                dataType: "json",
                success: _this.onSetupSuccess,
                complete: _this.onComplete
            });
        };
        this.onSetupSuccess = function (data) {
            var result = new Result(data);
            alpha.showMessage(_this.$setupOtpMessage, result);
            if (result.isSuccessful) {
                var that = _this;
                setTimeout(function () {
                    that.$registerPanel.hide(400);
                    that.$removeSecretKeyPanel.show(400);
                }, 3000);
                _this.$btnSubmitOtp.attr("disabled", "disabled");
                _this.$txtOtp.attr("disabled", "disabled");
                _this.$btnChangeStatus.removeAttr("disabled");
                _this.$cbkEnableOtp.removeAttr("disabled");
            }
        };
        this.onChangedOtpStatusEvent = function (e) {
            var newStatus = _this.$cbkEnableOtp.is(":checked");
            if (newStatus !== _this.previousStatus) {
                _this.$btnChangeStatus.removeAttr("disabled");
            }
            else {
                _this.$btnChangeStatus.attr("disabled", "disabled");
            }
        };
        this.changeStatusEvent = function (e) {
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/ChangeOtpStatus",
                data: { isEnable: _this.$cbkEnableOtp.is(":checked") },
                dataType: "json",
                success: _this.onChangeStatusSuccess,
                complete: _this.onComplete
            });
        };
        this.onChangeStatusSuccess = function (data) {
            var result = new Result(data);
            alpha.showMessage(_this.$changeStatusMessage, result);
            setTimeout(function () {
                parent.window.location.href = parent.window.location.href;
            }, 2000);
            _this.previousStatus = _this.$cbkEnableOtp.is(":checked");
        };
        this.removeOtpEvent = function (e) {
            var that = _this;
            _this.bootbox.confirm(_this.resources.removeOtpWarning, function (ok) {
                if (ok) {
                    that.$.blockUI();
                    that.$.ajax({
                        type: "POST",
                        url: alpha.urls.mainRootPath + "Otp/RemoveOtp",
                        dataType: "json",
                        success: that.onRemoveOtpSuccess,
                        complete: that.onComplete
                    });
                }
            });
        };
        this.onRemoveOtpSuccess = function (data) {
            var result = new Result(data);
            alpha.showMessage(_this.$removeOtpMessage, result);
            setTimeout(function () {
                parent.window.location.href = parent.window.location.href;
            }, 2000);
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.$ = $;
        this.bootbox = bootbox;
        this.clipboard = clipboard;
    }
    return SetupOtp;
}());
var setUpOtp = new SetupOtp($, bootbox, new Clipboard('.icon-content-copy'));
$(function () {
    setUpOtp.init();
});
//# sourceMappingURL=setup-otp.js.map