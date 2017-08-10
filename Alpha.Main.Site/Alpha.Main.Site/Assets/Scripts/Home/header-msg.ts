class HeaderMessage {
    private messageIndex: number = 0;
    private messageIdIndex: number = 0;
    private messageContentIndex: number = 1;
    private messageTypeIndex: number = 2;
    private messages: Array<string> = [];
    private publicMessageId: number = 0;
    private privateMessageId: number = 0;
    private scrollMessageTimeout: number;
    private scrollerLeft: number = 0;
    private scrollerWidth: number = 0;
    private maskWidth: number = 0;
    private scrollerPaused: boolean;
    private topNews: HTMLElement;
    private scroller: HTMLElement;
    private domElements = {
        btnPrivateMessage: null,
        privateMessageCounter: null,
        liveMessage: null,
        moveBetsMessage: null,
        btnSpecialMessage: null,
        marqueeMessage: null,
        hotMessages: null,
        userMessageModal: null,
        currentHotMessage: null,
        scroller: null,
        contentMessage: null,
        showMoreContainer: null,
        showLessContainer: null
    };
    protected urls = {
        mainAppPrivateMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=0',
        mainAppPublicMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=1',
        mainAppSpecialMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=2',
        oldMainAppNormalMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=',
        oldMainAppPrivateMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=private',
        oldMainAppSpecialMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=special',
        updatePrivateMessageCount: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx',
        getMessages: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/GetMessages.ashx',
        getHotMessages: alpha.urls.messageRootPath + 'Announcement/HeaderAnnouncement'
    };
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }
    init = () => {
        this.cacheElements();
        this.registerEvents();
        this.getHotMessages();
        this.getMessages();
        setInterval(this.getMessages, 60 * 1000);
    }

    viewPrivateMessage = () => {
        indexManager.changeMenu(this.urls.mainAppPrivateMessageForm);
        const countPrivateMsg = this.domElements.privateMessageCounter.html().replace("+", "");

        if (parseInt(countPrivateMsg) > 0) {
            //// Reset private message after click on view private message link
            this.$.ajax({
                url: this.urls.updatePrivateMessageCount,
                type: 'POST',
                success: (rawResult) => {
                    const result = alpha.core.convertReponseToJson(rawResult);

                    if (result.errCode === 0) {
                        this.domElements.privateMessageCounter.html('0');
                    }
                }
            });
        }
    }

    viewSpecialMessage = () => {
        indexManager.changeMenu(this.urls.mainAppSpecialMessageForm);
    }

    viewLiveMessage = (event: JQueryEventObject) => {
        const liveMessageUrl = this.$(event.target).attr('live-message-url');

        if (liveMessageUrl) {
            indexManager.changeMenu(liveMessageUrl);
        }
        else {
            indexManager.changeMenu(this.urls.mainAppPublicMessageForm);
        }
    }

    viewMoveBetsMessage = (event: JQueryEventObject) => {
        event.stopPropagation();
    }

    getHotMessages = () => {
        const ajaxSettings = this.createAjaxSettings('POST', this.urls.getHotMessages, this.showHotMessages);

        this.$.ajax(ajaxSettings);
    }

    showHotMessages = (message) => {
        this.appendHotMessageToPopup(message);
        this.domElements.currentHotMessage = this.domElements.hotMessages.find('p.current-hot-message');
        if (this.domElements.currentHotMessage.length > 0) {
            this.domElements
                .userMessageModal
                .modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, a' });

            this.calculateHotMessagePopupHeight(false);
        }

        setInterval(this.swapMessage, 18 * 1000);
    }

    createAjaxSettings = (methodType: string, url: string, onSuccess: any): JQueryAjaxSettings => {
        let ajaxSettings: JQueryAjaxSettings = {
            type: methodType,
            url: url,
            data: null,
            cache: false,
            success: onSuccess
        };

        return ajaxSettings;
    }

    resetHotMessageElements = (showFullMessage) => {
        const minMessageHeight = 40;
        this.domElements.currentHotMessage.css('min-height', minMessageHeight);

        if (showFullMessage) {
            this.domElements.showMoreContainer.addClass("hide");
            this.domElements.currentHotMessage.removeClass("brief-content");
            this.domElements.currentHotMessage.css('height', '');
        } else {
            this.domElements.showLessContainer.addClass("hide");
            this.domElements.currentHotMessage.removeClass("brief-content");
        }
    }

    setHotMessageElementStyleByMessageHeight = (showFullMessage, messageHeight, maxMessageHeight) => {
        if (showFullMessage) {
            if (messageHeight <= maxMessageHeight) {
                this.domElements.showLessContainer.addClass("hide");
            } else {
                this.domElements.showLessContainer.removeClass("hide");
            }
        }
        else {
            if (messageHeight <= maxMessageHeight) {
                this.domElements.showMoreContainer.addClass("hide");
            } else {
                this.domElements.currentHotMessage.addClass("brief-content").height(maxMessageHeight);
                this.domElements.showMoreContainer.removeClass("hide");
            }
        }
    }

    calculateHotMessageHeight = (showFullMessage, maxMessageHeight) => {
        const bodyBorderHeight = 2;
        const bodyPaddingHeight = 5 + bodyBorderHeight;
        let messageHeight = this.domElements.currentHotMessage.height();

        this.setHotMessageElementStyleByMessageHeight(showFullMessage, messageHeight, maxMessageHeight);

        if (maxMessageHeight < messageHeight) {
            messageHeight = showFullMessage ?
                messageHeight + this.domElements.showLessContainer.height() :
                maxMessageHeight + this.domElements.showMoreContainer.height();
        }

        messageHeight += bodyPaddingHeight;

        return messageHeight;
    }

    calculateHotMessagePopupHeight = (showFullMessage) => {
        this.resetHotMessageElements(showFullMessage);

        const maxMessageHeight = 53;
        const messageHeight = this.calculateHotMessageHeight(showFullMessage, maxMessageHeight);
        const headerHeight = 10;
        const popUpHeight = headerHeight + messageHeight;

        this.domElements.userMessageModal.height(popUpHeight);
    };

    showMoreHotMessage = () => {
        this.calculateHotMessagePopupHeight(true);
    }

    showLessHotMessage = () => {
        this.calculateHotMessagePopupHeight(false);
    }

    getMessages = () => {
        this.$.ajax({
            type: "POST",
            url: this.urls.getMessages,
            success: this.onSuccessGetMessages
        });
    }

    onSuccessGetMessages = (rawResults) => {
        const results = alpha.core.convertReponseToJson(rawResults),
            movebetsMessage = results.movebets_message || "";

        this.showPublicMessage(results);

        this.domElements.moveBetsMessage.html(movebetsMessage);

        this.showPrivateMessage(results);

        this.applyScroller();
    }

    showPublicMessage = (results: any) => {
        const publicMessage = results.public_message ? results.public_message : 0,
            privateMessage = results.private_message ? results.private_message : 0;

        // Process Public Message: show new public message and its link to message form
        const url = this.urls.mainAppPublicMessageForm;

        if (publicMessage[this.messageIdIndex]
            && publicMessage[this.messageIdIndex] !== this.publicMessageId) {
            this.domElements.liveMessage
                .html(publicMessage[this.messageContentIndex] + '. ')
                .find('font').attr('live-message-url', url);

            this.publicMessageId = publicMessage[this.messageIdIndex];
            if (this.publicMessageId !== 0) {
                this.appendMessageToPopup(publicMessage[this.messageContentIndex]);
            }
        }
        else {
            const spanMoveBets = this.$("#span-movebets");
            if (spanMoveBets.length) {
                spanMoveBets.remove();
            }
        }
    }

    showPrivateMessage = (results: any) => {
        const privateMessage = results.private_message ? results.private_message : 0,
            unreadPriMsgCount = results.unread_private_messages ? results.unread_private_messages : 0;

        // Process Private Message: show new private message
        const url = this.urls.mainAppPrivateMessageForm;

        if (privateMessage[this.messageIdIndex]
            && this.privateMessageId !== privateMessage[this.messageIdIndex]) {
            //// Append Private Message to Marquee with green color format.
            const privateMessageContent = this.domElements.liveMessage.find('#private-message-content');

            this.privateMessageId = privateMessage[this.messageIdIndex];
            if (privateMessageContent.length > 0) {
                privateMessageContent.html("Private message: " + privateMessage[this.messageContentIndex]);
            }
            else {
                const liveMessageContent = this.domElements.liveMessage.html();

                this.domElements.liveMessage.html(
                    liveMessageContent +
                    "<font id='private-message-content' color=\"green\">Private message: " +
                    privateMessage[this.messageContentIndex] +
                    "</font>")
                    .find('#private-message-content').attr('live-message-url', url);
            }
            if (this.privateMessageId !== 0) {
                this.appendMessageToPopup(privateMessage[this.messageContentIndex]);
            }
        }

        const unreadPriMsgCountText = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;

        this.domElements.privateMessageCounter.html(unreadPriMsgCountText);
    }

    appendMessageToPopup = (message) => {
        this.domElements.contentMessage.append(
            '<p class="not-shown-message">'
            + message
            + '</p>');
    }

    appendHotMessageToPopup = (message) => {
        this.domElements.contentMessage.append(message);
    }

    swapMessage = () => {
        let nextMessage: JQuery;

        if (this.domElements.currentHotMessage.length === 0) {
            nextMessage = this.domElements.hotMessages.find('p.not-shown-message:first');
        }
        else {
            nextMessage = this.domElements.currentHotMessage.next();
        }

        if (nextMessage.length > 0) {
            if (this.domElements.currentHotMessage.length > 0) {
                //// Hide old Message
                this.domElements.currentHotMessage.animate({
                    opacity: 0.0
                },
                    1500,
                    () => {
                        //// Remove Old Message and Show New Message
                        this.domElements.currentHotMessage.remove();
                        this.showNextMessage(nextMessage);
                    });
            }
            else {
                this.showNextMessage(nextMessage);
            }

            this.domElements.userMessageModal.modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, a' });
            this.calculateHotMessagePopupHeight(false);
        }

        setTimeout(this.swapMessage, 18 * 1000);
    }

    showNextMessage = (nextMessage: JQuery) => {
        nextMessage.show().removeClass('not-shown-message');
        this.domElements.currentHotMessage = nextMessage;
        this.calculateHotMessagePopupHeight(false);
        nextMessage.animate({ opacity: 1.0 }, 1500);
    }

    applyScroller = () => {
        if (this.scrollerLeft) {
            return;
        }

        this.scrollerLeft = 100;
        this.scrollerWidth = this.domElements.scroller.width();

        this.scroller = this.domElements.scroller[0];
        this.topNews = this.domElements.liveMessage[0];

        this.topNews.onmouseover = () => {
            this.pauseMarquee();
        };
        this.topNews.onmouseout = () => {
            this.playMarquee();
        };

        this.domElements.moveBetsMessage[0].onmouseover = () => {
            this.pauseMarquee();
        };
        this.domElements.moveBetsMessage[0].onmouseout = () => {
            this.playMarquee();
        };

        const textNews = this.domElements.marqueeMessage[0];
        this.maskWidth = textNews.offsetWidth;

        this.scrollMessage();
    }

    playMarquee = () => {
        if (this.scrollerPaused) {
            this.scrollerPaused = false;
            this.scrollMessageTimeout = setTimeout(this.scrollMessage, 20);
        } else {
            this.scrollerPaused = false;
        }
    }

    pauseMarquee = () => {
        clearTimeout(this.scrollMessageTimeout);
        this.scrollMessageTimeout = 0;
        this.scrollerPaused = true;
    }

    scrollMessage = () => {
        if (this.scrollerPaused) {
            return;
        }

        if (this.scrollerLeft + this.scrollerWidth < -5) {
            this.scrollerLeft = this.maskWidth + 5;
            this.scrollerWidth = this.topNews.offsetWidth;
        }

        this.scroller.style.left = (this.scrollerLeft--) + 'px';
        this.scrollMessageTimeout = setTimeout(this.scrollMessage, 20);
    }

    protected cacheElements = () => {
        this.domElements.btnPrivateMessage = this.$('#btnPrivateMessage');
        this.domElements.privateMessageCounter = this.$('#privateMessageCounter');
        this.domElements.liveMessage = this.$('#liveMessage');
        this.domElements.moveBetsMessage = this.$('#movebets-message');
        this.domElements.btnSpecialMessage = this.$('#btnSpecialMessage');
        this.domElements.marqueeMessage = this.$('#marqueeMessage');
        this.domElements.hotMessages = this.$('#userMessageModalBody');
        this.domElements.contentMessage = this.$('#msg-content');
        this.domElements.userMessageModal = this.$('#userMessageModal');
        this.domElements.currentHotMessage = this.domElements.hotMessages.find('p.current-hot-message');
        this.domElements.scroller = this.$('#scroller');
        this.domElements.showMoreContainer = this.$(".showmore-container");
        this.domElements.showLessContainer = this.$(".showless-container");
    };

    protected registerEvents = () => {
        this.domElements.btnPrivateMessage.on('click', this.viewPrivateMessage);
        this.domElements.btnSpecialMessage.on('click', this.viewSpecialMessage);
        this.domElements.liveMessage.on('click', this.viewLiveMessage);
        this.domElements.marqueeMessage.on('click', this.viewLiveMessage);
        this.domElements.moveBetsMessage.on('click', this.viewMoveBetsMessage);
        this.domElements.showMoreContainer.on('click', this.showMoreHotMessage);
        this.domElements.showLessContainer.on('click', this.showLessHotMessage);
    };
}

$(() => {
    if (!alpha.isScreenLower768) {
        const headerMessage = new HeaderMessage($);
        headerMessage.init();
    }
});