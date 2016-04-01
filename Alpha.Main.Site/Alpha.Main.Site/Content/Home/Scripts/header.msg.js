var oldMainRootPath = document.getElementById('oldMainRootPath').value;
var headerMessage = {
    urls: {
        oldMainAppMessageForm: oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=',
        oldMainAppPrivateMessageForm: oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=private',
        updatePrivateMessageCount: oldMainRootPath + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx',
        getMessages: oldMainRootPath + '_UserMessage/Handlers/GetMessages.ashx'
    },

    messageIndex: 0,
    messages: [],

    messageIdIndex: 0,
    messageContentIndex: 1,
    messageTypeIndex: 2,

    domElements: {
        btnPrivateMessage: null,
        privateMessageCounter: null,
        liveMessage: null,
        btnSpecialMessage: null,
        topMessagePopup: null,
        marqueeMessage: null,
        popupContainer1: null,
        popupContainer2: null,
        hotMessages: null,
        userMessageModal: null
    },

    init: function () {
        headerMessage.cacheElements();
        headerMessage.registerEvents();

        headerMessage.showHotMessage();
        headerMessage.getMessages();
    },

    cacheElements: function () {
        headerMessage.domElements.btnPrivateMessage = $('#btnPrivateMessage');
        headerMessage.domElements.privateMessageCounter = $('#privateMessageCounter');
        headerMessage.domElements.liveMessage = $('#liveMessage');
        headerMessage.domElements.btnSpecialMessage = $('#btnSpecialMessage');
        headerMessage.domElements.topMessagePopup = $('.top-message');
        headerMessage.domElements.marqueeMessage = $('#marqueeMessage');
        headerMessage.domElements.popupContainer1 = $('#popupContainer1');
        headerMessage.domElements.popupContainer2 = $('#popupContainer2');
        headerMessage.domElements.hotMessages = $('#userMessageModalBody p');
        headerMessage.domElements.userMessageModal = $('#userMessageModal');
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
    },

    viewPrivateMessage: function () {
        
        indexPageManager.changeMenu(headerMessage.urls.oldMainAppPrivateMessageForm);

        var countPrivateMsg = headerMessage.domElements.privateMessageCounter.html().replace("+", "");

        if (parseInt(countPrivateMsg) > 0) {
            //// Reset private message after click on view private message link
            $.ajax({
                url: headerMessage.urls.updatePrivateMessageCount,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (data) {
                if (data.errCode == 0) {
                    headerMessage.domElements.privateMessageCounter.html(0);
                }
            });
        }
    },

    viewSpecialMessage: function () {
        indexPageManager.changeMenu(headerMessage.urls.oldMainAppPrivateMessageForm);
    },

    viewLiveMessage: function () {
        var liveMessageUrl = headerMessage.domElements.liveMessage.data('live-message-url');
        if (liveMessageUrl) {
            indexPageManager.changeMenu(liveMessageUrl);
        }
        else {
            indexPageManager.changeMenu(headerMessage.urls.oldMainAppPrivateMessageForm);
        }
    },

    showHotMessage: function () {
        headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false });
        //$(headerMessage.domElements.hotMessages[0]).animate({
        //    opacity: 0.0
        //}, 1500, function () {
        //    $(headerMessage.domElements.hotMessages[0]).hide()
        //    $(headerMessage.domElements.hotMessages[1]).show().animate({
        //        opacity: 1.0
        //    }, 1500);
        //});
       
        //if (headerMessage.domElements.hotMessages.length == 0) {

        //    return;
        //}


        //headerMessage.domElements.popupContainer1.html(messages[headerMessage.messageIndex]);
        //if (messages.length > 1) {
        //    headerMessage.domElements.popupContainer2(messages[headerMessage.messageIndex + 1]);
        //}

        //headerMessage.domElements.topMessagePopup.css("height", headerMessage.domElements.popupContainer1.height() + 'px');
        //if (messages.length > 1) {
        //    setInterval(headerMessage.startSwapMessage, 18000);
        //}


    },

    showPublicMessage: function (publicMessage) {
        // Process Public Message: show new public message and its link to message form
        var url = headerMessage.urls.oldMainAppPrivateMessageForm + publicMessage[headerMessage.messageType];
        var balloonPopupContent = "";

        if (publicMessage[headerMessage.messageIdIndex] && publicMessage[headerMessage.messageIdIndex] != publicMessageId) {
            headerMessage.domElements.liveMessage.html(publicMessage[headerMessage.messageContentIndex]);
            headerMessage.domElements.liveMessage.data(url);

            // Work with FF, IE6-7
            if (publicMessageId > 0) {
                balloonPopupContent = publicMessage[headerMessage.messageContentIndex];
            }

            publicMessageId = publicMessage[headerMessage.messageIdIndex];
        }
    },

    showPrivateMessage: function (privateMessage) {
        // Process Private Message: show new private message
        if (privateMessage[headerMessage.messageIdIndex] && privateMessageId != privateMessage[headerMessage.messageIdIndex]) {
            var liveMessageContent = headerMessage.domElements.liveMessage.html();
            headerMessage.domElements.liveMessage.html(liveMessageContent + ". <font color=\"green\">Private message:" + privateMessage[headerMessage.messageContentIndex] + "</font>");

            if (privateMessageId > 0) {
                balloonPopupContent = privateMessage[headerMessage.messageContentIndex];
            }

            privateMessageId = privateMessage[headerMessage.messageIdIndex];
        }
    },

    processBallonPopup: function () {
        // Process Balloon Pop-up: show new public and private messages if existed
        if (balloonPopupContent != "") {
            headerMessage.domElements.popupContainer1.html(balloonPopupContent);
            self.showPopupTop(0);
            setTimeout(function () {
                self.showPopupTop(1);
            }, (30 * 100));
        }
    },

    getMessages: function () {
        $.ajax({
            type: "POST",
            url: headerMessage.urls.getMessages,
            responseType: "json",
            success: function (rawResults) {
                var results = convertReponseToJson(rawResults);

                try {
                    var publicMessage = results.public_message ? results.public_message : 0,
                        privateMessage = results.private_message ? results.private_message : 0,
                        unreadPriMsgCount = results.unread_private_messages ? results.unread_private_messages : 0;

                    unreadPriMsgCount = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;

                    headerMessage.domElements.privateMessageCounter.html(unreadPriMsgCount);

                    headerMessage.showPublicMessage(publicMessage);

                    headerMessage.showPrivateMessage(privateMessage);

                } catch (e) { }

                setTimeout(headerMessage.getMessages, (60 * 1000));
            }
        });
    },

    startSwapMessage: function () {
        if (headerMessage.messageIndex >= self.data.msgList.length - 1) {
            msgIndex = -1;
        }

        popupContents1Height = headerMessage.domElements.popupContainer1.height() + 20;
        popupContents2.innerHTML = self.data.msgList[++headerMessage.messageIndex];
        setTimeout(headerMessage.swapMessage, 50);
    },

    swapMessage: function () {
        popupContents1MarginTop -= 5;
        if (Math.abs(popupContents1MarginTop) >= popupContents1Height) {
            popupContents1MarginTop = 0;
            var parentNode = popupContents1.parentNode;
            parentNode.removeChild(popupContents1);
            parentNode.appendChild(popupContents1);
            var node2 = popupContents2;
            popupContents2 = popupContents1;
            popupContents1 = node2;

            popupContents1.style.marginTop = popupContents1MarginTop + "px";
            popupContents2.style.marginTop = popupContents1MarginTop + "px";

            $("#popup-top").css("height", popupContents1.clientHeight + 'px');
            return;
        }

        popupContents1.style.marginTop = popupContents1MarginTop + "px";
        setTimeout(self.swapMessage, 50);
    }
};

$(function () {
    headerMessage.init();
});