/// <reference path="../core.ts" />
'use strict';

declare var Clipboard: any;

class CreateApiKeyResult {
    $: JQueryStatic;
    topWindow: any;
    clipboardKey: any;
    clipboardSecret: any;

    constructor($: JQueryStatic, topWindow: any, clipboardKey: any, clipboardSecret: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboardKey = clipboardKey;
        this.clipboardSecret = clipboardSecret;
    }

    init = () => {
        this.registerEvents();
    }

    registerEvents = () => {
        const self = this;
        if (window.document.queryCommandSupported
            && window.document.queryCommandSupported("copy")) {
            this.clipboardKey
                .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });

            this.clipboardSecret
                .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
        }

        this.$("#btnOk").click(this.handleBtnOk)
    }

    handleBtnOk = () => {
        this.topWindow.popupManager.close();
    }
}

$(function () {
    const createApiKeyResult = new CreateApiKeyResult($, window.top, new Clipboard('.icon-content-copy'), new Clipboard('.icon-copy-secret'));
    createApiKeyResult.init();
});
