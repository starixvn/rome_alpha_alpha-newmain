/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
var VerifyOtp = /** @class */ (function () {
    function VerifyOtp($) {
        var _this = this;
        this.$ = $;
        this.closedAccountCode = 200;
        this.resources = {
            requiredFieldMessage: document.getElementById('requiredFieldMessage').value
        };
        this.init = function () {
            _this.cachedElements();
            _this.registerEvents();
            var actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
            _this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
        };
        this.cachedElements = function () {
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOTP").val("");
            _this.$btnSubmit = _this.$("#btnSubmit");
            _this.$otpMessage = _this.$("#otp-message");
        };
        this.registerEvents = function () {
            _this.$otpForm.submit(_this.verifyOtpEvent);
            if (top.hasPopupOpen()) {
                top.setSecurityCodePopupTitle(_this.$("#title_header").text());
                _this.$otpForm.addClass("has-popup");
                _this.$("#title_header").remove();
            }
        };
        this.verifyOtpEvent = function (e) {
            e.preventDefault();
            var otp = _this.$txtOtp.val();
            if (otp.length === 0) {
                _this.$(_this.$otpMessage).removeClass('hidden');
                alpha.showMessage(_this.$otpMessage, new Result({ Code: 1, Message: _this.resources.requiredFieldMessage }));
                _this.$txtOtp.focus();
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
                securityHelper.afterVerifed();
                _this.$txtOtp.attr("disabled", "disabled");
                _this.$btnSubmit.attr("disabled", "disabled").remove();
            }
            else {
                _this.$(_this.$otpMessage).removeClass('hidden');
                alpha.showMessage(_this.$otpMessage, result);
                _this.$txtOtp.focus();
                if (result.code === _this.closedAccountCode) {
                    alert(result.message);
                    alpha.core.signOut();
                }
            }
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
    }
    return VerifyOtp;
}());
var verifyOtp = new VerifyOtp($);
$(function () {
    verifyOtp.init();
});
//# sourceMappingURL=verify-otp.js.map