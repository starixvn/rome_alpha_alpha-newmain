class PopupAnnouncement {
        url = alpha.urls.messageRootPath + 'Announcement/Popup';
        protected $: JQueryStatic;    

        constructor($: JQueryStatic) {
            this.$ = $;
        }

        init = () => {
            var iframe = this.$('.announcement-modal iframe');
            iframe.attr("src", this.url);
        };          
    }

$(document).ready(() => {
    if ($("#disableMessages").val() == "1") {
        return;
    }

    const announcement = new PopupAnnouncement($);
    announcement.init();
})