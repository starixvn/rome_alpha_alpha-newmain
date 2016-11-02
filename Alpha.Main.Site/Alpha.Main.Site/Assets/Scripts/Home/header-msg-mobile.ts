module Home {
    export class HeaderMessageMobile {
        private storedMoveBetsEndTime: Date = new Date();

        init = () => {
            this.showMoveBetsMessagePopUp();
            setInterval(this.showMoveBetsMessagePopUp, 5000);
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
            toastr.options.closeHtml = '<button type="button" class="icon icon-close" data-dismiss="modal" aria-label="Close"></button>';
            toastr.options.timeOut = 10000;
            toastr.options.preventDuplicates = true;
            toastr.warning(message).css('top', this.getHotMessageHeight() + 'px');
        }

        private getHotMessageHeight = (): number => {
            const hotMessage = $('#userMessageModal');

            if (hotMessage.is(':visible')) {
                return hotMessage.height();
            }

            return 0;
        }
    }
}