module Dashboard {
    export class Base {
        protected $: JQueryStatic;

        constructor($: JQueryStatic) {
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

        private createAjaxSettings = (url: string, method: string, component: any, handleOnSuccessLoading: any): JQueryAjaxSettings => {
            const ajaxSetting: JQueryAjaxSettings = {
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
}