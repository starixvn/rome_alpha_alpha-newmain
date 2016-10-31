/// <reference path="../../core.ts" />
declare var Clipboard: any;

class SetupOtp {
    $: any;
    bootbox: any;
    clipboard: any;

    $otpForm: any;
    $txtOtp: any;
    $registerPanel: any;
    $removeSecretKeyPanel: any;
    $btnRemoveOtp: any;
    $cbkEnableOtp: any;
    $btnChangeStatus: any;
    $changeStatusMessage: any;
    $removeOtpMessage: any;
    $setupOtpMessage: any;
    $btnSubmitOtp: any;
    $usernameForm: any;
    $usernameMessage: any;
    $txtUsername: any;

    isAlreadySetup: boolean;
    previousStatus: boolean;

    resources = {
        removeOtpWarning: (<HTMLInputElement>document.getElementById('RemoveOtpWarning')).value,
        removeOtpSuccessMessage: (<HTMLInputElement>document.getElementById('RemoveOtpSuccessMessage')).value,
        otpIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('OtpIsInvalidOrExpired')).value,
        authenticatorAppVerified: (<HTMLInputElement>document.getElementById('AuthenticatorAppVerified')).value,
        thereIsNoChange: (<HTMLInputElement>document.getElementById('ThereIsNoChange')).value,
        thisFieldRequired: (<HTMLInputElement>document.getElementById('ThisFieldRequired')).value
    }

    constructor($: any, bootbox: any, clipboard: any) {
        this.$ = $;
        this.bootbox = bootbox;
        this.clipboard = clipboard;
    }

    init = () => {
        this.cachedDomElements();
        this.registerEvents();

        this.isAlreadySetup = this.$cbkEnableOtp.data("is-already-setup");
        this.previousStatus = this.$cbkEnableOtp.data("is-checked");
    }

    cachedDomElements = () => {
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOtp").val("");
        this.$registerPanel = this.$("#register-panel");
        this.$removeSecretKeyPanel = this.$("#remove-secret-key-panel");
        this.$btnRemoveOtp = this.$("#btn-remove-otp");
        this.$cbkEnableOtp = this.$("#cbkEnableOtp");
        this.$btnChangeStatus = this.$("#btn-change-status");
        this.$changeStatusMessage = this.$("#change-status-message");
        this.$removeOtpMessage = this.$("#remove-otp-message");
        this.$setupOtpMessage = this.$("#setup-otp-message");
        this.$btnSubmitOtp = this.$("#btnSubmitOtp");
        this.$usernameForm = this.$("#username-form");
        this.$usernameMessage = this.$("#username-message");
        this.$txtUsername = this.$("#username");
    }

    registerEvents = () => {
        this.$otpForm.submit(this.setUpEvent);
        this.$usernameForm.submit(this.changeUsernameEvent);
        this.$btnRemoveOtp.click(this.removeOtpEvent);
        this.$btnChangeStatus.click(this.changeStatusEvent);
        this.$cbkEnableOtp.change(this.onChangedOtpStatusEvent);

        var that = this;
        this.clipboard
            .on("success", function (e) {
                that.$(e.trigger).attr("title", "Copied!").tooltip("show");
            })
            .on("error", function (e) {
            window.console.error("Action:", e.action);
            window.console.error("Trigger:", e.trigger);
        });
    }

    changeUsernameEvent = (e: any) => {
        var username = this.$txtUsername.val();
        if (username.length === 0) {
            alpha.showMessage(this.$usernameMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
            this.$txtUsername.focus();
            e.preventDefault();
        }
    }

    setUpEvent = (e: any) => {
        e.preventDefault();

        var otp = this.$txtOtp.val();
        if (otp.length === 0) {
            this.$txtOtp.focus();
            alpha.showMessage(this.$setupOtpMessage, new Result({ Code: 1, Message: this.resources.thisFieldRequired }));
            return;
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/SetUpOtp",
            data: { otp: otp },
            dataType: "json",
            success: this.onSetupSuccess,
            complete: this.onComplete
        });
    }

    onSetupSuccess = (data: any) => {
        var result = new Result(data);
        alpha.showMessage(this.$setupOtpMessage, result);

        if (result.isSuccessful) {
            var that = this;
            setTimeout(() => {
                that.$registerPanel.hide(400);
                that.$removeSecretKeyPanel.show(400);
            }, 3000);

            this.$btnSubmitOtp.attr("disabled", "disabled");
            this.$txtOtp.attr("disabled", "disabled");
            this.$btnChangeStatus.removeAttr("disabled");
            this.$cbkEnableOtp.removeAttr("disabled");
        }
    }

    onChangedOtpStatusEvent = (e: any) => {
        var newStatus = this.$cbkEnableOtp.is(":checked");
        if (newStatus !== this.previousStatus) {
            this.$btnChangeStatus.removeAttr("disabled");
        }
        else {
            this.$btnChangeStatus.attr("disabled", "disabled");
        }
    }

    changeStatusEvent = (e: any) => {
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/ChangeOtpStatus",
            data: { isEnable: this.$cbkEnableOtp.is(":checked") },
            dataType: "json",
            success: this.onChangeStatusSuccess,
            complete: this.onComplete
        });
    }

    onChangeStatusSuccess = (data: any) => {
        var result = new Result(data);
        alpha.showMessage(this.$changeStatusMessage, result);

        setTimeout(() => {
            parent.window.location.href = parent.window.location.href;
        }, 2000);

        this.previousStatus = this.$cbkEnableOtp.is(":checked");
    }

    removeOtpEvent = (e: any) => {
        var that = this;
        this.bootbox.confirm(this.resources.removeOtpWarning, function (ok) {
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
    }

    onRemoveOtpSuccess = (data: any) => {
        var result = new Result(data);
        alpha.showMessage(this.$removeOtpMessage, result);
        setTimeout(() => {
            window.location.href = window.location.href;
        }, 2000);
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var setUpOtp = new SetupOtp($, bootbox, new Clipboard('.icon-content-copy'));

$(function () {
    setUpOtp.init();
});