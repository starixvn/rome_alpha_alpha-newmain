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
        }

        let ajaxSetting = {
            url: '/site-main/Notice/MarkMessageAsRead',
            data: { messageId: message.attr('id') },
            method: 'POST',
            success: onSuccess
        }

        $.ajax(ajaxSetting);
    }

    showMoreMessagesRegister = () => {
        $('.more-info').click(() => {
            let $this = $('.more-info');

            let onSuccess = (htmlString: any) => {
                $this.html(htmlString);
            }

            let ajaxSetting = {
                url: '/site-main/Notice/NoticeMessages?lastPostDate=' + $this.attr('value'),
                method: 'GET',
                success: onSuccess
            }

            $.ajax(ajaxSetting);
        });
    }
}