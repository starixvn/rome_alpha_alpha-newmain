/// <reference path="../core.ts" />
'use strict';

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
            self.topWindow.popupManager.open(url, "Edit API Key", 800, 500, null, null, null, 335, 150);
        })
    }

    registerApiKeyRevokeEvent = () => {
        const self = this;
        self.$(".btn-revoke").click(function () {
            const apiName = self.$(this).attr("apiname");
            const title = `Are you sure you want to revoke this API Key <b>${apiName}</b> ?  This action cannot be undone.`;
            self.topWindow.bootboxConfirm(title, undefined, () => {
                const key = self.$(this).attr("apikey");
                self.$.ajax({
                    type: 'POST',
                    url: alpha.urls.mainRootPath + `ApiKey/RevokeApiKey`,
                    data: { key },
                    dataType: 'json',
                    success: function (data, textStatus, jqXHR) {
                        window.location.reload();
                    },
                    complete: function (jqXHR, textStatus) {

                    }
                })
            });
        })
    }
}

$(function () {
    const apiKeyManagement = new ApiKeyManagement($, window.top);
    apiKeyManagement.init();
});
