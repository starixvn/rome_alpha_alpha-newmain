module Announcement {
    export class Mobile extends Base {
        private readMessageIdsKey: string = 'ReadMessageIds';

        startLoading = () => {
            this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds), this.onSuccessLoadAnnouncement);
        }

        init = () => {
            this.showRedCircleInIconAnnouncement(false);
            this.loadAnnouncementPeriodically(60000);
        }

        bindEvents = () => {
            this.bindCommonEvents();

            this.$("#prevButton").on('click', () => {
                this.loadPreviousAnnouncement();
            });

            this.$("#nextButton").on('click', () => {
                this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds, 1), this.onSuccessLoadNextAnnouncement);
            });

            this.registerAnnouncementModal();
        }

        showAnnouncement = (result: string) => {
            if (this.hasEmptyAnnouncementData(result)) {
                this.showEmptyAnnoucementNotification(this.$(result).val());
            }

            this.showAnnouncementBase(result);
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);

            this.checkNextAnnouncement((result) => {
                if (this.hasEmptyAnnouncementData(result)) {
                    this.showRedCircleInIconAnnouncement(false);
                }
            });

            this.saveReadMessageIdsToLocalStorage(this.showedMessageIds);
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
                this.loadAnnouncement(
                    this.createLoadAnnoucementData(this.getReadMessageIdsFromLocalStorage(), 1),
                    onSuccess);

            loadAnnouncement();
            setInterval(loadAnnouncement, interval);
        }

        private loadPreviousAnnouncement = () => {
            if (this.showedMessageIds.length >= 2) {
                this.showedMessageIds = this.showedMessageIds.slice(0, -2);
                this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds, 1), this.onSuccessLoadNextAnnouncement);
            }
        }

        private onSuccessLoadNextAnnouncement = (result: string) => {
            this.appendNewAnnouncementToPopup(result);

            this.checkNextAnnouncement((result) => {
                if (this.hasEmptyAnnouncementData(result)) {
                    this.showRedCircleInIconAnnouncement(false);
                }
            });

            this.saveReadMessageIdsToLocalStorage(this.showedMessageIds);
        }

        private appendNewAnnouncementToPopup = (result) => {
            var newAnnouncement = this.$("<div>" + result + "</div>");
            var newId = newAnnouncement.find("#popup-id").val();

            var text = result;
            if (this.hasEmptyAnnouncementData(text)) {
                return;
            }

            this.$('#announcementModalBody').empty().append(newAnnouncement.find("#announcementModalBody").html());
            this.$("#popup-id").val(newId);

            var popUp = this.domElements.popUpId;
            var id = popUp.length ? popUp.val() : 0;
            var popUpPanel = this.domElements.popUpPanel;

            if (!this.checkShowedMessage(id)) {
                this.showedMessageIds.push(id);
            }

            if (id < 0 && popUpPanel) {
                popUpPanel.css('display', 'none');
            }
        }

        private checkNextAnnouncement = (onSuccess: (result: string) => void) => {
            this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds, 1), onSuccess);
        }

        private saveReadMessageIdsToLocalStorage = (readMessageIds: Array<string>) => {
            let storedReadMessageIds: Array<string>
                = this.deduplicateArray(this.getReadMessageIdsFromLocalStorage().concat(readMessageIds));

            alpha.localStorage.set(this.generateReadMessageIdsKey(), storedReadMessageIds);
        }

        private getReadMessageIdsFromLocalStorage = (): Array<string> => {
            const readMessageIds = alpha.localStorage.get(this.generateReadMessageIdsKey());

            return (readMessageIds !== null && typeof readMessageIds !== "undefined") ? readMessageIds : [];
        }

        private generateReadMessageIdsKey(): string {
            const userId = $('#userId').val();
            const subUserId = $('#subUserId').val();

            return this.readMessageIdsKey + '-' + userId + '-' + subUserId;
        }

        private deduplicateArray = (array: Array<string>) => {
            return array.filter((value, index, arr) => {
                return arr.indexOf(value) === index;
            });
        }
    }
}