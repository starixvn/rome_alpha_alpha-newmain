"use strict";
var NewCustomerCaptcha = /** @class */ (function () {
    function NewCustomerCaptcha($) {
        var _this = this;
        this.refreshCaptcha = function () {
            var url = _this.$modelRefreshUrl.val();
            var tokenParams = {};
            tokenParams[_this.$modelTokenParameterName.val()] = _this.$modelTokenElement.val();
            _this.$.post(url, tokenParams)
                .always(function (result) {
                eval(result.responseText);
            });
        };
        this.$ = $;
    }
    NewCustomerCaptcha.prototype.init = function () {
        this.cacheDOMElements();
        this.registerEvents();
    };
    NewCustomerCaptcha.prototype.cacheDOMElements = function () {
        this.$captchaRefreshIcon = this.$("#captcha-refresh-icon");
        var imageElementId = this.$("#ModelImageElementId").val();
        this.$modelImageElement = this.$("#" + imageElementId);
        this.$modelTokenParameterName = this.$("#ModelTokenParameterName");
        var tokenElementId = this.$("#ModelTokenElementId").val();
        this.$modelTokenElement = this.$("#" + tokenElementId);
        this.$modelRefreshUrl = this.$("#ModelRefreshUrl");
    };
    NewCustomerCaptcha.prototype.registerEvents = function () {
        this.$captchaRefreshIcon.click(this.refreshCaptcha);
        this.$modelImageElement.click(this.refreshCaptcha);
    };
    return NewCustomerCaptcha;
}());
$(function () {
    var captcha = new NewCustomerCaptcha($);
    captcha.init();
});
//# sourceMappingURL=captcha.js.map