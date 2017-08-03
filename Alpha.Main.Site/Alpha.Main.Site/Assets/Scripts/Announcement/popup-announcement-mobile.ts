module PopupAnnouncement {
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

            this.$(".prevButton").on('click', () => {
                this.goToPrevAnnouncement();
            });

            this.$(".nextButton").on('click', () => {
                this.goToNextAnnouncement();
            });
        }

        goToNextAnnouncement = () => {
            this.hiddenAnnouncementModal(this.selectedMessageId);

            var nextAnnouncement = this.$('#announcement_' + this.selectedMessageId).next();
            if (nextAnnouncement != undefined) {
                var id = this.$(nextAnnouncement).find('#AnnouncementId').val();                
                this.selectedMessageId = id;
                if (this.shownMessageIds.indexOf(id) == -1) {
                    this.shownMessageIds.push(id);
                }

                this.displayAnnouncementModal(this.selectedMessageId);
            }
        }

        goToPrevAnnouncement = () => {
            this.hiddenAnnouncementModal(this.selectedMessageId);
            var prevAnnouncement = this.$('#announcement_' + this.selectedMessageId).prev();
            if (prevAnnouncement != undefined) {
                var id = this.$(prevAnnouncement).find('#AnnouncementId').val();                
                this.selectedMessageId = id;
                if (this.shownMessageIds.indexOf(id) == -1) {
                    this.shownMessageIds.push(id);
                }

                this.displayAnnouncementModal(this.selectedMessageId);
            }
        }        

        showAnnouncement = (result: string) => {
            this.appendAnnouncementSection(result);
            this.cacheElements();
            this.announcementCookie.init(this.domElements.announcementCookieId.val());
            if (this.hasEmptyAnnouncementData(result)) {
                this.showEmptyAnnoucementNotification(this.$('#no-info').val());
                this.showRedCircleInIconAnnouncement(false);
            } else {
                this.showRedCircleInIconAnnouncement();
                this.showAnnouncementBase(result);
            }
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
    }
}