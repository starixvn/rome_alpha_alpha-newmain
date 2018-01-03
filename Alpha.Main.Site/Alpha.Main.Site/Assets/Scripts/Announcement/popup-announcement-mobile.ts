module PopupAnnouncement {
    export class Mobile extends Base {
        startLoading = () => {
            this.removeAnnouncementSession();
            this.showAnnouncement();
        }

        init = () => {
            this.removeAnnouncementSession();
            this.showAnnouncement();
        }

        showAnnouncement = () => {
            this.cacheElements();
            this.setIframeSrc();
            this.bindCommonEvents();
        }
    }
}