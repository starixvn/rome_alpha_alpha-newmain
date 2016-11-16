module Home {
    export class HeaderMessageMobile {
        private toastCloseButtonHtml = '<button type="button" class="icon icon-close" data-dismiss="modal" aria-label="Close"></button>';
        private storedMoveBetsEndTime: Date = new Date();
        private hotMessageModal: JQuery = $('#userMessageModal');

        init = () => {
            this.showMoveBetsMessagePopUp();
            setInterval(this.showMoveBetsMessagePopUp, 60000);
        }

        showMoveBetsMessagePopUp = () => {
            $.ajax({
                type: "POST",
                url: headerMessage.urls.getMessages,
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
            this.hotMessageModal.click(() => {
                $('#toast-container').css('top', this.getHotMessageHeight());
            });
        }
    }
}