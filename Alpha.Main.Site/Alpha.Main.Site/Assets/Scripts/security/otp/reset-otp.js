/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
var ResetOtp = /** @class */ (function () {
    function ResetOtp($, bootbox) {
        var _this = this;
        this.$ = $;
        this.bootbox = bootbox;
        this.allowReset = false;
        this.resources = {
            resetOtpWarning: document.getElementById('ResetOtpWarning').value
        };
        this.init = function () {
            _this.$otpForm = _this.$("#otp-form");
            _this.$otpForm.submit(_this.verifyOtpEvent);
        };
        this.verifyOtpEvent = function (e) {
            var that = _this;
            if (that.allowReset) {
                return;
            }
            e.preventDefault();
            _this.bootbox.confirm(_this.resources.resetOtpWarning, function (ok) {
                if (ok) {
                    that.allowReset = true;
                    that.$otpForm.submit();
                }
            });
        };
    }
    return ResetOtp;
}());
var resetOtp = new ResetOtp($, bootbox);
$(function () {
    resetOtp.init();
});
//# sourceMappingURL=reset-otp.js.map