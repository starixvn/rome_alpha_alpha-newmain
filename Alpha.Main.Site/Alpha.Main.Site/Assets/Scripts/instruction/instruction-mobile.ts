module Instruction {
    export class Mobile {
        private mobileBlockUI: JQuery;
        private mobileIntroModal: JQuery;
        private mobileCloseIntroBtn: JQuery;
        private $: JQueryStatic;
        static mobileInstructionKey: string = 'MobileInstruction';

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
            return alpha.localStorage.get(Mobile.mobileInstructionKey);
        }

        private registerEvent = () => {
            this.mobileCloseIntroBtn.click(() => {
                this.hideIntroduction();
                headerMessage.init();
            });

            this.$('.header-nav, .header-nav .icon-collapse').click(() => {
                this.hideIntroduction();
            });
        }

        private setReadIntroduction = () => {
            alpha.localStorage.set(Mobile.mobileInstructionKey, true);
        }
    }
}