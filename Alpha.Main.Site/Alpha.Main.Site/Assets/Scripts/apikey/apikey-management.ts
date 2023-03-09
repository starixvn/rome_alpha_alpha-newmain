/// <reference path="../core.ts" />
'use strict';

declare var Clipboard: any;

class ApiKeyManagement {
    $: JQueryStatic;
    topWindow: any;
    $showURL: JQuery;
    $urlMask: JQuery;
    $apiKeyNames: JQuery;
    $apiKeyPopups: JQuery;
    clipboard: any;

    constructor($: JQueryStatic, topWindow: any, clipboard: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboard = clipboard;
    }

    init = () => {
        this.$showURL = this.$(".dummy-text");
        this.$urlMask = this.$(".service-url-mask");
        this.$apiKeyNames = this.$(".api-key-name");
        this.$apiKeyPopups = this.$(".api-key-popup");

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
            self.topWindow.popupManager.open(url, self.resources.editAPIKey, 380, 540, null, null, null, 335, 150);
        })

        this.$showURL.click(this.showURL);

        this.$apiKeyNames.click(function () { self.showApiKey(self.$(this)); });

        if (window.document.queryCommandSupported
            && window.document.queryCommandSupported("copy")) {
            this.clipboard
                .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                    setTimeout(() => { self.$apiKeyPopups.addClass('hide'); }, 3000);
                })
                .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
        }
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

    showApiKey = ($elm) => {
        this.$apiKeyPopups.addClass('hide');

        var popup = $elm.parent().find('.api-key-popup');
        popup.removeClass('hide');

        popup.css({ left: $elm.position().left });
    };
}

$(function () {
    const apiKeyManagement = new ApiKeyManagement($, window.top, new Clipboard('.icon-content-copy'));
    apiKeyManagement.init();
});
