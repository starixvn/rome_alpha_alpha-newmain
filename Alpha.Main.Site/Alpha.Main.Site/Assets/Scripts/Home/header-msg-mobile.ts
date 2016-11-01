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
            var results = alpha.core.convertReponseToJson(rawResults);
            var movebetsMessage: string = results.movebets_message || "",
                movebetsEndTime: Date = results.movebets_endtime ? new Date(results.movebets_endtime) : null;

            if (movebetsEndTime !== null && movebetsEndTime > this.storedMoveBetsEndTime) {
                this.showMoveBetsMessageToaster(movebetsMessage);
                this.storedMoveBetsEndTime = movebetsEndTime;
            }
        }

        private showMoveBetsMessageToaster = (message: string) => {
            toastr.options.positionClass = 'toast-top-full-width toastr-announcement';
            toastr.options.closeButton = true;
            toastr.options.timeOut = 11115000;
            toastr.options.preventDuplicates = true;
            toastr.warning(message);
        }
    }
}