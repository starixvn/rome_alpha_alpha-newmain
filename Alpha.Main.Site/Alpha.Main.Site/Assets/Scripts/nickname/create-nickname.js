"use strict";
var Nickname = /** @class */ (function () {
    function Nickname($) {
        this.$ = $;
    }
    Nickname.prototype.init = function () {
        this.cacheElements();
        this.bindEvents();
    };
    Nickname.prototype.cacheElements = function () {
        this.nicknameRule = this.$("#nicknameRule").val();
        this.$nickname = this.$("#nickname");
        this.$nicknameLabel = this.$("#nickname-error");
        this.$loginByUsernameOrNickname = this.$("#loginByUsernameOrNickname");
        this.$loginByNickname = this.$("#loginByNickname");
        this.$buttonSubmit = this.$("#buttonSubmit");
        this.mainRootPath = this.$("#mainRootPath").val();
    };
    Nickname.prototype.bindEvents = function () {
        var self = this;
        this.$nickname
            .focus()
            .keyup(function () {
            var nickname = self.$.trim(self.$nickname.val());
            if (nickname.length > 0) {
                var validNickname = self.isValidNickname(nickname);
                if (validNickname) {
                    self.showValidIcon();
                    self.removeErrorMessage();
                }
                else {
                    self.showErrorIcon();
                }
            }
        });
        this.$loginByUsernameOrNickname.change(function () {
            self.$buttonSubmit.prop("disabled", true);
        });
        this.$loginByNickname.change(function () {
            self.$buttonSubmit.prop("disabled", false);
        });
        this.$("#frmNickname").submit(function () {
            self.$.blockUI();
            var nickname = self.$.trim(self.$nickname.val());
            var validNickname = self.isValidNickname(nickname);
            if (!validNickname) {
                self.$.unblockUI();
                self.showErrorIcon();
                self.showErrorMessage(self.nicknameRule);
                self.$nickname.focus();
            }
            return validNickname;
        });
        this.$("#buttonBack").click(alpha.core.signOut);
        this.$(".changelanguage-link").click(function () {
            self.changeLanguage(self.$(this).data("value"));
        });
    };
    Nickname.prototype.isValidNickname = function (nickname) {
        var nicknameRuleRegExp = new RegExp(/^[a-zA-Z0-9]{5,20}$/i);
        return nicknameRuleRegExp.test(nickname);
    };
    Nickname.prototype.showValidIcon = function () {
        this.$nickname.removeClass("error").addClass("valid");
    };
    Nickname.prototype.showErrorIcon = function () {
        this.$nickname.removeClass("valid").addClass("error");
    };
    Nickname.prototype.showErrorMessage = function () {
        this.$nicknameLabel.addClass("error").html(this.nicknameRule);
    };
    Nickname.prototype.removeErrorMessage = function () {
        this.$nicknameLabel.empty().removeClass("error");
    };
    Nickname.prototype.changeLanguage = function (lang) {
        this.$.ajax({
            type: "POST",
            url: this.mainRootPath + "Language/Change",
            data: { languageCode: lang },
            success: function (result) {
                if (result) {
                    window.location.reload(true);
                }
            }
        });
    };
    return Nickname;
}());
$(function () {
    new Nickname($).init();
});
//# sourceMappingURL=create-nickname.js.map