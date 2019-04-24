"use strict";
class Nickname {
    $: JQueryStatic;
    $nickname: JQuery;
    $messsageContainer: JQuery;
    nicknameRule: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        let self: any = this;

        this.$nickname = this.$("#nickname");
        this.$nickname.focus();
        this.$messsageContainer = this.$("#message-container");
        this.nicknameRule = this.$("#nicknameRule").html();

        this.$("#frmNickname").submit(function (): boolean {
            self.$.blockUI();
            const nicknameRuleRegExp: RegExp = new RegExp(/^[a-zA-Z0-9]{5,15}$/i);
            const isMatch: boolean = nicknameRuleRegExp.test(self.$.trim(self.$nickname.val()));

            if (!isMatch) {
                self.$.unblockUI();
                const result: Result = new Result({ Code: 1, Message: self.nicknameRule });
                alpha.showMessage(self.$messsageContainer, result);
                self.$nickname.focus();
            }

            return isMatch;
        });

        this.$("#buttonBack").click(alpha.core.signOut);
    }
}

$(function (): void {
    new Nickname($).init();
});