/// <reference path="../core.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AddAccount = /** @class */ (function (_super) {
    __extends(AddAccount, _super);
    function AddAccount($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.resources = {
            accountDoesNotExist: document.getElementById('AccountDoesNotExist').value,
            unsubscribeWarning: document.getElementById('UnsubscribeWarning').value,
            overMaxAddAccountTimes: document.getElementById('OverMaxAddAccountTimes').value,
            accountHasAlreadyBeenRegistered: document.getElementById('AccountHasAlreadyBeenRegistered').value,
            accountIsRequired: document.getElementById('AccountIsRequired').value,
            language: document.getElementById('Language').value
        };
        _this.init = function () {
            _this.cachedDOMElements();
            _this.registerEvents();
            var isVerifying = _this.convertAttributeToBool(_this.$("#IsVerifying").val());
            if (isVerifying) {
                _this.verifyAccount.init();
            }
            _this.cacheDOMAndRegisterFunctionExample(function () { return _this.resources.language; });
        };
        _this.cachedDOMElements = function () {
            _this.$registerLetstalkPanel = _this.$("#registerLetstalkPanel");
            _this.$txtAccount = _this.$("#txtAccount");
            _this.$addAccountMessage = _this.$("#add-account-message");
            _this.$btnAdd = _this.$("#btnAdd");
            _this.$btnUnsubscribe = _this.$("#btnUnsubscribe");
            _this.$logoLetstalk = _this.$("#logoLetstalk");
            _this.$imgQRCode = _this.$("#imgQRCode");
            _this.$linkShowQRCode = _this.$("#showQRCode");
            _this.$linkHideQRCode = _this.$("#hideQRCode");
            _this.$divLinkTooltip = _this.$("#divLinkTooltip");
        };
        _this.registerEvents = function () {
            _this.$txtAccount.keypress(function (e) {
                if (e.which === 32) {
                    return false;
                }
                if (e.keyCode === 13) {
                    _this.$btnAdd.click();
                }
            });
            _this.$btnAdd.click(_this.addAccountEvent);
            _this.$btnUnsubscribe.click(_this.unsubscribeEvent);
            _this.$linkShowQRCode.click(function () {
                _this.$logoLetstalk.addClass("hide");
                _this.$imgQRCode.removeClass("hide");
                _this.$linkShowQRCode.addClass("hide");
                _this.$linkHideQRCode.removeClass("hide");
            });
            _this.$linkHideQRCode.click(function () {
                _this.$imgQRCode.addClass("hide");
                _this.$logoLetstalk.removeClass("hide");
                _this.$linkHideQRCode.addClass("hide");
                _this.$linkShowQRCode.removeClass("hide");
            });
        };
        _this.addAccountEvent = function (e) {
            e.preventDefault();
            var letstalkId = _this.$txtAccount.val().trim();
            _this.$txtAccount.val(letstalkId);
            if (letstalkId.length === 0) {
                _this.$txtAccount.focus();
                alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 1, Message: _this.resources.accountIsRequired }));
                return;
            }
            else {
                _this.$addAccountMessage.html("");
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "ChatBot/AddAccount",
                data: { letstalkId: letstalkId },
                dataType: "text",
                success: _this.onAddAccountSuccess,
                complete: _this.onComplete,
            });
        };
        _this.onAddAccountSuccess = function (data) {
            var result = _this.safelyParseJSON(data);
            if (result && result.IsNotOk) {
                _this.$txtAccount.focus();
                if (result.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(result.Message);
                    return;
                }
                if (result.IsMessengerAccountDoesNotExist) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 2, Message: _this.resources.accountDoesNotExist }));
                    return;
                }
                if (result.IsMessengerAccountHasBeenRegisteredByAnotherCustomer) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 5, Message: _this.resources.accountHasAlreadyBeenRegistered }));
                    return;
                }
                if (result.IsOverMaxAddAccountTimes) {
                    alpha.showMessage(_this.$addAccountMessage, new Result({ Code: 6, Message: _this.resources.overMaxAddAccountTimes }));
                    return;
                }
                alpha.showMessage(_this.$addAccountMessage, new Result({ Code: result.Code, Message: result.Message }));
                return;
            }
            _this.$btnAdd.hide();
            _this.$btnUnsubscribe.show();
            _this.$txtAccount.attr("disabled", "disabled");
            _this.$divLinkTooltip.remove();
            _this.$registerLetstalkPanel.append(data);
            _this.updateExpiredTime();
            _this.verifyAccount.init();
        };
        _this.unsubscribeEvent = function (e) {
            e.preventDefault();
            var self = _this;
            _this.topWindow.bootboxConfirm(_this.resources.unsubscribeWarning, undefined, function () {
                _this.$.blockUI();
                _this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                    dataType: "json",
                    success: self.onUnSubscribeSuccess,
                    complete: self.onComplete
                });
            });
        };
        _this.onUnSubscribeSuccess = function (data) {
            if (data && data.IsNotOk) {
                if (data.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(data.Message);
                    return;
                }
                var result = new Result(data);
                alpha.showMessage(_this.$addAccountMessage, result);
                return;
            }
            parent.window.location.href = parent.window.location.href;
        };
        _this.verifyAccount = new VerifyAccount($, topWindow);
        return _this;
    }
    return AddAccount;
}(BaseChatBot));
$(function () {
    var addAccount = new AddAccount($, window.top);
    addAccount.init();
});
//# sourceMappingURL=add-account.js.map