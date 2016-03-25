var headerNotificationManager;

$(document).ready(function () {
    headerNotificationManager.init();
});

var HeaderNotificationManager = function () {
    var self = this;
    self.data = {
        scroller_left: undefined,
        scroller_width: 0,
        scroller: null,
        topnews: null,
        scrollerPaused: false
    };

    var popupContents1 = null;
    var popupContents2 = null;
    var popupContents1Height = 0;
    var popupContents1MarginTop = 0;

    var msgIndex = 0;
    var publicMessageId = 0; // public message
    var privateMessageId = 0; // private message
    var scrollMessageTimeout = 0;

    self.init = function () {
        self.bindEvents();
        self.showHotMsg(_resources.msgList);
    };

    self.bindEvents = function () {
        $(".top-message").click(function () {
            self.showPopupTop(false);
        });

        $(".live-message").click(function () {
            indexPageManager.changeMenu("/" + top.baseOldMainApp + 'Info/NewMessageForm.aspx?pageQuery=', this);
        });
        $(".special-message").click(function () {
            indexPageManager.changeMenu("/" + top.baseOldMainApp + 'Info/NewMessageForm.aspx?pageQuery=', this);
        });

        $(".private-message").click(function () {
            self.viewPrivateMessage("/" + top.baseOldMainApp + 'Info/NewMessageForm.aspx?pageQuery=private');
        });

        setTimeout(self.getMessages, 500);
    };
    self.viewPrivateMessage = function (url) {
        indexPageManager.changeMenu(url);
        var countPrivateMsg = $('.private-message .value').html().replace("+", "");
        if (parseInt(countPrivateMsg) > 0) {
            self.updateCountPrivateMsg();
        }
    };
    self.updateCountPrivateMsg = function () {
        var url = '/' + top.baseOldMainApp + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx';
        $.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                data = convertReponseToJson(data);
                if (data.errCode == 0) {
                    $('.private-message .value').html(0);
                }
            }
        });
    };
    self.showHotMsg = function (msgs) {
        var topPopup = $("#popup-top");
        if (msgs.length == 0) {
            topPopup.addClass('display-none');
            return;
        }

        popupContents1 = $('#popup-contents')[0];
        popupContents2 = $('#popup-contents-2')[0];
        popupContents1.innerHtml = msgs[msgIndex];
        if (msgs.length > 1) {
            popupContents2.innerHtml = msgs[msgIndex + 1];
        }

        self.showPopupTop(true);
        topPopup.css("height", popupContents1.clientHeight + 'px');
        if (msgs.length > 1) {
            setInterval(self.swapMessage_Start, 18000);
        }
    };
    self.showPopupTop = function (show) {
        var topPopup = $("#popup-top");
        if (show) {
            topPopup.removeClass('display-none');
        } else {
            topPopup.addClass('display-none');
        }
    };
    self.getMessages = function () {
        var url = "/" + top.baseOldMainApp + '_UserMessage/Handlers/GetMessages.ashx';
        $.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (results) {
                results = convertReponseToJson(results);
                try {
                    var publicMessage = (results.public_message != null && results.public_message != 'undefined') ? results.public_message : 0;
                    var privateMessage = (results.private_message != null && results.private_message != 'undefined') ? results.private_message : 0;
                    var unreadPriMsgCount = (results.unread_private_messages != null && results.unread_private_messages != 'undefined') ? results.unread_private_messages : 0;

                    unreadPriMsgCount = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;
                    $('.private-message .value').html(unreadPriMsgCount);

                    var msgId = 0,
                        msgContent = 1,
                        msgType = 2;

                    // Process Public Message: show new public message and its link to message form
                    var url = '/' + top.baseOldMainApp + "Info/NewMessageForm.aspx?pageQuery=";
                    var balloonPopupContent = "";
                    url += publicMessage[msgType];

                    if ((publicMessage[msgId] != null) && (publicMessageId != publicMessage[msgId])) {
                        $('#live-message').html(publicMessage[msgContent]);
                        $('#marquee').click(function () {
                            indexPageManager.changeMenu(url);
                        });

                        // Work with FF, IE6-7
                        if (publicMessageId > 0) {
                            balloonPopupContent = publicMessage[msgContent];
                        }

                        publicMessageId = publicMessage[msgId];
                    }

                    // Process Private Message: show new private message
                    if ((privateMessage[msgId] != null) && (privateMessageId != privateMessage[msgId])) {
                        var liveMessageContent = $('#live-message').html();
                        $('#live-message').html(liveMessageContent + ". <font color=\"green\">Private message:" + privateMessage[msgContent] + "</font>");
                        if (privateMessageId > 0) {
                            balloonPopupContent = privateMessage[msgContent];
                        }

                        privateMessageId = privateMessage[msgId];
                    }

                    // Process Balloon Pop-up: show new public and private messages if existed
                    if (balloonPopupContent != "") {
                        $("#popup-contents").html(balloonPopupContent);
                        self.showPopupTop(0);
                        setTimeout(function () {
                            self.showPopupTop(1);
                        }, (30 * 100));
                    }

                    self.applyScroller();
                } catch (e) { }

                setTimeout(self.getMessages, (60 * 1000));
            }
        });
    };
    self.swapMessage_Start = function () {
        if (msgIndex >= _resources.msgList.length - 1) {
            msgIndex = -1;
        }

        popupContents1Height = popupContents1.clientHeight + 20;
        popupContents2.innerHTML = _resources.msgList[++msgIndex];
        setTimeout(self.swapMessage, 50);
    };
    self.swapMessage = function () {
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
    };
    self.applyScroller = function () {
        if (typeof self.data.scroller_left != 'undefined') {
            // Apply first time only
            return;
        }

        self.data.scroller_left = -100;
        self.data.scroller_width = 0;

        self.data.scroller = $('#scroller')[0];
        self.data.topnews = $('#live-message')[0];

        self.data.topnews.onmouseover = function () {
            clearTimeout(scrollMessageTimeout);
            scrollMessageTimeout = 0;
            self.data.scrollerPaused = true;
        };
        self.data.topnews.onmouseout = function () {
            if (self.data.scrollerPaused) {
                self.data.scrollerPaused = false;
                scrollMessageTimeout = setTimeout(self.scrollMessage, 20);
            } else {
                self.data.scrollerPaused = false;
            }
        };

        var text_news = $('#marquee')[0];
        self.data.maskWidth = parseInt(text_news.offsetWidth);

        self.scrollMessage();
    };
    self.scrollMessage = function () {
        if (self.data.scrollerPaused) {
            return;
        }

        if (self.data.scroller_left + self.data.scroller_width < -5) {
            self.data.scroller_left = self.data.maskWidth + 5;
            self.data.scroller_width = parseInt(self.data.topnews.offsetWidth);
        }

        self.data.scroller.style.left = (self.data.scroller_left--) + 'px';
        scrollMessageTimeout = setTimeout(self.scrollMessage, 20);
    };
};

var headerNotificationManager = new HeaderNotificationManager();