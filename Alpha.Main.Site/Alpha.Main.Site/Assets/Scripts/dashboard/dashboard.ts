/// <reference path="./balance-info.ts" />
/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />

class Dashboard {
    protected $: any;

    constructor($: any) {
        this.$ = $;
    }

    loadInfo = (id: string, handleOnSuccessLoading: any) => {
        let $component = this.$(id),
            url = $component.attr('action-url');

        if (url !== '' && typeof url !== 'undefined') {
            const ajaxSettings = this.createAjaxSettings(url, 'GET', $component, handleOnSuccessLoading);

            this.$.ajax(ajaxSettings);
        }
    }

    private createAjaxSettings = (url: string, method: string, component: any, handleOnSuccessLoading: any) => {
        const ajaxSetting = {
            method: method,
            url: url,
            success: (responseHtml) => {
                component.html(responseHtml);
                handleOnSuccessLoading();
            }
        };

        return ajaxSetting;
    }
}