var PopupAnnouncement = /** @class */ (function () {
    function PopupAnnouncement($) {
        var _this = this;
        this.url = alpha.urls.messageRootPath + 'Announcement/Popup';
        this.init = function () {
            var iframe = _this.$('.announcement-modal iframe');
            iframe.attr("src", _this.url);
        };
        this.$ = $;
    }
    return PopupAnnouncement;
}());
$(document).ready(function () {
    if ($("#disableMessages").val() == "1") {
        return;
    }
    var announcement = new PopupAnnouncement($);
    announcement.init();
});
//# sourceMappingURL=popup-announcement.js.map