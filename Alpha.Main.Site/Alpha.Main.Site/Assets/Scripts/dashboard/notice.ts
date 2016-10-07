/// <reference path="../core.ts" />

class NoticeMessage {
    registerEvent = () => {
        this.onToggleMessage(this.markMessageAsRead);

        this.onToggleShowMoreMessages();

        this.onSwipeInMobileView();
    }

    onToggleMessage = (markMessageAsRead: any) => {
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
        const isUnRead: boolean = message.hasClass('bold');

        if (isUnRead) {
            const onSuccess = (isMarkedSuccess: any) => {
                if (isMarkedSuccess) {
                    message.removeClass('bold');
                }
            };

            const ajaxSetting = {
                url: '/site-main/Notice/MarkMessageAsRead',
                data: { messageId: message.attr('id') },
                method: 'POST',
                success: onSuccess
            };

            $.ajax(ajaxSetting);
        }
    }

    onToggleShowMoreMessages = () => {
        const $moreMessagesBtn = $('.more-info');

        $moreMessagesBtn.click(() => {
            const $content = $('#notice');

            const onSuccess = (htmlString: any) => {
                $content.html(htmlString);
                new NoticeMessage();
            }

            const ajaxSetting = {
                url: alpha.urls.mainRootPath + '/Notice/MoreMessages?totalMessage=' + $moreMessagesBtn.attr('totalMsg'),
                method: 'GET',
                success: onSuccess
            };

            $moreMessagesBtn.attr('disabled', 'disabled');
            $moreMessagesBtn.text('Loading...');

            $.ajax(ajaxSetting);
        });
    }

    onSwipeInMobileView = () => {
        const stats = document.getElementById('stats'),
            statsTab = document.getElementById('stats-tab'),
            notice = document.getElementById('notice'),
            noticeTab = document.getElementById('notice-tab');

        let noticeHM = new Hammer(notice);

        noticeHM.on("swiperight", (ev) => {
            notice.classList.remove('active');
            noticeTab.classList.remove('active');

            stats.classList.add('active');
            statsTab.classList.add('active');
        });
    }
}