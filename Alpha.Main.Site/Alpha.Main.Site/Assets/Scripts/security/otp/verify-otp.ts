/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtp {
    $otpForm: any;
    $txtOtp: any;
    $btnSubmit: any;
    $otpMessage: any;

    resources = {
        requiredFieldMessage: (<HTMLInputElement>document.getElementById('requiredFieldMessage')).value
    }

    constructor(public $) {
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();
    }

    cachedElements = () => {
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOTP");
        this.$btnSubmit = this.$("#btnSubmit");
        this.$otpMessage = this.$("#otp-message");
    }

    registerEvents = () => {
        this.$otpForm.submit(this.verifyOtpEvent);

        if (top.hasPopupOpen()) {
            top.setSecurityCodePopupTitle(this.$("#title_header").text());
            this.$("#container").addClass("has-popup");
            this.$("#title_header").remove();
        }
    }

    verifyOtpEvent = (e: any) => {
        e.preventDefault();

        var otp = this.$txtOtp.val();
        if (otp.length === 0) {
            alpha.showMessage(this.$otpMessage, new Result({ Code: 1, Message: this.resources.requiredFieldMessage }));
            this.$txtOtp.focus();
            return;
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/VerifyOtp",
            data: { otp: otp },
            dataType: "json",
            success: this.onVerifiedSuccess,
            complete: this.onComplete
        });
    }

    onVerifiedSuccess = (data) => {
        var result = new Result(data);

        if (result.isSuccessful) {
            securityHelper.afterVerifed();
            this.$txtOtp.attr("disbaled");
            this.$btnSubmit.attr("disbaled");
        }
        else {
            alpha.showMessage(this.$otpMessage, result);
            this.$txtOtp.focus();
        }
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var verifyOtp = new VerifyOtp($);

$(function () {
    verifyOtp.init();
});