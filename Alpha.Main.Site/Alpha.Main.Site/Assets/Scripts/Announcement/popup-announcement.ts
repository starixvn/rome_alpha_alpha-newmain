module PopupAnnouncement {
    export class Base {
        protected $: JQueryStatic;
        protected announcementCookie: AnnouncementCounter;
        protected eventIdOfLoadNewAnnouncement: number = 0;
        protected shownMessageIds: Array<string> = [];
        protected selectedMessageId: string = "";
        protected constants = {
            timeToRemoveAnnouncement: 12000,
            timeToCheckNewAnnouncement: 3000
        }
        protected domElements = {
            announcementSections: null,
            popUpContainer: null,
            popUpContainerText: null,
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
            this.domElements.announcementSections = this.$('.js-announcement');
            this.domElements.popUpContainer = this.$('.popup-container');
            this.domElements.popUpContainerText = this.$('.popup-container-text');
            this.domElements.announcementCookieId = this.$("#announcement-cookies-id");
        }

        bindEvents = () => {
            this.bindCommonEvents();
        }

        bindCommonEvents = () => {
            this.domElements.popUpContainerText.find('.icon-close').on('click', () => {
                clearTimeout(this.eventIdOfLoadNewAnnouncement);
                this.removeCurrentAnnouncement();
            });

            this.domElements.popUpContainer.find('.icon-close').on('click', () => {
                clearTimeout(this.eventIdOfLoadNewAnnouncement);
                this.removeCurrentAnnouncement();
            });
        }

        getAnnouncements = (onSuccess: (result: string) => void) => {
            const ajaxSettings = this.createAjaxSettings('POST', this.url, onSuccess);

            this.$.ajax(ajaxSettings);
        }

        showAnnouncement = (result: string) => {
            this.appendAnnouncementSection(result);
            this.cacheElements();
            this.announcementCookie.init(this.domElements.announcementCookieId.val());

            this.eventIdOfLoadNewAnnouncement = setTimeout(this.removeCurrentAnnouncement, this.constants.timeToRemoveAnnouncement);

            if (this.hasEmptyAnnouncementData(result)) {
                return;
            }

            this.showAnnouncementBase(result);            
        }

        appendAnnouncementSection = (result: string) => {
            this.$('#announcement-id').remove();

            var div = document.createElement('div');
            div.innerHTML = result;
            div.setAttribute('id', 'announcement-id');
            document.body.appendChild(div);
        }

        getSelectedMessageId = () => {
            for (var i = 0; i < this.domElements.announcementSections.length; i++) {
                var id = this.$(this.domElements.announcementSections[i]).find('#AnnouncementId').val();
                if (this.shownMessageIds.indexOf(id) == -1) {
                    this.shownMessageIds.push(id);
                    this.selectedMessageId = id;
                    return;
                }
            }

            this.selectedMessageId = "";
        };

        showAnnouncementBase = (result: string) => {
            this.getSelectedMessageId();
            this.bindEvents();

            this.displayAnnouncementModal(this.selectedMessageId);
        }

        removeCurrentAnnouncement = () => {
            this.hiddenAnnouncementModal(this.selectedMessageId);

            setTimeout(() => { this.getAnnouncements(this.onSuccessLoadAnnouncement) }, this.constants.timeToCheckNewAnnouncement);
        }

        increaseCounter = (id: string) => {
            var announcementId = this.$("#announcement_" + id).find("#AnnouncementId").val();
            this.announcementCookie.increaseCounter(announcementId);
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);
        }

        hasEmptyAnnouncementData = (html: any): boolean => {
            var isEmpty = true;
            for (var i = 0; i < this.domElements.announcementSections.length; i++) {
                var announcementId = this.$(this.domElements.announcementSections[i]).find('#AnnouncementId').val();
                var isExcess = this.announcementCookie.isExcess(announcementId);
                if (!isExcess) {
                    isEmpty = false;
                }
            }
            return isEmpty;
        }

        displayAnnouncementModal = (id: string) => {
            var isExcess = this.announcementCookie.isExcess(id);
            if (!isExcess) {
                var announcement = this.$("#announcement_" + id);
                announcement.find(".list-action").removeClass('hidden');
                var popUpContainer = announcement.find(".announcement-modal");
                popUpContainer.removeClass('hidden');
                popUpContainer
                    .modal({ show: true, backdrop: false })
                    .tinyDraggable({ exclude: 'button.icon.icon-close.close, .modal-body, p, a' });

                this.increaseCounter(id);
            }
        }

        hiddenAnnouncementModal = (id: string) => {
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