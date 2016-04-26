/// <reference path="index.js" />

'use strict';
/*global index*/

var headerMessage = {
    urls: {
        oldMainAppNormalMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=',
        oldMainAppPrivateMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=private',
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

    domElements: {
        btnPrivateMessage: null,
        privateMessageCounter: null,
        liveMessage: null,
        btnSpecialMessage: null,
        marqueeMessage: null,
        hotMessages: null,
        userMessageModal: null,
        currentHotMessage: null,
        scroller: null,

        showMoreContainer: null,
        showLessContainer: null
    },

    init: function () {
        headerMessage.cacheElements();
        headerMessage.registerEvents();

        headerMessage.showHotMessages();
        headerMessage.getMessages();
    },

    cacheElements: function () {
        headerMessage.domElements.btnPrivateMessage = $('#btnPrivateMessage');
        headerMessage.domElements.privateMessageCounter = $('#privateMessageCounter');
        headerMessage.domElements.liveMessage = $('#liveMessage');
        headerMessage.domElements.btnSpecialMessage = $('#btnSpecialMessage');
        headerMessage.domElements.marqueeMessage = $('#marqueeMessage');
        headerMessage.domElements.hotMessages = $('#userMessageModalBody');
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

        headerMessage.domElements.showMoreContainer.click(headerMessage.showMoreHotMessage);
        headerMessage.domElements.showLessContainer.click(headerMessage.showLessHotMessage);
    },

    viewPrivateMessage: function () {

        index.changeMenu(headerMessage.urls.oldMainAppPrivateMessageForm);

        var countPrivateMsg = headerMessage.domElements.privateMessageCounter.html().replace("+", "");

        if (parseInt(countPrivateMsg) > 0) {
            //// Reset private message after click on view private message link
            $.ajax({
                url: headerMessage.urls.updatePrivateMessageCount,
                type: 'POST',
                responseType: 'json',
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
        index.changeMenu(headerMessage.urls.oldMainAppSpecialMessageForm);
    },

    viewLiveMessage: function () {
        var liveMessageUrl = headerMessage.domElements.liveMessage.data('live-message-url');
        if (liveMessageUrl) {
            index.changeMenu(liveMessageUrl);
        }
        else {
            index.changeMenu(headerMessage.urls.oldMainAppNormalMessageForm);
        }
    },

    showHotMessages: function () {
        if (headerMessage.domElements.currentHotMessage.length > 0) {
            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false });
            headerMessage.domElements.userMessageModal.tinyDraggable();
            headerMessage.calculateHotMessagePopupHeight(false);
        }

        setInterval(headerMessage.swapMessage, 18 * 1000);
    },

    calculateHotMessagePopupHeight: function (showFullMessage) {
        var maxMessageHeight = 70;
        var isShowFull = showFullMessage;
        var messageHeight = headerMessage.domElements.currentHotMessage.height();
        var headerHeight = 20;
        var popUpPadding = 14;

        if (showFullMessage) {
            headerMessage.domElements.showMoreContainer.addClass("hide");
            headerMessage.domElements.currentHotMessage.removeClass("brief-content");
            messageHeight = headerMessage.domElements.currentHotMessage.height();

            if (messageHeight <= maxMessageHeight) {
                headerMessage.domElements.showLessContainer.addClass("hide");
            } else {
                headerMessage.domElements.showLessContainer.removeClass("hide");
                messageHeight += headerMessage.domElements.showLessContainer.height() + 10;
            }

            messageHeight += 10;
        }
        else {
            headerMessage.domElements.showLessContainer.addClass("hide");
            headerMessage.domElements.currentHotMessage.removeClass("brief-content");
            messageHeight = headerMessage.domElements.currentHotMessage.height();

            if (messageHeight <= maxMessageHeight) {
                headerMessage.domElements.showMoreContainer.addClass("hide");
                messageHeight = messageHeight + 10;
            } else {
                headerMessage.domElements.currentHotMessage.addClass("brief-content");
                headerMessage.domElements.showMoreContainer.removeClass("hide");
                messageHeight = maxMessageHeight + headerMessage.domElements.showMoreContainer.height() + 10;
            }
        }

        var popUpHeight = headerHeight + messageHeight + popUpPadding;
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
        var url = headerMessage.urls.oldMainAppNormalMessageForm + publicMessage[headerMessage.messageTypeIndex];

        if (publicMessage[headerMessage.messageIdIndex]
            && publicMessage[headerMessage.messageIdIndex] !== headerMessage.publicMessageId) {

            headerMessage.domElements.liveMessage
                .html(publicMessage[headerMessage.messageContentIndex])
                .data(url);

            headerMessage.appendMessageToPopup(publicMessage[headerMessage.messageContentIndex]);

            headerMessage.publicMessageId = publicMessage[headerMessage.messageIdIndex];
        }
    },

    showPrivateMessage: function (privateMessage, unreadPriMsgCount) {
        // Process Private Message: show new private message
        if (privateMessage[headerMessage.messageIdIndex]
            && headerMessage.privateMessageId !== privateMessage[headerMessage.messageIdIndex]) {

            //// Append Private Message to Marquee with green color format.
            var privateMessageContent = headerMessage.domElements.liveMessage.find('#private-message-content');

            if (privateMessageContent.length > 0) {
                privateMessageContent.html(privateMessage[headerMessage.messageContentIndex]);
            }
            else {
                var liveMessageContent = headerMessage.domElements.liveMessage.html();
                headerMessage.domElements.liveMessage.html(
                    liveMessageContent
                    + ". <font id='private-message-content' color=\"green\">Private message: "
                    + privateMessage[headerMessage.messageContentIndex]
                    + "</font>");
            }

            headerMessage.appendMessageToPopup(privateMessage[headerMessage.messageContentIndex]);

            headerMessage.privateMessageId = privateMessage[headerMessage.messageIdIndex];
        }

        unreadPriMsgCount = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;

        headerMessage.domElements.privateMessageCounter.html(unreadPriMsgCount);
    },

    appendMessageToPopup: function (messsage) {
        headerMessage.domElements.hotMessages.append(
                '<p class="not-shown-message">'
                + messsage
                + '</p>');
    },

    getMessages: function () {
        $.ajax({
            type: "POST",
            url: headerMessage.urls.getMessages,
            responseType: "json",
            success: function (rawResults) {
                var results = alpha.core.convertReponseToJson(rawResults);

                try {
                    var publicMessage = results.public_message ? results.public_message : 0,
                        privateMessage = results.private_message ? results.private_message : 0,
                        unreadPriMsgCount = results.unread_private_messages ? results.unread_private_messages : 0;

                    headerMessage.showPublicMessage(publicMessage);

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

            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false });
            headerMessage.domElements.userMessageModal.tinyDraggable();
        }
    },

    showNextMessage: function (nextMessage) {
        nextMessage.show().removeClass('not-shown-message').animate({
            opacity: 1.0
        },
      1500,
      function () {
          headerMessage.domElements.currentHotMessage = nextMessage;
          headerMessage.calculateHotMessagePopupHeight(false);
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
            clearTimeout(headerMessage.scrollMessageTimeout);
            headerMessage.scrollMessageTimeout = 0;
            headerMessage.scrollerPaused = true;
        };
        headerMessage.topnews.onmouseout = function () {
            if (headerMessage.scrollerPaused) {
                headerMessage.scrollerPaused = false;
                headerMessage.scrollMessageTimeout = setTimeout(headerMessage.scrollMessage, 20);
            } else {
                headerMessage.scrollerPaused = false;
            }
        };

        var textNews = headerMessage.domElements.marqueeMessage[0];
        headerMessage.maskWidth = parseInt(textNews.offsetWidth);

        headerMessage.scrollMessage();
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
