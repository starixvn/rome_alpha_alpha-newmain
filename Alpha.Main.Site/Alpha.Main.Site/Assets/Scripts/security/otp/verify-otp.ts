/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtp {
    $otpForm: any;
    $txtOtp: any;
    $btnSubmit: any;
    $otpMessage: any;
    lockedAccountCode: number = 200;

    resources = {
        requiredFieldMessage: (<HTMLInputElement>document.getElementById('requiredFieldMessage')).value
    }

    constructor(public $) {
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();

        const actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
        this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
    }

    cachedElements = () => {
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOTP").val("");
        this.$btnSubmit = this.$("#btnSubmit");
        this.$otpMessage = this.$("#otp-message");
    }

    registerEvents = () => {
        this.$otpForm.submit(this.verifyOtpEvent);

        if (top.hasPopupOpen()) {
            top.setSecurityCodePopupTitle(this.$("#title_header").text());
            this.$otpForm.addClass("has-popup");
            this.$("#title_header").remove();
        }
    }

    verifyOtpEvent = (e: any) => {
        e.preventDefault();

        var otp = this.$txtOtp.val();
        if (otp.length === 0) {
            this.$(this.$otpMessage).removeClass('hidden');
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
            var reportDomain = (<HTMLInputElement>top.document.getElementById('reportDomain')).value;
            if (reportDomain != '') {
                (<HTMLIFrameElement>top.document.getElementById('reportFrame')).src = reportDomain + '/Auth/Otp';
            }

            setTimeout(() => {
                securityHelper.afterVerifed();
                this.$txtOtp.attr("disabled", "disabled");
                this.$btnSubmit.attr("disabled", "disabled").remove();
            }, 2000);
        }
        else {
            this.$(this.$otpMessage).removeClass('hidden');
            alpha.showMessage(this.$otpMessage, result);
            this.$txtOtp.focus();

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

var verifyOtp = new VerifyOtp($);

$(function () {
    verifyOtp.init();
});