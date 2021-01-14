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