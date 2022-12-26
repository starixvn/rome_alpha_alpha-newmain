/// <reference path="../core.ts" />

'use strict';
class BaseApiKey {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }
}

class CreateApiKey extends BaseApiKey {
    $btnSubmit: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow);
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$btnSubmit = this.$(".btn-submit");
    }


    registerEvents = () => {
        this.$btnSubmit.click(this.showApiResult);
    }

    showApiResult = () => {
        this.topWindow.bootboxConfirm("API Key created!", undefined);
    }
}

$(function () {
    const createApiKey = new CreateApiKey($, window.top);
    createApiKey.init();
});
