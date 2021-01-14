/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
var VerifyOtpDefferenceDevice = /** @class */ (function () {
    function VerifyOtpDefferenceDevice($) {
        var _this = this;
        this.$ = $;
        this.resources = {
            requiredFieldMessage: document.getElementById('requiredFieldMessage').value
        };
        this.init = function () {
            securityHelper.initLanguage();
            _this.cachedElements();
            _this.registerEvents();
            var actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
            _this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
        };
        this.cachedElements = function () {
            _this.$btnBack = _this.$("#btn-back");
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOTP");
        };
        this.registerEvents = function () {
            _this.$btnBack.click(alpha.core.signOut);
            _this.$otpForm.submit(_this.verifyOtpEvent);
        };
        this.verifyOtpEvent = function (e) {
            e.preventDefault();
            var otp = _this.$txtOtp.val();
            if (otp.length === 0) {
                _this.$txtOtp.addClass("error").next().text(_this.resources.requiredFieldMessage);
                return;
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/VerifyOtp",
                data: { otp: otp },
                dataType: "json",
                success: _this.onVerifiedSuccess,
                complete: _this.onComplete
            });
        };
        this.onVerifiedSuccess = function (data) {
            var result = new Result(data);
            if (result.isSuccessful) {
                securityHelper.gotoHomePage();
            }
            else {
                _this.$txtOtp.addClass("error").next().text(result.message);
            }
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
    }
    return VerifyOtpDefferenceDevice;
}());
var verifyOtpDefferenceDevice = new VerifyOtpDefferenceDevice($);
$(function () {
    verifyOtpDefferenceDevice.init();
});
//# sourceMappingURL=verify-otp-difference-device.js.map