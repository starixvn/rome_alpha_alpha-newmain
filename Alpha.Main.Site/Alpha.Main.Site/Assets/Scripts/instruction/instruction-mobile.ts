class InstructionMobile {
    private mobileBlockUI: JQuery;
    private mobileIntroModal: JQuery;
    private mobileCloseIntroBtn: JQuery;
    private $: JQueryStatic;
    private mobileInstructionKey: string = 'MobileInstruction';

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        if (!this.readInstruction()) {
            this.cacheDom();
            this.registerEvent();
            this.showInstruction();
        }
    }

    showInstruction = () => {
        this.mobileBlockUI.show();
        this.mobileIntroModal.show();
    }

    hideInstruction = () => {
        if (this.mobileBlockUI.is(':visible') || this.mobileIntroModal.is(':visible')) {
            this.setReadInstruction();
            this.mobileBlockUI.hide();
            this.mobileIntroModal.hide(0, headerMessage.init);
        }
    }

    readInstruction = (): boolean => {
        return alpha.localStorage.get(this.mobileInstructionKey);
    }

    setReadInstruction = () => {
        alpha.localStorage.set(this.mobileInstructionKey, true);
    }

    private cacheDom = () => {
        this.mobileBlockUI = this.$('.blockUI-mobile');
        this.mobileIntroModal = this.$('#instruction-mobile');
        this.mobileCloseIntroBtn = this.$('#instruction-mobile').find('.button-close');
    }

    private registerEvent = () => {
        this.mobileCloseIntroBtn.click(() => {
            this.hideInstruction();
        });

        this.$('.header-nav, .header-nav .icon-collapse').click(() => {
            this.hideInstruction();
        });
    }
}