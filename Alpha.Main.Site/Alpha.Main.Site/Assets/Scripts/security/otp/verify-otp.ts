/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtp {
    $otpForm: any;
    $txtOtp: any;

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
            this.$txtOtp.addClass("error").next().text(this.resources.requiredFieldMessage);
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
        }
        else {
            this.$txtOtp.addClass("error").next().text(result.message);
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