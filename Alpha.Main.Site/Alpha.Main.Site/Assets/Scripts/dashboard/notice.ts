module Dashboard {
    export class NoticeMessage extends Base {
        registerEvent = () => {
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

                $this.click(() => {
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
                    url: alpha.urls.mainRootPath + '/Notice/MarkMessageAsRead',
                    data: { messageId: message.attr('id') },
                    method: 'POST',
                    success: onSuccess
                };

                this.$.ajax(ajaxSetting);
            }
        }

        private onToggleShowMoreMessages = () => {
            const moreMessagesBtn = this.$('.more-info');

            moreMessagesBtn.click(() => {
                const content = this.$('#notice');

                const onSuccess = (htmlString: any) => {
                    content.html(htmlString);

                    new NoticeMessage($).registerEvent();
                }

                const ajaxSetting: JQueryAjaxSettings = {
                    url: alpha.urls.mainRootPath + '/Notice/MoreMessages?totalMessage=' + moreMessagesBtn.attr('totalMsg'),
                    method: 'GET',
                    success: onSuccess
                };

                moreMessagesBtn.attr('disabled', 'disabled');
                moreMessagesBtn.text('Loading...');

                this.$.ajax(ajaxSetting);
            });
        }

        private onSwipeInMobileView = () => {
            const stats = document.getElementById('stats'),
                statsTab = document.getElementById('stats-tab'),
                notice = document.getElementById('notice'),
                noticeTab = document.getElementById('notice-tab');

            this.$('#notice').hammer().on("swiperight", () => {
                notice.classList.remove('active');
                noticeTab.classList.remove('active');

                stats.classList.add('active');
                statsTab.classList.add('active');
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
}