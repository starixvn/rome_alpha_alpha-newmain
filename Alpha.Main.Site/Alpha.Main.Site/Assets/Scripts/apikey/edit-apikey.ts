/// <reference path="../core.ts" />

class EditApiKey {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {

    }
}

$(function () {
    const editApiKey = new EditApiKey($, window.top);
    editApiKey.init();
});
