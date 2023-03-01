/// <reference path="../core.ts" />
'use strict';

class ApiKeyManagement {
    $: JQueryStatic;
    topWindow: any;
    $showURL: JQuery;
    $urlMask: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.$showURL = this.$(".dummy-text");
        this.$urlMask = this.$(".service-url-mask");

        this.registerApiKeyEditEvent();
        this.registerApiKeyRevokeEvent();
    }

    resources = {
        editAPIKey: (<HTMLInputElement>document.getElementById('EditAPIKey')).value,
        areYouSureRevokeAPIKey: (<HTMLInputElement>document.getElementById('AreYouSureRevokeAPIKey')).value,
        thisActionCannotBeUndone: (<HTMLInputElement>document.getElementById('ThisActionCannotBeUndone')).value,
    }

    registerApiKeyEditEvent = () => {
        const self = this;
        self.$(".btn-edit").click(function () {
            const apikey = self.$(this).attr("apikey");
            const url = alpha.urls.mainRootPath + `ApiKey/EditApiKey?key=${apikey}`;
            self.topWindow.popupManager.open(url, self.resources.editAPIKey, 800, 500, null, null, null, 335, 150);
        })

        this.$showURL.click(this.showURL);
    }

    registerApiKeyRevokeEvent = () => {
        const self = this;
        self.$(".btn-revoke").click(function () {
            const apiName = self.$(this).attr("apiname");
            const title = `${self.resources.areYouSureRevokeAPIKey} <b>${apiName}</b>?  ${self.resources.thisActionCannotBeUndone}`;
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

    showURL = () => {
        this.$urlMask.removeClass('close');
        this.$urlMask.addClass('open');
    }
}

$(function () {
    const apiKeyManagement = new ApiKeyManagement($, window.top);
    apiKeyManagement.init();
});
