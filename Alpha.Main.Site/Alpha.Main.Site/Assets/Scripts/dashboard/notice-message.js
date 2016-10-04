var NoticeMessage = (function () {
    function NoticeMessage() {
        this.onClickMessageRegister = function (markMessageAsRead) {
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
            var isRead = message.hasClass('bold');
            var onSuccess = function (isMarkedSuccess) {
                if (isMarkedSuccess) {
                    message.removeClass('bold');
                }
            };
            var ajaxSetting = {
                url: '/site-main/Notice/MarkMessageAsRead',
                data: { messageIds: message.attr('id') },
                method: 'POST',
                success: onSuccess
            };
            $.ajax(ajaxSetting);
        };
        this.showMoreMessagesRegister = function () {
            $('.more-info').click(function () {
                var $this = $('.more-info');
                var onSuccess = function (htmlString) {
                    $this.html(htmlString);
                };
                var ajaxSetting = {
                    url: '/site-main/Notice/NoticeMessages?lastPostDate=' + $this.attr('value'),
                    method: 'GET',
                    success: onSuccess
                };
                $.ajax(ajaxSetting);
            });
        };
        this.onClickMessageRegister(this.markMessageAsRead);
        this.showMoreMessagesRegister();
    }
    ;
    return NoticeMessage;
}());
