class InstructionMobile extends Instruction {
    private mobileBlockUI: JQuery;
    private mobileIntroModal: JQuery;
    private mobileCloseIntroBtn: JQuery;

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

    initHeaderMessage = () => {
        const headerMessage = new HeaderMessageMobile(this.$);
        headerMessage.init();
    }

    private cacheDom = () => {
        this.mobileBlockUI = this.$('.blockUI-mobile');
        this.mobileIntroModal = this.$('#instruction-mobile');
        this.mobileCloseIntroBtn = this.$('#instruction-mobile').find('.button-close');
    }

    private registerEvent = () => {
        this.mobileCloseIntroBtn.on('click', () => {
            this.hideInstruction();
        });

        this.$('.header-nav, .header-nav .icon-collapse').on('click', () => {
            this.hideInstruction();
        });
    }
}

$(() => {
    if (alpha.isScreenLower768) {
        const mobile = new InstructionMobile($);
        mobile.init();
    }
});