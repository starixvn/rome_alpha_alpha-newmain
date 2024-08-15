/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class VerifyOtpDefferenceDevice {
    $btnBack: any;
    $otpForm: any;
    $txtOtp: any;
    $btnGenerateOTP: any;
    $countDown: any;
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
    }

    cachedElements = () => {
        this.$btnBack = this.$("#btn-back");
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOTP");
        this.$btnGenerateOTP = this.$("#btnGenerateOTP");
        this.$countDown = this.$("#countdown");
    }

    registerEvents = () => {
        this.$btnBack.click(() => alpha.core.signOut2());
        this.$otpForm.submit(this.verifyOtpEvent);
        this.$btnGenerateOTP.click(this.generateOTP);
        this.$otpForm.on("keypress", (e) => {
            var keyPressed = e.keyCode || e.which;
            if (keyPressed === 13) {
                this.verifyOtpEvent(e)
            }
        });
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
        }
        else {
            this.$txtOtp.addClass("error");
            this.$("#error").text(result.message);
            if (result.code === this.lockedAccountCode) {
                alert(result.message);
                alpha.core.signOut(result.message);
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

    verifyOtpEvent = (e: any) => {
        e.preventDefault();

        var otp = this.$txtOtp.val();
        if (otp.length === 0) {
            this.$txtOtp.addClass("error");
            this.$("#error").text(this.resources.requiredFieldMessage);
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
            this.$txtOtp.addClass("error");
            this.$("#error").text(result.message);

            if (result.code === this.lockedAccountCode) {
                alert(result.message);
                alpha.core.signOut(result.message);
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