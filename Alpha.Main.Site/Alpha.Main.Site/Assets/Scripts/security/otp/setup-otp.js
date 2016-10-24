/// <reference path="../../core.ts" />
var SetupOtp = (function () {
    function SetupOtp($) {
        var _this = this;
        this.init = function () {
            _this.cachedDomElements();
            _this.registerEvents();
        };
        this.cachedDomElements = function () {
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOtp");
            _this.$registerPanel = _this.$("#register-panel");
            _this.$removeSecretKeyPanel = _this.$("#remove-secret-key-panel");
            _this.$btnRemoveOtp = _this.$("#btn-remove-otp");
        };
        this.registerEvents = function () {
        };
        this.setUpEvent = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/SetUp",
                data: { opt: _this.$txtOtp.val() },
                dataType: "json",
                success: function (result) {
                    if (result.Code == 0) {
                    }
                },
                complete: function () {
                    this.$.unblockUI();
                }
            });
        };
        this.onSuccess = function (result) {
            if (result.Code == 0) {
                _this.$registerPanel.hide();
                _this.$removeSecretKeyPanel.show();
            }
        };
        this.removeOtp = function (result) {
        };
        this.$ = $;
    }
    return SetupOtp;
}());
var setUpOtp = new SetupOtp($);
$(function () {
    setUpOtp.init();
});
//# sourceMappingURL=setup-otp.js.map