var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var NoticeMessage = /** @class */ (function (_super) {
    __extends(NoticeMessage, _super);
    function NoticeMessage() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.registerEvents = function () {
            _this.onToggleMessage(_this.markMessageAsRead);
            _this.onToggleShowMoreMessages();
            _this.hideMoreBtnIfHaveNoData();
        };
        _this.onToggleMessage = function (markMessageAsRead) {
            $('.message').each(function (index, element) {
                var $this = _this.$(element), shortContent = $this.children('.short-content'), fullContent = $this.children('.full-content'), icon = $this.find('.icon');
                $this.on('click', function () {
                    if (!shortContent.is(':hidden')) {
                        shortContent.hide();
                        fullContent.show();
                        icon.toggleClass('icon-plus icon-minus');
                        markMessageAsRead($this);
                    }
                    else {
                        shortContent.show();
                        fullContent.hide();
                        icon.toggleClass('icon-minus icon-plus');
                    }
                });
            });
        };
        _this.markMessageAsRead = function (message) {
            var isUnRead = message.hasClass('bold');
            if (isUnRead) {
                var onSuccess = function (isMarkedSuccess) {
                    if (isMarkedSuccess) {
                        message.removeClass('bold');
                    }
                };
                var ajaxSetting = {
                    url: alpha.urls.messageRootPath + 'PortalMessage/MarkMessageAsRead',
                    data: { messageId: message.attr('id') },
                    method: 'POST',
                    success: onSuccess
                };
                _this.$.ajax(ajaxSetting);
            }
        };
        _this.onToggleShowMoreMessages = function () {
            var moreMessagesBtn = _this.$('.more-info');
            moreMessagesBtn.on('click', function () {
                var content = _this.$('#notice');
                var onSuccess = function (htmlString) {
                    content.html(htmlString);
                    _this.registerEvents();
                };
                var ajaxSetting = {
                    url: alpha.urls.messageRootPath + 'PortalMessage/MoreMessages?totalMessage=' + moreMessagesBtn.attr('totalMsg'),
                    method: 'GET',
                    cache: false,
                    success: onSuccess
                };
                moreMessagesBtn.attr('disabled', 'disabled');
                moreMessagesBtn.html('<span class="block-icon-small"></span>');
                _this.$.ajax(ajaxSetting);
            });
        };
        _this.hideMoreBtnIfHaveNoData = function () {
            var noInfo = document.getElementById('no-info'), messageAmount = _this.$('.msg-contents').length, moreTotalMessage = Number($('.more-info').attr('totalMsg'));
            if (noInfo !== null || moreTotalMessage > (messageAmount + 10)) {
                _this.$('.more-info').hide();
            }
        };
        return _this;
    }
    return NoticeMessage;
}(DashboardBase));
$(function () {
    var notice = new NoticeMessage($);
    notice.loadInfo('#notice', notice.registerEvents);
});
//# sourceMappingURL=notice.js.map