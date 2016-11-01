module Home {
    export class HeaderMessageMobile {
        init = () => {
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
                today: Date = new Date(),
                movebetsEndTime: Date = results.movebets_endtime ? new Date(results.movebets_endtime) : today;

            if (movebetsEndTime > today) {
                alpha.localStorage.set('movebets_endtime', movebetsEndTime);
                this.showMoveBetsMessageToaster(movebetsMessage);

                //setInterval(this.showMoveBetsMessagePopUp, movebetsEndTime.getTime() - today.getTime());
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