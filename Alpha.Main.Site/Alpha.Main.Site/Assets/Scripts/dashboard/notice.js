/// <reference path="../core.ts" />
var NoticeMessage = (function () {
    function NoticeMessage() {
        var _this = this;
        this.registerEvent = function () {
            _this.onToggleMessage(_this.markMessageAsRead);
            _this.onToggleShowMoreMessages();
            _this.onSwipeInMobileView();
        };
        this.onToggleMessage = function (markMessageAsRead) {
            $('.msg').each(function () {
                var $this = $(this), $shortContent = $this.children('.short-content'), $fullContent = $this.children('.full-content'), $icon = $this.find('.icon');
                $this.click(function () {
                    if (!$shortContent.is(':hidden')) {
                        $shortContent.hide();
                        $fullContent.show();
                        $icon.toggleClass('icon-plus icon-minus');
                        markMessageAsRead($this);
                    }
                    else {
                        $shortContent.show();
                        $fullContent.hide();
                        $icon.toggleClass('icon-minus icon-plus');
                    }
                });
            });
        };
        this.markMessageAsRead = function (message) {
            var isUnRead = message.hasClass('bold');
            if (isUnRead) {
                var onSuccess = function (isMarkedSuccess) {
                    if (isMarkedSuccess) {
                        message.removeClass('bold');
                    }
                };
                var ajaxSetting = {
                    url: '/site-main/Notice/MarkMessageAsRead',
                    data: { messageId: message.attr('id') },
                    method: 'POST',
                    success: onSuccess
                };
                $.ajax(ajaxSetting);
            }
        };
        this.onToggleShowMoreMessages = function () {
            var $moreMessagesBtn = $('.more-info');
            $moreMessagesBtn.click(function () {
                var $content = $('#notice');
                var onSuccess = function (htmlString) {
                    $content.html(htmlString);
                    new NoticeMessage();
                };
                var ajaxSetting = {
                    url: alpha.urls.mainRootPath + '/Notice/MoreMessages?totalMessage=' + $moreMessagesBtn.attr('totalMsg'),
                    method: 'GET',
                    success: onSuccess
                };
                $moreMessagesBtn.attr('disabled', 'disabled');
                $moreMessagesBtn.text('Loading...');
                $.ajax(ajaxSetting);
            });
        };
        this.onSwipeInMobileView = function () {
            var stats = document.getElementById('stats'), statsTab = document.getElementById('stats-tab'), notice = document.getElementById('notice'), noticeTab = document.getElementById('notice-tab');
            var noticeHM = new Hammer(notice);
            noticeHM.on("swiperight", function (ev) {
                notice.classList.remove('active');
                noticeTab.classList.remove('active');
                stats.classList.add('active');
                statsTab.classList.add('active');
            });
        };
    }
    return NoticeMessage;
}());
