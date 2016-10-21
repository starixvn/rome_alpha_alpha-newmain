/// <reference path="../core.ts" />

class NoticeMessage extends Dashboard {
    registerEvent = () => {
        this.onToggleMessage(this.markMessageAsRead);

        this.onToggleShowMoreMessages();

        this.onSwipeInMobileView();

        this.hideMoreBtnIfHaveNoData();
    }

    private onToggleMessage = (markMessageAsRead: any) => {
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

    private markMessageAsRead = (message: any) => {
        const isUnRead: boolean = message.hasClass('bold');

        if (isUnRead) {
            const onSuccess = (isMarkedSuccess: any) => {
                if (isMarkedSuccess) {
                    message.removeClass('bold');
                }
            };

            const ajaxSetting = {
                url: alpha.urls.mainRootPath + '/Notice/MarkMessageAsRead',
                data: { messageId: message.attr('id') },
                method: 'POST',
                success: onSuccess
            };

            $.ajax(ajaxSetting);
        }
    }

    private onToggleShowMoreMessages = () => {
        const $moreMessagesBtn = $('.more-info');

        $moreMessagesBtn.click(() => {
            const $content = $('#notice');

            const onSuccess = (htmlString: any) => {
                $content.html(htmlString);

                new NoticeMessage($).registerEvent();
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

    private onSwipeInMobileView = () => {
        const stats = document.getElementById('stats'),
            statsTab = document.getElementById('stats-tab'),
            notice = document.getElementById('notice'),
            noticeTab = document.getElementById('notice-tab');

        $('#notice').hammer().on("swiperight", () => {
            notice.classList.remove('active');
            noticeTab.classList.remove('active');

            stats.classList.add('active');
            statsTab.classList.add('active');
        });
    }

    private hideMoreBtnIfHaveNoData = () => {
        const noInfo: Element = document.getElementById('no-info'),
            messageAmount: number = $('.msg-contents').length,
            moreTotalMessage: number = Number($('.more-info').attr('totalMsg'));

        if (noInfo !== null || moreTotalMessage > (messageAmount + 10)) {
            $('.more-info').hide();
        }
    }
}

const notice = new NoticeMessage($);
notice.loadInfo('#notice', notice.registerEvent);