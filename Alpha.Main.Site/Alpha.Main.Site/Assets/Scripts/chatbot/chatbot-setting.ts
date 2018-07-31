/// <reference path="../core.ts" />

"use strict";

class ChatBotSetting extends BaseChatBot {
    $: JQueryStatic;
    addAccount: AddAccount;
    bootbox: any;

    // Language
    $language: JQuery;
    $languageSelectorContainer: JQuery;
    $changeLanguageLink: JQuery;
    $userId: JQuery;
    $userName: JQuery;
    $btnUnsubscribe: JQuery;

    constructor($: JQueryStatic, bootbox: any) {
        super($)
        this.$ = $;
        this.bootbox = bootbox;
    }

    init = () => {
        this.cachedDOMElements();
        this.initLanguage();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$language = this.$("#language");
        this.$languageSelectorContainer = this.$(".language-selector-container");
        this.$changeLanguageLink = this.$(".changelanguage-link");
        this.$userId = this.$("#UserId");
        this.$userName = this.$("#UserName");
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
    }

    registerEvents = () => {
        var self = this;
        this.$(".function").click((e) => {
            var userId = self.$userId.val();
            var checkbox = self.$(e.target);
            var functionId = checkbox.attr("data-functionId");
            var command = checkbox.attr("data-command");
            var isEnable = checkbox.is(":checked");
            self.updateFunction(userId, functionId, command, isEnable);
        });

        this.$btnUnsubscribe.click(this.unsubscribeEvent);
        this.$changeLanguageLink.on('click touchend', (e) => {
            self.changeLanguage(self.$(e.target).data("value"));
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

    unsubscribeEvent = (e: any) => {
        e.preventDefault();
        var self = this;
        var userId = self.$userId.val();
        var userName = self.$userName.val();
        this.bootbox.confirm(this.resources.unsubscribeWarning, (ok) => {
            if (ok) {
                this.$.blockUI();
                this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "ChatBot/Unsubscribe",
                    data: { userId: userId, messengerUserName: userName },
                    dataType: "json",
                    success: self.onUnSubscribeSuccess,
                    complete: self.onComplete
                });
            }
        });
    }

    onUnSubscribeSuccess = (data: any) => {
        if (data.IsOk) {
            parent.window.location.href = parent.window.location.href;
        }
    }

    changeLanguage = (lang) => {
        var self = this;
        var userId = this.$userId.val();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ChangeLanguage",
            data: { userId: userId, language: lang },
            success: function (result) {
                if (result && result.IsOk) {
                    var language = self.$(".list-language ." + lang + " > a").html();
                    self.$languageSelectorContainer.html(language + '<span class="caret"></span>');
                }
            }
        });
    }
}

$(function () {
    var chatBotSetting = new ChatBotSetting($, bootbox);
    chatBotSetting.init();
});