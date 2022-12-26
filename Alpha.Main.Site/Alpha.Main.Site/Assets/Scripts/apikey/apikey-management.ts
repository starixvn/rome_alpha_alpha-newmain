/// <reference path="../core.ts" />

class ApiKeyManagement {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.registerApiKeyEditEvent();
        this.registerApiKeyRevokeEvent();
    }

    registerApiKeyEditEvent = () => {
        const self = this;
        self.$(".btn-edit").click(function () {
            const apikey = self.$(this).attr("apikey");
            const url = alpha.urls.mainRootPath + `ApiKey/EditApiKey?key=${apikey}`;
            self.topWindow.popupManager.open(url, "Edit API Key", 800, 500);
        })
    }

    registerApiKeyRevokeEvent = () => {
        const self = this;
        self.$(".btn-revoke").click(function () {
            const key = self.$(this).attr("apikey");
            self.$.ajax({
                type: 'POST',
                url: alpha.urls.mainRootPath + `ApiKey/RevokeApiKey`,
                data: { key },
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    //self.topWindow.bootboxConfirm(apikey, undefined);
                    window.location.reload();
                },
                complete: function (jqXHR, textStatus) {

                },
                error: function (jqXHR) {

                }
            })
        })
    }
}

$(function () {
    const apiKeyManagement = new ApiKeyManagement($, window.top);
    apiKeyManagement.init();
});
