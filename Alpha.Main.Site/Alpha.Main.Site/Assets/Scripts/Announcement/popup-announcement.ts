module PopupAnnouncement {
    export class Base {
        protected $: JQueryStatic;
        protected announcementCookie: AnnouncementCounter;
        protected intervalAnnouncementId: number = 0;
        protected messageIndex = 0;
        protected constants = {
            maximumTimeToShowAnnouncementPopUp: 5,
            announcementsTransitionTime: 30000
        }
        protected domElements = {
            numberAnnouncement: 0,
            announcementId: null,
            popUpId: null,
            popUpPanel: null,
            popUpContainer: null,
            popUpContainerText: null,
            popUpContainerTextNoContent: null,
            announcementCookieId: null
        }

        constructor($: JQueryStatic, announcementCookie: AnnouncementCounter) {
            this.$ = $;
            this.announcementCookie = announcementCookie;
        }

        url = alpha.urls.messageRootPath + 'Announcement/PopupAnnouncement';

        init = () => {
            this.getAnnouncements(this.onSuccessLoadAnnouncement);
        }

        cacheElements = () => {
            this.domElements.numberAnnouncement = parseInt(this.$("#numberAnnouncement").val());
            this.domElements.announcementId = this.$('#announcement-id');
            this.domElements.popUpId = this.$("#popup-id");
            this.domElements.popUpPanel = this.$("#popup_panel");
            this.domElements.popUpContainer = this.$('#popup-container');
            this.domElements.popUpContainerText = this.$('#popup-container-text');
            this.domElements.popUpContainerTextNoContent = this.$('#popup-container-text-no-content');
            this.domElements.announcementCookieId = this.$("#announcement-cookies-id");
        }

        bindEvents = () => {
            this.bindCommonEvents();
        }

        bindCommonEvents = () => {
            this.domElements.popUpContainerText.find('.icon-close').on('click', () => {
                clearInterval(this.intervalAnnouncementId);
            });

            this.domElements.popUpContainer.find('.icon-close').on('click', () => {
                clearInterval(this.intervalAnnouncementId);
            });
        }

        getAnnouncements = (onSuccess: (result: string) => void) => {
            const ajaxSettings = this.createAjaxSettings('POST', this.url, onSuccess);

            this.$.ajax(ajaxSettings);
        }

        showAnnouncement = (result: string) => {
            if (this.hasEmptyAnnouncementData(result)) {
                return;
            }

            this.showAnnouncementBase(result);
        }

        showAnnouncementBase = (result: string) => {
            var div = document.createElement('div');
            div.innerHTML = result;
            div.setAttribute('id', 'announcement-id');
            document.body.appendChild(div);

            this.messageIndex = 1;
            this.cacheElements();
            this.announcementCookie.init(this.domElements.announcementCookieId.val());
            this.bindEvents();

            this.displayAnnouncementModal(this.messageIndex);

            this.intervalAnnouncementId = setInterval(this.registerFetchNextAnnouncement, this.constants.announcementsTransitionTime);
        }

        registerFetchNextAnnouncement = () => {
            this.hiddenAnnouncementModal(this.messageIndex);

            if (this.domElements.numberAnnouncement > 0 &&
                this.messageIndex < this.domElements.numberAnnouncement) {
                this.messageIndex++;
                this.displayAnnouncementModal(this.messageIndex);
            } else if (this.messageIndex >= this.domElements.numberAnnouncement) {
                clearInterval(this.intervalAnnouncementId);
            }
        }

        increaseCounter = (id: number) => {
            var announcementId = this.$("#announcement_" + id).find("#AnnouncementId").val();
            this.announcementCookie.increaseCounter(announcementId);
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);
        }

        hasEmptyAnnouncementData = (html: any): boolean => {
            const id: string = this.$(html).attr('id');

            return id === "no-info";
        }

        displayAnnouncementModal = (id: number) => {
            var isExcess = this.announcementCookie.isExccess(id.toString());
            if (!isExcess) {
                this.$("#announcement_" + id).find(".list-action").removeClass('hidden');
                var popUpContainer = this.$("#announcement_" + id).find(".announcement-modal");
                popUpContainer.removeClass('hidden');
                popUpContainer
                    .modal({ show: true, backdrop: false })
                    .tinyDraggable({ exclude: 'button.icon.icon-close.close, .modal-body, p, a' });

                this.increaseCounter(id);
            }
        }

        hiddenAnnouncementModal = (id: number) => {
            this.$("#announcement_" + id).find(".announcement-modal").addClass('hidden');
            this.$("#announcement_" + id).find(".list-action").addClass('hidden');
        }

        createAjaxSettings = (methodType: string, url: string, onSuccess: any): JQueryAjaxSettings => {
            let ajaxSettings: JQueryAjaxSettings = {
                type: methodType,
                url: url,
                data: null,
                cache: false,
                success: onSuccess
            };

            return ajaxSettings;
        }
    }
}



$(document).ready(() => {

    let counter = new PopupAnnouncement.AnnouncementCounter($);
    if (alpha.isScreenLower768) {
        const mobileAnnouncement = new PopupAnnouncement.Mobile($, counter);
        mobileAnnouncement.init();
    } else {
        const announcement = new PopupAnnouncement.Base($, counter);
        announcement.init();
    }      
})