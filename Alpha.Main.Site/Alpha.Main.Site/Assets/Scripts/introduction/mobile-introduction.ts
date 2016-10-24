module Introduction {
    export class MobileIntroduction {
        private mobileBlockUI: JQuery;
        private mobileIntroModal: JQuery;
        private mobileCloseIntroBtn: JQuery;
        private $: JQueryStatic;
        private readMobileIntroductionKey: string = 'ReadMobileIntroduction';

        constructor($: JQueryStatic) {
            this.$ = $;
            this.mobileBlockUI = $('.blockUI-mobile');
            this.mobileIntroModal = $('#instruction-mobile');
            this.mobileCloseIntroBtn = $('#instruction-mobile').find('.button-close');

            this.registerEvent();
        }

        showIntroduction = () => {
            this.mobileBlockUI.show();
            this.mobileIntroModal.show();
        }

        hideIntroduction = () => {
            this.setReadIntroduction();
            this.mobileBlockUI.hide();
            this.mobileIntroModal.hide();
        }

        readIntroduction = (): boolean => {
            return alpha.localStorage.get(this.readMobileIntroductionKey);
        }

        private registerEvent = () => {
            this.mobileCloseIntroBtn.click(() => {
                this.hideIntroduction();
            });

            this.$('.header-nav').click(() => {
                this.hideIntroduction();
            });
        }

        private setReadIntroduction = () => {
            alpha.localStorage.set(this.readMobileIntroductionKey, true);
        }
    }
}