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
                data: { messageId: message.attr('id') },
                method: 'POST',
                success: onSuccess
            };
            $.ajax(ajaxSetting);
        };
        this.showMoreMessagesRegister = function () {
            var $moreMessagesBtn = $('.more-info');
            $moreMessagesBtn.click(function () {
                var $content = $('#notice');
                var onSuccess = function (htmlString) {
                    debugger;
                    $content.html(htmlString);
                    new NoticeMessage();
                };
                var ajaxSetting = {
                    url: '/site-main/Notice/MoreMessages?totalMessage=' + $moreMessagesBtn.attr('totalMsg'),
                    method: 'GET',
                    success: onSuccess
                };
                $moreMessagesBtn.attr('disabled', 'disabled');
                $moreMessagesBtn.text('Loading...');
                $.ajax(ajaxSetting);
            });
        };
        this.onClickMessageRegister(this.markMessageAsRead);
        this.showMoreMessagesRegister();
    }
    ;
    return NoticeMessage;
}());
