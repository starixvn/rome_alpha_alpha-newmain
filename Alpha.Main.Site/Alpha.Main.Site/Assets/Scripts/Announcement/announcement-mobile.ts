module Announcement {
    export class Mobile extends Base {
        startLoading = () => {
            this.getAnnouncements(this.onSuccessLoadAnnouncement);
        }

        init = () => {
            this.showRedCircleInIconAnnouncement(false);
            this.getAnnouncements(this.onSuccessLoadAnnouncement);
        }

        bindEvents = () => {
            this.bindCommonEvents();

            this.$("#prevButton").on('click', () => {
                this.registerFetchNextAnnouncement();
            });

            this.$("#nextButton").on('click', () => {
                this.registerFetchPrevAnnouncement
            });
        }

        registerFetchPrevAnnouncement = () => {
            this.hiddenAnnouncementModal(this.messageIndex);

            if (this.domElements.numberAnnouncement > 0 &&
                this.messageIndex > 1) {
                this.messageIndex--;
                this.displayAnnouncementModal(this.messageIndex);
            } else if (this.messageIndex == 0) {
                clearInterval(this.intervalAnnouncementId);
            }
        }

        showAnnouncement = (result: string) => {
            if (this.hasEmptyAnnouncementData(result)) {
                this.showEmptyAnnoucementNotification(this.$(result).val());
                this.showRedCircleInIconAnnouncement(false);
            }else{
                this.showRedCircleInIconAnnouncement();
            }

            this.showAnnouncementBase(result);
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);
        }

        private showRedCircleInIconAnnouncement = (isShow: boolean = true) => {
            if (isShow) {
                this.$('.icon-bell').find('.circle-red').show();
            } else {
                this.$('.icon-bell').find('.circle-red').hide();
            }
        }

        private showEmptyAnnoucementNotification = (text: string) => {
            toastr.options.positionClass = 'toast-top-full-width';
            toastr.options.timeOut = 2000;
            toastr.options.preventDuplicates = true;
            toastr.info(text);
        }

        private loadAnnouncementPeriodically = (interval: number) => {
            const onSuccess = (result: string) => {
                if (this.hasEmptyAnnouncementData(result)) {
                    this.showRedCircleInIconAnnouncement(false);
                } else {
                    this.showRedCircleInIconAnnouncement();
                }
            }

            const loadAnnouncement = () =>
                this.getAnnouncements(onSuccess);

            loadAnnouncement();
            setInterval(loadAnnouncement, interval);
        }       
    }
}