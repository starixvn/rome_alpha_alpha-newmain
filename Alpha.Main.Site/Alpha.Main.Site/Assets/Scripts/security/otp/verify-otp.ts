/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtp {
    topWindow: any;
    $otpForm: any;
    $txtOtp: any;
    $btnSubmit: any;
    $otpMessage: any;
    $btnGenerateOTP: any;
    $countDown: any;
    lockedAccountCode: number = 200;

    resources = {
        requiredFieldMessage: (<HTMLInputElement>document.getElementById('requiredFieldMessage')).value
    }

    constructor(public $, topWindow: any) {
        this.topWindow = topWindow
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
        this.$btnGenerateOTP = this.$("#btnGenerateOTP");
        this.$countDown = this.$("#countdown");
    }

    registerEvents = () => {
        this.$otpForm.submit(this.verifyOtpEvent);

        if (top.hasPopupOpen()) {
            top.setSecurityCodePopupTitle(this.$("#title_header").text());
            this.$otpForm.addClass("has-popup");
            this.$("#title_header").remove();
        }

        this.$btnGenerateOTP.click(this.generateOTP);
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
            setTimeout(() => {
                securityHelper.afterVerifed();
                this.$txtOtp.attr("disabled", "disabled");
                this.$btnGenerateOTP.attr("disabled", "disabled").remove();
                this.$btnSubmit.attr("disabled", "disabled").remove();
            }, 3000);
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

    generateOTP = (e: any) => {
        e.preventDefault();
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/GenerateOTP",
            success: this.onGenerateOTPSuccess,
            complete: this.onComplete
        });
    }

    onGenerateOTPSuccess = (data) => {
        var result = new Result(data);
        if (result.isSuccessful) {
            this.$txtOtp.focus();
            var countDownTime = new Date().getTime() + 180000;
            this.countDown(countDownTime);
        } else {
            this.$(this.$otpMessage).removeClass('hidden');
            alpha.showMessage(this.$otpMessage, result);

            if (result.code === this.lockedAccountCode) {
                alert(result.message);
                alpha.core.signOut();
            }
        }
    }

    countDown = (countDownTime) => {
        this.$btnGenerateOTP.attr("disabled", "disabled");
        var x = setInterval(() => {
            var now = new Date().getTime();
            var distance = countDownTime - now;
            if (distance < 0) {
                clearInterval(x);
                this.$countDown.text("");
                this.$btnGenerateOTP.removeAttr("disabled", "disabled");
            } else {
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                this.$countDown.text(`(${minutes}:${seconds})`);
            }
        }, 1000);
    }
}

var verifyOtp = new VerifyOtp($, window.top);

$(function () {
    verifyOtp.init();
});