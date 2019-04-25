"use strict";
class Nickname {
    $: JQueryStatic;
    nicknameRule: string;
    $nickname: JQuery;
    $nicknameLabel: JQuery;
    $loginByUsernameOrNickname: JQuery;
    $loginByNickname: JQuery;
    $buttonSubmit: JQuery;
    mainRootPath: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    cacheElements(): void {
        this.nicknameRule = this.$("#nicknameRule").val();
        this.$nickname = this.$("#nickname");
        this.$nicknameLabel = this.$("#nickname-error");
        this.$loginByUsernameOrNickname = this.$("#loginByUsernameOrNickname");
        this.$loginByNickname = this.$("#loginByNickname");
        this.$buttonSubmit = this.$("#buttonSubmit");
        this.mainRootPath = this.$("#mainRootPath").val();
    }

    bindEvents(): void {
        const self: any = this;

        this.$nickname
            .focus()
            .keyup(function (): void {
                const nickname = self.$.trim(self.$nickname.val());

                if (nickname.length > 0) {
                    const validNickname: boolean = self.isValidNickname(nickname);
                    if (validNickname) {
                        self.showValidIcon();
                        self.removeErrorMessage();
                    } else {
                        self.showErrorIcon();
                    }
                }
            });

        this.$loginByUsernameOrNickname.change(function (): void {
            self.$buttonSubmit.prop("disabled", true);
        });

        this.$loginByNickname.change(function (): void {
            self.$buttonSubmit.prop("disabled", false);
        });

        this.$("#frmNickname").submit(function (): boolean {
            self.$.blockUI();

            const nickname = self.$.trim(self.$nickname.val());
            const validNickname: boolean = self.isValidNickname(nickname);

            if (!validNickname) {
                self.$.unblockUI();
                self.showErrorIcon();
                self.showErrorMessage(self.nicknameRule);
                self.$nickname.focus();
            }

            return validNickname;
        });

        this.$("#buttonBack").click(alpha.core.signOut);

        this.$(".changelanguage-link").click(function (): void {
            self.changeLanguage(self.$(this).data("value"));
        });
    }

    isValidNickname(nickname: string): boolean {
        const nicknameRuleRegExp: RegExp = new RegExp(/^[a-zA-Z0-9]{5,15}$/i);

        return nicknameRuleRegExp.test(nickname);
    }

    showValidIcon(): void {
        this.$nickname.removeClass("error").addClass("valid");
    }

    showErrorIcon(): void {
        this.$nickname.removeClass("valid").addClass("error");
    }

    showErrorMessage(): void {
        this.$nicknameLabel.addClass("error").html(this.nicknameRule);
    }

    removeErrorMessage(): void {
        this.$nicknameLabel.empty().removeClass("error");
    }

    changeLanguage(lang: string): void {
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
    }
}

$(function (): void {
    new Nickname($).init();
});