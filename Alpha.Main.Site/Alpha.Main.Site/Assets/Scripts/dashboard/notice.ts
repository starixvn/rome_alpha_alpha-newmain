class NoticeMessage extends DashboardBase {
    registerEvents = () => {
        this.onToggleMessage(this.markMessageAsRead);

        this.onToggleShowMoreMessages();

        this.hideMoreBtnIfHaveNoData();
    }

    private onToggleMessage = (markMessageAsRead: (message: JQuery) => void) => {
        $('.message').each((index: number, element: Element) => {
            let $this = this.$(element),
                shortContent = $this.children('.short-content'),
                fullContent = $this.children('.full-content'),
                icon = $this.find('.icon');

            $this.on('click', () => {
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
    }

    private markMessageAsRead = (message: JQuery) => {
        const isUnRead: boolean = message.hasClass('bold');

        if (isUnRead) {
            const onSuccess = (isMarkedSuccess: any) => {
                if (isMarkedSuccess) {
                    message.removeClass('bold');
                }
            };

            const ajaxSetting = {
                url: alpha.urls.messageRootPath + 'PortalMessage/MarkMessageAsRead',
                data: { messageId: message.attr('id') },
                method: 'POST',
                success: onSuccess
            };

            this.$.ajax(ajaxSetting);
        }
    }

    private onToggleShowMoreMessages = () => {
        const moreMessagesBtn = this.$('.more-info');

        moreMessagesBtn.on('click', () => {
            const content = this.$('#notice');

            const onSuccess = (htmlString: any) => {
                content.html(htmlString);

                this.registerEvents();
            }

            const ajaxSetting: JQueryAjaxSettings = {
                url: alpha.urls.messageRootPath + 'PortalMessage/MoreMessages?totalMessage=' + moreMessagesBtn.attr('totalMsg'),
                method: 'GET',
                cache: false,
                success: onSuccess
            };

            moreMessagesBtn.attr('disabled', 'disabled');
            moreMessagesBtn.html('<span class="block-icon-small"></span>');

            this.$.ajax(ajaxSetting);
        });
    }

    private hideMoreBtnIfHaveNoData = () => {
        const noInfo: Element = document.getElementById('no-info'),
            messageAmount: number = this.$('.msg-contents').length,
            moreTotalMessage: number = Number($('.more-info').attr('totalMsg'));

        if (noInfo !== null || moreTotalMessage > (messageAmount + 10)) {
            this.$('.more-info').hide();
        }
    }
}

$(() => {
    const notice = new NoticeMessage($);
    notice.loadInfo('#notice', notice.registerEvents);
})