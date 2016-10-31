'use strict';
/*global indexManager*/

var headerMessage = {
    urls: {
        oldMainAppNormalMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=',
        oldMainAppPrivateMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=private',
        mainAppPrivateMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=0',
        mainAppPublicMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=1',
        mainAppSpecialMessageForm: alpha.urls.mainRootPath + 'Message/Index?messageType=2',
        oldMainAppSpecialMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=special',
        updatePrivateMessageCount: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx',
        getMessages: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/GetMessages.ashx'
    },

    messageIndex: 0,
    messages: [],

    messageIdIndex: 0,
    messageContentIndex: 1,
    messageTypeIndex: 2,
    publicMessageId: 0,
    privateMessageId: 0,
    scrollMessageTimeout: null,

    scrollerLeft: null,
    scrollerWidth: null,
    maskWidth: null,
    scrollerPaused: null,
    topnews: null,
    scroller: null,

    domElements: {
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
    },

    init: () => {
        headerMessage.cacheElements();
        headerMessage.registerEvents();

        headerMessage.showHotMessages();
        headerMessage.getMessages();

        if (alpha.isScreenLower768) {
            const headerMessageMobile = new Home.HeaderMessageMobile();
            headerMessageMobile.showMoveBetsMessagePopUp();
        }
    },

    cacheElements: function () {
        headerMessage.domElements.btnPrivateMessage = $('#btnPrivateMessage');
        headerMessage.domElements.privateMessageCounter = $('#privateMessageCounter');
        headerMessage.domElements.liveMessage = $('#liveMessage');
        headerMessage.domElements.moveBetsMessage = $('#movebets-message');
        headerMessage.domElements.btnSpecialMessage = $('#btnSpecialMessage');
        headerMessage.domElements.marqueeMessage = $('#marqueeMessage');
        headerMessage.domElements.hotMessages = $('#userMessageModalBody');
        headerMessage.domElements.contentMessage = $('#msg-content');
        headerMessage.domElements.userMessageModal = $('#userMessageModal');
        headerMessage.domElements.currentHotMessage = headerMessage.domElements.hotMessages.find('p.current-hot-message');
        headerMessage.domElements.scroller = $('#scroller');

        headerMessage.domElements.showMoreContainer = $(".showmore-container");
        headerMessage.domElements.showLessContainer = $(".showless-container");
    },

    registerEvents: function () {
        headerMessage.domElements.btnPrivateMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewPrivateMessage);

        headerMessage.domElements.btnSpecialMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewSpecialMessage);

        headerMessage.domElements.liveMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewLiveMessage);

        headerMessage.domElements.marqueeMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewLiveMessage);

        headerMessage.domElements.moveBetsMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewMoveBetsMessage);

        headerMessage.domElements.showMoreContainer.click(headerMessage.showMoreHotMessage);
        headerMessage.domElements.showLessContainer.click(headerMessage.showLessHotMessage);
    },

    viewPrivateMessage: function () {
        indexManager.changeMenu(headerMessage.urls.mainAppPrivateMessageForm);

        var countPrivateMsg = headerMessage.domElements.privateMessageCounter.html().replace("+", "");

        if (parseInt(countPrivateMsg) > 0) {
            //// Reset private message after click on view private message link
            $.ajax({
                url: headerMessage.urls.updatePrivateMessageCount,
                type: 'POST',
                success: function (rawResult) {
                    var result = alpha.core.convertReponseToJson(rawResult);
                    if (result.errCode === 0) {
                        headerMessage.domElements.privateMessageCounter.html(0);
                    }
                }
            });
        }
    },

    viewSpecialMessage: function () {
        indexManager.changeMenu(headerMessage.urls.mainAppSpecialMessageForm);
    },

    viewLiveMessage: (event) => {
        var liveMessageUrl = $(event.target).attr('live-message-url');

        if (liveMessageUrl) {
            indexManager.changeMenu(liveMessageUrl);
        }
        else {
            indexManager.changeMenu(headerMessage.urls.mainAppPublicMessageForm);
        }
    },

    viewMoveBetsMessage: function (event) {
        event.stopPropagation();
    },

    showHotMessages: function () {
        if (headerMessage.domElements.currentHotMessage.length > 0) {
            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, a' });
            headerMessage.calculateHotMessagePopupHeight(false);
        }

        setInterval(headerMessage.swapMessage, 18 * 1000);
    },

    resetHotMessageElements: function (showFullMessage) {
        var minMessageHeight = 40;
        headerMessage.domElements.currentHotMessage.css('min-height', minMessageHeight);

        if (showFullMessage) {
            headerMessage.domElements.showMoreContainer.addClass("hide");
            headerMessage.domElements.currentHotMessage.removeClass("brief-content");
            headerMessage.domElements.currentHotMessage.css('height', '');
        } else {
            headerMessage.domElements.showLessContainer.addClass("hide");
            headerMessage.domElements.currentHotMessage.removeClass("brief-content");
        }
    },
    setHotMessageElementStyleByMessageHeight: function (showFullMessage, messageHeight, maxMessageHeight) {
        if (showFullMessage) {
            if (messageHeight <= maxMessageHeight) {
                headerMessage.domElements.showLessContainer.addClass("hide");
            } else {
                headerMessage.domElements.showLessContainer.removeClass("hide");
            }
        }
        else {
            if (messageHeight <= maxMessageHeight) {
                headerMessage.domElements.showMoreContainer.addClass("hide");
            } else {
                headerMessage.domElements.currentHotMessage.addClass("brief-content").height(maxMessageHeight);
                headerMessage.domElements.showMoreContainer.removeClass("hide");
            }
        }
    },
    calculateHotMessageHeight: function (showFullMessage, maxMessageHeight) {
        var messageHeight = headerMessage.domElements.currentHotMessage.height();
        headerMessage.setHotMessageElementStyleByMessageHeight(showFullMessage, messageHeight, maxMessageHeight);

        if (maxMessageHeight < messageHeight) {
            messageHeight = showFullMessage ?
                messageHeight + headerMessage.domElements.showLessContainer.height() :
                maxMessageHeight + headerMessage.domElements.showMoreContainer.height();
        }

        var bodyBorderHeight = 2;
        var bodyPaddingHeight = 5 + bodyBorderHeight;
        messageHeight += bodyPaddingHeight;

        return messageHeight;
    },
    calculateHotMessagePopupHeight: function (showFullMessage) {
        headerMessage.resetHotMessageElements(showFullMessage);

        var maxMessageHeight = 53;
        var messageHeight = headerMessage.calculateHotMessageHeight(showFullMessage, maxMessageHeight);

        var headerHeight = 10;
        var popUpHeight = headerHeight + messageHeight;

        headerMessage.domElements.userMessageModal.height(popUpHeight);
    },

    showMoreHotMessage: function () {
        headerMessage.calculateHotMessagePopupHeight(true);
    },

    showLessHotMessage: function () {
        headerMessage.calculateHotMessagePopupHeight(false);
    },

    showPublicMessage: function (publicMessage) {
        // Process Public Message: show new public message and its link to message form
        const url = headerMessage.urls.mainAppPublicMessageForm;

        if (publicMessage[headerMessage.messageIdIndex]
            && publicMessage[headerMessage.messageIdIndex] !== headerMessage.publicMessageId) {
            headerMessage.domElements.liveMessage
                .html(publicMessage[headerMessage.messageContentIndex] + '. ')
                .find('font').attr('live-message-url', url);

            if (headerMessage.publicMessageId !== 0) {
                headerMessage.appendMessageToPopup(publicMessage[headerMessage.messageContentIndex]);
            }

            headerMessage.publicMessageId = publicMessage[headerMessage.messageIdIndex];
        }
        else {
            var spanMoveBets = $("#span-movebets");
            if (spanMoveBets.length) {
                spanMoveBets.remove();
            }
        }
    },

    showPrivateMessage: function (privateMessage, unreadPriMsgCount) {
        // Process Private Message: show new private message
        const url = headerMessage.urls.mainAppPrivateMessageForm;

        if (privateMessage[headerMessage.messageIdIndex]
            && headerMessage.privateMessageId !== privateMessage[headerMessage.messageIdIndex]) {
            //// Append Private Message to Marquee with green color format.
            var privateMessageContent = headerMessage.domElements.liveMessage.find('#private-message-content');

            if (privateMessageContent.length > 0) {
                privateMessageContent.html("Private message: " + privateMessage[headerMessage.messageContentIndex]);
            }
            else {
                var liveMessageContent = headerMessage.domElements.liveMessage.html();

                headerMessage.domElements.liveMessage
                    .html(liveMessageContent +
                    "<font id='private-message-content' color=\"green\">Private message: " +
                    privateMessage[headerMessage.messageContentIndex] +
                    "</font>")
                    .find('#private-message-content').attr('live-message-url', url);
            }

            if (headerMessage.privateMessageId !== 0) {
                headerMessage.appendMessageToPopup(privateMessage[headerMessage.messageContentIndex]);
            }

            headerMessage.privateMessageId = privateMessage[headerMessage.messageIdIndex];
        }

        unreadPriMsgCount = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;

        headerMessage.domElements.privateMessageCounter.html(unreadPriMsgCount);
    },

    appendMessageToPopup: function (message) {
        headerMessage.domElements.contentMessage.append(
            '<p class="not-shown-message">'
            + message
            + '</p>');
    },

    getMessages: function () {
        $.ajax({
            type: "POST",
            url: headerMessage.urls.getMessages,
            success: function (rawResults) {
                try {
                    var results = alpha.core.convertReponseToJson(rawResults);

                    var publicMessage = results.public_message ? results.public_message : 0,
                        privateMessage = results.private_message ? results.private_message : 0,
                        unreadPriMsgCount = results.unread_private_messages ? results.unread_private_messages : 0,
                        movebetsMessage = results.movebets_message || "";

                    headerMessage.showPublicMessage(publicMessage);

                    headerMessage.domElements.moveBetsMessage.html(movebetsMessage || "");

                    headerMessage.showPrivateMessage(privateMessage, unreadPriMsgCount);

                    headerMessage.applyScroller();
                } catch (e) { }

                setTimeout(headerMessage.getMessages, 60 * 1000);
            }
        });
    },

    swapMessage: function () {
        var nextMessage = [];

        if (headerMessage.domElements.currentHotMessage.length === 0) {
            nextMessage = headerMessage.domElements.hotMessages.find('p.not-shown-message:first');
        }
        else {
            nextMessage = headerMessage.domElements.currentHotMessage.next();
        }

        if (nextMessage.length > 0) {
            if (headerMessage.domElements.currentHotMessage.length > 0) {
                //// Hide old Message
                headerMessage.domElements.currentHotMessage.animate({
                    opacity: 0.0
                },
                    1500,
                    function () {
                        //// Remove Old Message and Show New Message
                        headerMessage.domElements.currentHotMessage.remove();
                        headerMessage.showNextMessage(nextMessage);
                    });
            }
            else {
                headerMessage.showNextMessage(nextMessage);
            }

            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false })
                .tinyDraggable({ exclude: 'button.icon.icon-close.close, a' });
            headerMessage.calculateHotMessagePopupHeight(false);
        }
    },

    showNextMessage: function (nextMessage) {
        nextMessage.show().removeClass('not-shown-message');
        headerMessage.domElements.currentHotMessage = nextMessage;
        headerMessage.calculateHotMessagePopupHeight(false);
        nextMessage.animate({
            opacity: 1.0
        },
            1500,
            function () {
            });
    },

    applyScroller: function () {
        if (headerMessage.scrollerLeft) {
            return;
        }

        headerMessage.scrollerLeft = 100;
        headerMessage.scrollerWidth = headerMessage.domElements.scroller.width();

        headerMessage.scroller = headerMessage.domElements.scroller[0];
        headerMessage.topnews = headerMessage.domElements.liveMessage[0];

        headerMessage.topnews.onmouseover = function () {
            headerMessage.pauseMarquee();
        };
        headerMessage.topnews.onmouseout = function () {
            headerMessage.playMarquee();
        };

        headerMessage.domElements.moveBetsMessage[0].onmouseover = function () {
            headerMessage.pauseMarquee();
        };
        headerMessage.domElements.moveBetsMessage[0].onmouseout = function () {
            headerMessage.playMarquee();
        };

        var textNews = headerMessage.domElements.marqueeMessage[0];
        headerMessage.maskWidth = parseInt(textNews.offsetWidth);

        headerMessage.scrollMessage();
    },

    playMarquee: function () {
        if (headerMessage.scrollerPaused) {
            headerMessage.scrollerPaused = false;
            headerMessage.scrollMessageTimeout = setTimeout(headerMessage.scrollMessage, 20);
        } else {
            headerMessage.scrollerPaused = false;
        }
    },

    pauseMarquee: function () {
        clearTimeout(headerMessage.scrollMessageTimeout);
        headerMessage.scrollMessageTimeout = 0;
        headerMessage.scrollerPaused = true;
    },

    scrollMessage: function () {
        if (headerMessage.scrollerPaused) {
            return;
        }

        if (headerMessage.scrollerLeft + headerMessage.scrollerWidth < -5) {
            headerMessage.scrollerLeft = headerMessage.maskWidth + 5;
            headerMessage.scrollerWidth = parseInt(headerMessage.topnews.offsetWidth);
        }

        headerMessage.scroller.style.left = (headerMessage.scrollerLeft--) + 'px';
        headerMessage.scrollMessageTimeout = setTimeout(headerMessage.scrollMessage, 20);
    }
};

$(function () {
    headerMessage.init();
});