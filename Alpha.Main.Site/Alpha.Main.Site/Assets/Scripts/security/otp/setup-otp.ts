/// <reference path="../../core.ts" />

class SetupOtp {
    $: any;
    bootbox: any;

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

    isAlreadySetup: boolean;
    previousStatus: boolean;

    resources = {
        removeOtpWarning: (<HTMLInputElement>document.getElementById('RemoveOtpWarning')).value,
        removeOtpSuccessMessage: (<HTMLInputElement>document.getElementById('RemoveOtpSuccessMessage')).value,
        otpIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('OtpIsInvalidOrExpired')).value,
        authenticatorAppVerified: (<HTMLInputElement>document.getElementById('AuthenticatorAppVerified')).value,
        thereIsNoChange: (<HTMLInputElement>document.getElementById('ThereIsNoChange')).value
    }

    constructor($: any, bootbox: any) {
        this.$ = $;
        this.bootbox = bootbox;
    }

    init = () => {
        this.cachedDomElements();
        this.registerEvents();

        this.isAlreadySetup = this.$cbkEnableOtp.data("is-already-setup");
        this.previousStatus = this.$cbkEnableOtp.data("is-checked");
    }

    cachedDomElements = () => {
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOtp");
        this.$registerPanel = this.$("#register-panel");
        this.$removeSecretKeyPanel = this.$("#remove-secret-key-panel");
        this.$btnRemoveOtp = this.$("#btn-remove-otp");
        this.$cbkEnableOtp = this.$("#cbkEnableOtp");
        this.$btnChangeStatus = this.$("#btn-change-status");
        this.$changeStatusMessage = this.$("#change-status-message");
        this.$removeOtpMessage = this.$("#remove-otp-message");
        this.$setupOtpMessage = this.$("#setup-otp-message");
    }

    registerEvents = () => {
        this.$otpForm.submit(this.setUpEvent);
        this.$btnRemoveOtp.click(this.removeOtpEvent);
        this.$btnChangeStatus.click(this.changeStatusEvent);
    }

    setUpEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/SetUpOtp",
            data: { otp: this.$txtOtp.val() },
            dataType: "json",
            success: this.onSetupSuccess,
            complete: this.onComplete
        });
    }

    onSetupSuccess = (data: any) => {
        var result = new Result(data);
        if (result.isSuccessful) {
            this.$registerPanel.hide();
            this.$removeSecretKeyPanel.show();
            this.$btnChangeStatus.removeAttr("disabled");
            this.$cbkEnableOtp.removeAttr("disabled");
        }
        else {
            alpha.showMessage(this.$setupOtpMessage, result);
        }
    }

    changeStatusEvent = (e: any) => {
        var newStatus = this.$cbkEnableOtp.is(":checked");
        if (newStatus === this.previousStatus) {
            var warning = new Result({ Code: Result.warningCode, Message: this.resources.thereIsNoChange });
            alpha.showMessage(this.$changeStatusMessage, warning);
            return;
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/ChangeOtpStatus",
            data: { isEnable: newStatus },
            dataType: "json",
            success: this.onChangeStatusSuccess,
            complete: this.onComplete
        });
    }

    onChangeStatusSuccess = (data: any) => {
        var result = new Result(data);
        alpha.showMessage(this.$changeStatusMessage, result);

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
        window.location.href = window.location.href;
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var setUpOtp = new SetupOtp($, bootbox);

$(function () {
    setUpOtp.init();
});