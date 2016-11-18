class HeaderMessageMobile extends HeaderMessage {
    private toastCloseButtonHtml = '<button type="button" class="icon icon-close" data-dismiss="modal" aria-label="Close"></button>';
    private storedMoveBetsEndTime: Date = new Date();
    private hotMessageModal: JQuery;

    init = () => {
        this.hotMessageModal = this.$('#userMessageModal');

        this.showMoveBetsMessagePopUp();
        setInterval(this.showMoveBetsMessagePopUp, 60000);
    }

    showMoveBetsMessagePopUp = () => {
        this.$.ajax({
            type: "POST",
            url: this.urls.getMessages,
            success: this.onSuccessGetMessages
        });
    }

    private onSuccessGetMessages = (rawResults: string) => {
        const results = alpha.core.convertReponseToJson(rawResults),
            movebetsMessage: string = results.movebets_message || "",
            movebetsEndTime: Date = results.movebets_endtime ? new Date(results.movebets_endtime) : null;

        if (movebetsEndTime !== null && movebetsEndTime > this.storedMoveBetsEndTime) {
            this.showMoveBetsMessageToaster(movebetsMessage);
            this.storedMoveBetsEndTime = movebetsEndTime;
        }
    }

    private showMoveBetsMessageToaster = (message: string) => {
        toastr.options.positionClass = 'toast-top-full-width toastr-announcement';
        toastr.options.closeButton = true;
        toastr.options.closeHtml = this.toastCloseButtonHtml;
        toastr.options.timeOut = 10000;
        toastr.options.preventDuplicates = true;
        toastr.warning(message).parent().css('top', this.getHotMessageHeight());

        this.handleHotMessageClick();
    }

    private getHotMessageHeight = (): number => {
        return this.hotMessageModal.is(':visible') ? (48 + this.hotMessageModal.height() + 2) : 48;
    }

    private handleHotMessageClick = () => {
        this.hotMessageModal.on('click', () => {
            this.$('#toast-container').css('top', this.getHotMessageHeight());
        });
    }
}

$(() => {
    const isReadInstruction = alpha.localStorage.check(alpha.key.mobileInstruction);

    if (alpha.isScreenLower768 && isReadInstruction) {
        const headerMessageMobile = new HeaderMessageMobile($);
        headerMessageMobile.init();
    }
});