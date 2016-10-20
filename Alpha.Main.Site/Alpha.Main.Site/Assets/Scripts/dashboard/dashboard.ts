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
            const ajaxSettings = this.createAjaxSettings(url, $component, handleOnSuccessLoading);

            this.$.get(ajaxSettings);
        }
    }

    private createAjaxSettings = (url: string, component: any, handleOnSuccessLoading: any) => {
        const ajaxSetting = {
            url: url,
            onSuccess: (responseHtml) => {
                component.html(responseHtml);
                handleOnSuccessLoading();
            }
        };

        return ajaxSetting;
    }
}