/// <reference path="../core.ts" />

class ApiKeyManagement {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.registerApiKeyStatisticsEvent();
        this.registerApiKeyEditEvent();
        this.registerApiKeyRevokeEvent();
    }

    registerApiKeyStatisticsEvent = () => {
        const self = this;
        self.$(".btn-stats").click(function () {
            const apikey = self.$(this).attr("apikey");
            const url = alpha.urls.mainRootPath + "ApiKey/ApiKeyStatistics";
            self.topWindow.popupManager.open(url, "API key Statistics", 800, 500);
        })
    }

    registerApiKeyEditEvent = () => {
        const self = this;
        self.$(".btn-edit").click(function () {
            const apikey = self.$(this).attr("apikey");
            const url = alpha.urls.mainRootPath + "ApiKey/EditApiKey";
            self.topWindow.popupManager.open(url, "Edit API Key", 500, 500);
        })
    }

    registerApiKeyRevokeEvent = () => {
        const self = this;
        self.$(".btn-revoke").click(function () {
            const apikey = self.$(this).attr("apikey");
            self.topWindow.bootboxConfirm(apikey, undefined);
        })
    }
}

$(function () {
    const apiKeyManagement = new ApiKeyManagement($, window.top);
    apiKeyManagement.init();
});
