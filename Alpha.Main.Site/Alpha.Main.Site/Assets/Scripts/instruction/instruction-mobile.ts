class InstructionMobile extends Instruction {
    private mobileBlockUI: JQuery;
    private mobileIntroModal: JQuery;
    private mobileCloseIntroBtn: JQuery;

    constructor($: JQueryStatic) {
        super($);
    }

    init = () => {
        this.readInstructionKey = alpha.key.mobileInstruction

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
            this.mobileIntroModal.hide(0, this.initHeaderMessage);
        }
    }

    private initHeaderMessage = () => {
        const headerMessage = new HeaderMessage(this.$)
        headerMessage.init();

        const headerMessageMobile = new HeaderMessageMobile(this.$);
        headerMessageMobile.init();
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