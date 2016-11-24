/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

class ResetOtp {
    $otpForm: any;
    allowReset: boolean = false;

    resources = {
        resetOtpWarning: (<HTMLInputElement>document.getElementById('ResetOtpWarning')).value
    }

    constructor(public $, public bootbox) {
    }

    init = () => {
        this.$otpForm = this.$("#otp-form");
        this.$otpForm.submit(this.verifyOtpEvent);
    }

    verifyOtpEvent = (e: any) => {
        var that = this;
        if (that.allowReset) {
            return;
        }

        e.preventDefault();
        this.bootbox.confirm(this.resources.resetOtpWarning, function (ok) {
            if (ok) {
                that.allowReset = true;
                that.$otpForm.submit();
            }
        });
    }
}

var resetOtp = new ResetOtp($, bootbox);

$(function () {
    resetOtp.init();
});