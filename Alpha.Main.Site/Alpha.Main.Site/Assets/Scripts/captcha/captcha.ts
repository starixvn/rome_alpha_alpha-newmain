"use strict";
class Captcha {
    $: JQueryStatic;
    $captchaRefreshIcon: JQuery;
    $modelImageElement: JQuery;
    $modelTokenParameterName: JQuery;
    $modelTokenElement: JQuery;
    $modelRefreshUrl: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.cacheDOMElements();
        this.registerEvents();
    }

    cacheDOMElements(): void {
        this.$captchaRefreshIcon = this.$("#captcha-refresh-icon");

        const imageElementId: string = this.$("#ModelImageElementId").val();
        this.$modelImageElement = this.$("#" + imageElementId);
        this.$modelTokenParameterName = this.$("#ModelTokenParameterName");

        const tokenElementId: string = this.$("#ModelTokenElementId").val();
        this.$modelTokenElement = this.$("#" + tokenElementId);
        this.$modelRefreshUrl = this.$("#ModelRefreshUrl");
    }

    registerEvents(): void {
        this.$captchaRefreshIcon.click(this.refreshCaptcha);
        this.$modelImageElement.click(this.refreshCaptcha);
    }

    refreshCaptcha = () => {
        const url: string = this.$modelRefreshUrl.val();
        let tokenParams: any = {};
        tokenParams[this.$modelTokenParameterName.val()] = this.$modelTokenElement.val();

        this.$.post(url, tokenParams)
            .always(function (result) {
                eval(result.responseText);
            });
    }
}

$(function () {
    const captcha = new Captcha($);
    captcha.init();
});