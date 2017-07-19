module HeaderAnnouncement {
    export class Base {
        protected $: JQueryStatic;
        protected domElements = {
            hotMessages: null,
            userMessageModal: null,
            currentHotMessage: null,
            scroller: null,
            contentMessage: null,
            showMoreContainer: null,
            showLessContainer: null
        }

        constructor($: JQueryStatic) {
            this.$ = $;
        }

        url = alpha.urls.messageRootPath + 'Announcement/HeaderAnnouncement';

        init = () => {
            this.getAnnouncements(this.onSuccessLoadAnnouncement);
        }

        cacheElements = () => {
            this.domElements.hotMessages = this.$('#userMessageModalBody');
            this.domElements.contentMessage = this.$('#msg-content');
            this.domElements.userMessageModal = this.$('#userMessageModal');
            this.domElements.currentHotMessage = this.domElements.hotMessages.find('p.current-hot-message');
            this.domElements.scroller = this.$('#scroller');
            this.domElements.showMoreContainer = this.$(".showmore-container");
            this.domElements.showLessContainer = this.$(".showless-container");
        }

        bindEvents = () => {
            this.domElements.showMoreContainer.on('click', this.showMoreHotMessage);
            this.domElements.showLessContainer.on('click', this.showLessHotMessage);

        }

        showMoreHotMessage = () => {
            this.calculateHotMessagePopupHeight(true);
        }

        showLessHotMessage = () => {
            this.calculateHotMessagePopupHeight(false);
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
            div.setAttribute('id', 'headerAnnouncement-id');
            document.body.appendChild(div);

            this.cacheElements();
            this.bindEvents();

            this.displayAnnouncementModal();
        }

        onSuccessLoadAnnouncement = (result: string) => {
            this.showAnnouncement(result);
        }

        hasEmptyAnnouncementData = (html: any): boolean => {
            const id: string = this.$(html).attr('id');

            return id === "no-info";
        }

        displayAnnouncementModal = () => {
            var popUpContainer = this.domElements.userMessageModal
                .modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, a' });
            this.calculateHotMessagePopupHeight(false);
        }

        calculateHotMessagePopupHeight = (showFullMessage) => {
            this.resetHotMessageElements(showFullMessage);

            const maxMessageHeight = 53;
            const messageHeight = this.calculateHotMessageHeight(showFullMessage, maxMessageHeight);
            const headerHeight = 10;
            const popUpHeight = headerHeight + messageHeight;

            this.domElements.userMessageModal.height(popUpHeight);
        };

        setHotMessageElementStyleByMessageHeight = (showFullMessage, messageHeight, maxMessageHeight) => {
            if (showFullMessage) {
                if (messageHeight <= maxMessageHeight) {
                    this.domElements.showLessContainer.addClass("hide");
                } else {
                    this.domElements.showLessContainer.removeClass("hide");
                }
            }
            else {
                if (messageHeight <= maxMessageHeight) {
                    this.domElements.showMoreContainer.addClass("hide");
                } else {
                    this.domElements.currentHotMessage.addClass("brief-content").height(maxMessageHeight);
                    this.domElements.showMoreContainer.removeClass("hide");
                }
            }
        }

        calculateHotMessageHeight = (showFullMessage, maxMessageHeight) => {
            const bodyBorderHeight = 2;
            const bodyPaddingHeight = 5 + bodyBorderHeight;
            let messageHeight = this.domElements.currentHotMessage.height();

            this.setHotMessageElementStyleByMessageHeight(showFullMessage, messageHeight, maxMessageHeight);

            if (maxMessageHeight < messageHeight) {
                messageHeight = showFullMessage ?
                    messageHeight + this.domElements.showLessContainer.height() :
                    maxMessageHeight + this.domElements.showMoreContainer.height();
            }

            messageHeight += bodyPaddingHeight;

            return messageHeight;
        }

        resetHotMessageElements = (showFullMessage) => {
            const minMessageHeight = 40;
            this.domElements.currentHotMessage.css('min-height', minMessageHeight);

            if (showFullMessage) {
                this.domElements.showMoreContainer.addClass("hide");
                this.domElements.currentHotMessage.removeClass("brief-content");
                this.domElements.currentHotMessage.css('height', '');
            } else {
                this.domElements.showLessContainer.addClass("hide");
                this.domElements.currentHotMessage.removeClass("brief-content");
            }
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