module HeaderAnnouncement {
    export class Base {
        protected $: JQueryStatic;
        protected domElements = {
            contentMessage: null
        }

        constructor($: JQueryStatic) {
            this.$ = $;
        }

        url = alpha.urls.messageRootPath + 'Announcement/HeaderAnnouncement';

        init = () => {
            this.getAnnouncements(this.onSuccessLoadAnnouncement);
        }

        cacheElements = () => {
            this.domElements.contentMessage = this.$('#msg-content');
        }       

        getAnnouncements = (onSuccess: (result: string) => void) => {
            const ajaxSettings = this.createAjaxSettings('POST', this.url, onSuccess);

            this.$.ajax(ajaxSettings);
        }

        showAnnouncement = (result: string) => {
            if (this.hasEmptyAnnouncementData(result)) {
                return;
            }

            this.cacheElements();
            this.appendMessageToPopup(result);
        }        

        appendMessageToPopup = (message) => {
            this.domElements.contentMessage.append(
                 message);
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);
        }

        hasEmptyAnnouncementData = (html: any): boolean => {
            const id: string = this.$(html).attr('id');

            return id === "no-info";
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
    const announcement = new HeaderAnnouncement.Base($);
    announcement.init();
})