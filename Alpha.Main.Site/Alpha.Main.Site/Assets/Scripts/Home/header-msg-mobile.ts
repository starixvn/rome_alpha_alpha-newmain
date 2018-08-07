class HeaderMessageMobile extends HeaderMessage {
    private toastCloseButtonHtml = '<button type="button" class="icon icon-close" data-dismiss="modal" aria-label="Close"></button>';
    private storedMoveBetsEndTime: Date = new Date();
    private hotMessageModal: JQuery;

    mobileInit = () => {
        this.hotMessageModal = this.$('#userMessageModal');

        this.init();
    }

    private baseInit = this.init;
    onSuccessGetMessages = (rawResults: string) => {
        const results = rawResults;

        this.showPublicMessage(results);
        this.showPrivateMessage(results);
        this.showMoveBetsMessage(results);
    }

    showMoveBetsMessage = (results: any) => {
        const movebet = results.MoveBetsInformation;
        const movebetsMessage: string = movebet.Content || "",
            movebetsEndTime: Date = movebet.MoveBetsEndTime ? new Date(movebet.MoveBetsEndTime) : null;

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

        this.resizeOnHotMessageClose();
    }

    private resizeOnHotMessageClose = () => {
        this.hotMessageModal.on('click', () => {
            this.$('#toast-container').css('top', this.getHotMessageHeight());
        });
    }

    private getHotMessageHeight = (): number => {
        return this.hotMessageModal.is(':visible') ? (48 + this.hotMessageModal.height() + 2) : 48;
    }
}

$(() => {
    const isReadInstruction = alpha.localStorage.check(alpha.key.mobileInstruction);

    if (alpha.isScreenLower768 && isReadInstruction) {
        const headerMessageMobile = new HeaderMessageMobile($);
        headerMessageMobile.mobileInit();
    }
});