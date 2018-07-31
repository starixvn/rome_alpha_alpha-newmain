/// <reference path="../core.ts" />

"use strict";

class ChatBot extends BaseChatBot {
    $: JQueryStatic;
    addAccount: AddAccount;

    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;
    $userId: JQuery;

    constructor($: JQueryStatic, bootbox: any) {
        super($)
        this.$ = $;
        this.addAccount = new AddAccount($, new VerifyAccount($), bootbox);
    }

    init = () => {
        var isAlreadySetup = this.convertAttributeToBool(this.$("#IsAlreadySetup").val());

        if (isAlreadySetup) {
            this.cachedDOMElements();
            this.initLanguage();
            this.registerEvents();
        }
        else {
            this.addAccount.init();
        }
    }

    cachedDOMElements = () => {
        this.$language = this.$("#language");
        this.$languageSelectorContainer = this.$(".language-selector-container");
        this.$changeLanguageLink = this.$(".changelanguage-link");
        this.$userId = this.$("#UserId");
    }

    registerEvents = () => {
        var self = this;
        this.$(".function").click(function () {
            var userId = self.$userId.val();
            var checkbox = self.$(this);
            var functionId = checkbox.attr("data-functionId");
            var command = checkbox.attr("data-command");
            var isEnable = checkbox.is(":checked");
            self.updateFunction(userId, functionId, command, isEnable);
        });
    }

    initLanguage = () => {
        var currentLanguage = this.$language.data("currentlanguage");
        var language = this.$(".list-language ." + currentLanguage + " > a").html();

        this.$languageSelectorContainer.prepend(language);
    }

    updateFunction = (userId: number, functionId: any, command: string, isEnable: boolean) => {
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/UpdateFunction",
            data: { command: { userId: userId, functionId: functionId, command: command, isEnable: isEnable } },
            dataType: "json",
            complete: this.onComplete,
        });
    }
}

$(function () {
    var chatBot = new ChatBot($, bootbox);
    chatBot.init();
});