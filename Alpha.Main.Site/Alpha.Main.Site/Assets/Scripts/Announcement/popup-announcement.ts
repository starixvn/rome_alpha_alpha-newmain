module PopupAnnouncement {
    export class Base {
        url = alpha.urls.messageRootPath + 'Announcement/PopupAnnouncement';
        protected $: JQueryStatic;
        protected eventIdOfLoadNewAnnouncement: number = 0;
        protected announcementSessionKey: string = "AnnouncementIds";
        protected constants = {
            timeToRemoveAnnouncement: 120000,
            timeToCheckNewAnnouncement: 30000
        };
        protected domElements = {
            announcementModal: null,
            announcementIframe: null
        };

        constructor($: JQueryStatic) {
            this.$ = $;
        }

        init = () => {
            this.removeAnnouncementSession();
            this.showAnnouncement();
        };

        removeAnnouncementSession = () => {
            sessionStorage.removeItem(this.announcementSessionKey);
        };

        showAnnouncement = () => {
            this.cacheElements();
            this.setIframeSrc();
            this.eventIdOfLoadNewAnnouncement = setTimeout(this.removeCurrentAnnouncement, this.constants.timeToRemoveAnnouncement);
            this.bindCommonEvents();
        };

        cacheElements = () => {
            this.domElements.announcementModal = this.$('#announcement-modal');
            this.domElements.announcementIframe = this.$('#annoucement-iframe');
        };

        setIframeSrc = () => {
            var iframe = this.domElements.announcementIframe;
            iframe.attr("src", this.url);
        };

        removeCurrentAnnouncement = () => {
            this.hiddenAnnouncementModal();
            if (!alpha.isScreenLower768) {
                setTimeout(() => { this.showAnnouncement() }, this.constants.timeToCheckNewAnnouncement);
            }
        };

        bindCommonEvents = () => {
            this.domElements.announcementModal.find('.icon-close').on('click', () => {
                clearTimeout(this.eventIdOfLoadNewAnnouncement);
                this.removeCurrentAnnouncement();
            });
        };

        hiddenAnnouncementModal = () => {
            this.domElements.announcementModal.addClass('hidden');
        };
    }
}

$(document).ready(() => {
    if ($("#disableMessages").val() == "1") {
        return;
    }

    if (alpha.isScreenLower768) {
        const mobileAnnouncement = new PopupAnnouncement.Mobile($);
        mobileAnnouncement.init();
    } else {
        const announcement = new PopupAnnouncement.Base($);
        announcement.init();
    }
})