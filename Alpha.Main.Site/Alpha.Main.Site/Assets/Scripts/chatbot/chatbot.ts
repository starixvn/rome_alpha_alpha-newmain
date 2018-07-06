/// <reference path="../core.ts" />

"use strict";

class ChatBot extends BaseChatBot {
    $: JQueryStatic;
    addAccount: AddAccount;

    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;

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
        }
        else {
            this.addAccount.init();
        }
    }

    cachedDOMElements = () => {
        this.$language = this.$("#language");
        this.$languageSelectorContainer = this.$(".language-selector-container");
        this.$changeLanguageLink = this.$(".changelanguage-link");
    }

    initLanguage = () => {
        var currentLanguage = this.$language.data("currentlanguage");
        var language = this.$(".list-language ." + currentLanguage + " > a").html();

        this.$languageSelectorContainer.prepend(language);
    }
}

$(function () {
    var chatBot = new ChatBot($, bootbox);
    chatBot.init();
});