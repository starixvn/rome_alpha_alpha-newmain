module Announcement {
    interface ILoadAnnouncementData {
        messageIds: string;
        announcementNext: number;
    }

    export class Base {
        protected $: JQueryStatic;
        protected showedMessageIds: Array<string> = [];
        protected securityCodeId: string = '20130426,20130526';
        protected timeoutAnnouncement: number = 0;
        protected announcementCookieName: string = '';
        protected constants = {
            maximumTimeToShowAnnouncementPopUp: 5,
            normalExpiredDays: 30,
            securityCodeAnnouncementExpiredDay: 1,
            timeToRemoveAnnouncement: 120000,
            announcementsTransitionTime: 30000
        }
        protected domElements = {
            announcementId: null,
            popUpId: null,
            popUpPanel: null,
            popUpContainer: null,
            popUpContainerText: null,
            popUpContainerTextNoContent: null,
            hideAdvertisement: null,
            announcementCookieId: null
        }

        constructor($: JQueryStatic) {
            this.$ = $;
        }

        url = alpha.urls.mainRootPath + 'Announcement/HotAnnouncement';

        init = () => {
            this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds), this.onSuccessLoadAnnouncement);
        }

        cacheElements = () => {
            this.domElements.announcementId = this.$('#announcement-id');
            this.domElements.popUpId = this.$("#popup-id");
            this.domElements.popUpPanel = this.$("#popup_panel");
            this.domElements.popUpContainer = this.$('#popup-container');
            this.domElements.popUpContainerText = this.$('#popup-container-text');
            this.domElements.popUpContainerTextNoContent = this.$('#popup-container-text-no-content');
            this.domElements.hideAdvertisement = this.$("#popup-container #chkHide");
            this.domElements.announcementCookieId = this.$("#announcement-cookies-id");
        }

        bindEvents = () => {
            this.bindCommonEvents();

            setTimeout(() => {
                this.registerAnnouncementModal();
            }, 2000);
        }

        bindCommonEvents = () => {
            this.domElements.popUpContainerText.find('.icon-close').on('click', () => {
                this.removeAnnouncement();
            });

            this.domElements.popUpContainer.find('.icon-close').on('click', () => {
                parent['announcement'].removeAnnouncement();
            });

            this.domElements.hideAdvertisement.on('click', () => {
                this.doNotShowAnnouncement(this.$(this).is(':checked'));
            });
        }

        loadAnnouncement = (data: ILoadAnnouncementData, onSuccess: (result: string) => void) => {
            const ajaxSettings = this.createAjaxSettings('POST', this.url, data, onSuccess);

            this.$.ajax(ajaxSettings);
        }

        showAnnouncement = (result: string) => {
            if (this.hasEmptyAnnouncementData(result)) {
                return;
            }

            this.showAnnouncementBase(result);
        }

        showAnnouncementBase = (result: string) => {
            this.removeAnnouncement();

            var div = document.createElement('div');
            div.innerHTML = result;
            div.setAttribute('id', 'announcement-id');
            document.body.appendChild(div);

            this.cacheElements();
            this.announcementCookieName = this.getAnnouncementCookieName();

            var popUp = this.domElements.popUpId;
            var id = popUp.length ? popUp.val() : 0;

            if (!this.checkShowedMessage(id)) {
                this.showedMessageIds.push(id);
            }

            if (!alpha.isScreenLower768) {
                var count = this.getAnnouncementShowCount();
                if (count > this.constants.maximumTimeToShowAnnouncementPopUp) {
                    setTimeout(this.removeAnnouncement, 1000);
                    return;
                }

                this.increaseAnnouncementShowTimes();
            }

            var popUpPanel = this.domElements.popUpPanel;
            if (id < 0 && popUpPanel) {
                popUpPanel.css('display', 'none');
            }

            this.bindEvents();
        }

        removeAnnouncement = () => {
            var announcementId = this.domElements.announcementId;
            if (announcementId != null) {
                announcementId.remove();
                clearTimeout(this.timeoutAnnouncement);

                if (!alpha.isScreenLower768) {
                    setTimeout(() => {
                        this.loadAnnouncement(this.createLoadAnnoucementData(this.showedMessageIds, 1), this.onSuccessLoadAnnouncement);
                    }, this.constants.announcementsTransitionTime);
                }
            }
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);

            this.timeoutAnnouncement = setTimeout(this.removeAnnouncement, this.constants.timeToRemoveAnnouncement);
        }

        hasEmptyAnnouncementData = (html: any): boolean => {
            const id: string = this.$(html).attr('id');

            return id === "no-info";
        }

        checkShowedMessage = (id) => {
            const count = this.showedMessageIds.length;

            for (let i = 0; i < count; i++) {
                if (id === this.showedMessageIds[i]) {
                    return true;
                }
            }

            return false;
        }

        registerAnnouncementModal = () => {
            var popUpContainer = this.domElements.popUpContainerText.length
                ? this.domElements.popUpContainerText
                : this.domElements.popUpContainer;

            popUpContainer
                .modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, .modal-body, p' });
        }

        createAjaxSettings = (methodType: string, url: string, data: any, onSuccess: any): JQueryAjaxSettings => {
            let ajaxSettings: JQueryAjaxSettings = {
                type: methodType,
                url: url,
                data: data,
                success: onSuccess
            };

            return ajaxSettings;
        }

        createLoadAnnoucementData = (messageIds: Array<string>, announcementNext: number = 0): ILoadAnnouncementData => {
            return {
                messageIds: messageIds.join(','),
                announcementNext: announcementNext
            }
        }

        private doNotShowAnnouncement = (hide) => {
            this.$.cookie(
                this.announcementCookieName,
                hide ? this.constants.maximumTimeToShowAnnouncementPopUp : 1,
                { expires: this.constants.normalExpiredDays, path: '/' });
        }

        private increaseAnnouncementShowTimes = () => {
            var count = this.getAnnouncementShowCount();
            var popUpId = this.domElements.popUpId;
            var cookieDays = this.securityCodeId.indexOf(popUpId.val()) == -1
                ? this.constants.normalExpiredDays
                : this.constants.securityCodeAnnouncementExpiredDay;
            this.$.cookie(this.announcementCookieName, ++count, { expires: cookieDays, path: '/' });
        }

        private getAnnouncementShowCount = (): number => {
            var cookieName = this.announcementCookieName;

            if (cookieName == null) {
                return 0;
            }

            var value = this.$.cookie(cookieName);
            var count = parseInt(value, 10);

            if (isNaN(count)) {
                count = 0;
            }

            return count;
        }

        private getAnnouncementCookieName = () => {
            try {
                const popUpId = this.domElements.popUpId;

                if (popUpId.length === 0) {
                    return null;
                }

                if (parseInt(popUpId.val()) < 0) {
                    return null;
                }

                return "-" + popUpId.val() + this.domElements.announcementCookieId.val();
            } catch (e) {
                return null;
            }
        }
    }
}

$(document).ready(() => {
    if (alpha.isScreenLower768) {
        const mobileAnnouncement = new Announcement.Mobile($);
        mobileAnnouncement.init();
    } else {
        const announcement = new Announcement.Base($);
        announcement.init();
    }
})