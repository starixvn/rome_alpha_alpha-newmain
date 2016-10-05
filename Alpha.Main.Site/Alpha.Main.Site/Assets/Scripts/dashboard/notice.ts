class NoticeMessage {
    constructor() {
        this.onClickMessageRegister(this.markMessageAsRead);
        this.showMoreMessagesRegister();
    };

    onClickMessageRegister = (markMessageAsRead: any) => {
        $('.msg').each(function () {
            let $this = $(this),
                $shortContent = $this.children('.short-content'),
                $fullContent = $this.children('.full-content'),
                $icon = $this.find('.icon');

            $this.click(() => {
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
    }

    markMessageAsRead = (message: any) => {
        let isRead: boolean = message.hasClass('bold');

        let onSuccess = (isMarkedSuccess: any) => {
            if (isMarkedSuccess) {
                message.removeClass('bold');
            }
        };

        let ajaxSetting = {
            url: '/site-main/Notice/MarkMessageAsRead',
            data: { messageId: message.attr('id') },
            method: 'POST',
            success: onSuccess
        };

        $.ajax(ajaxSetting);
    }

    showMoreMessagesRegister = () => {
        let $moreMessagesBtn = $('.more-info');

        $moreMessagesBtn.click(() => {
            let $content = $('#notice');

            let onSuccess = (htmlString: any) => {
                debugger
                $content.html(htmlString);
                new NoticeMessage();
            }

            let ajaxSetting = {
                url: '/site-main/Notice/MoreMessages?totalMessage=' + $moreMessagesBtn.attr('totalMsg'),
                method: 'GET',
                success: onSuccess
            };

            $moreMessagesBtn.attr('disabled', 'disabled');
            $moreMessagesBtn.text('Loading...');

            $.ajax(ajaxSetting);
        });
    }
}