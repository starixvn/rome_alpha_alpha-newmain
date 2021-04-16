/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtpDefferenceDevice {
    $btnBack: any;
    $otpForm: any;
    $txtOtp: any;
    lockedAccountCode: number = 200;

    resources = {
        requiredFieldMessage: (<HTMLInputElement>document.getElementById('requiredFieldMessage')).value
    }

    constructor(public $) {
    }

    init = () => {
        securityHelper.initLanguage();

        this.cachedElements();
        this.registerEvents();

        const actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
        this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
    }

    cachedElements = () => {
        this.$btnBack = this.$("#btn-back");
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOTP");
    }

    registerEvents = () => {
        this.$btnBack.click(alpha.core.signOut);
        this.$otpForm.submit(this.verifyOtpEvent);
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
            securityHelper.gotoHomePage();
        }
        else {
            this.$txtOtp.addClass("error").next().text(result.message);

            if (result.code === this.lockedAccountCode) {
                alert(result.message);
                alpha.core.signOut();
            }
        }
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var verifyOtpDefferenceDevice = new VerifyOtpDefferenceDevice($);

$(function () {
    verifyOtpDefferenceDevice.init();
});