var HeaderMessage = /** @class */ (function () {
    function HeaderMessage($) {
        var _this = this;
        this.publicMessageId = 0;
        this.privateMessageId = 0;
        this.scrollerLeft = 0;
        this.scrollerWidth = 0;
        this.maskWidth = 0;
        this.domElements = {
            privateMessage: null,
            publicMessage: null,
            moveBetsMessage: null,
            marqueeMessage: null,
            hotMessages: null,
            userMessageModal: null,
            currentHotMessage: null,
            scroller: null,
            contentMessage: null,
            showMoreContainer: null,
            showLessContainer: null
        };
        this.urls = {
            mainAppPrivateMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=1',
            mainAppPublicMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=0',
            mainAppSpecialMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=2',
            getMessages: alpha.urls.messageRootPath + 'MarqueeMessage/Index'
        };
        this.init = function () {
            _this.cacheElements();
            _this.registerEvents();
            _this.getMessages();
        };
        this.viewLiveMessage = function (event) {
            var messageType = parseInt(_this.$(event.currentTarget).attr('messageType'));
            headerNav.buildIconNewMessageByType(messageType);
            if (messageType == alpha.messageType.Normal) {
                indexManager.changeMenu(_this.urls.mainAppPublicMessageForm);
            }
            else if (messageType == alpha.messageType.Personal) {
                indexManager.changeMenu(_this.urls.mainAppPrivateMessageForm);
            }
            else if (messageType == alpha.messageType.Special) {
                indexManager.changeMenu(_this.urls.mainAppSpecialMessageForm);
            }
        };
        this.viewMoveBetsMessage = function (event) {
            event.stopPropagation();
        };
        this.resetHotMessageElements = function (showFullMessage) {
            var minMessageHeight = 40;
            _this.domElements.currentHotMessage.css('min-height', minMessageHeight);
            if (showFullMessage) {
                _this.domElements.showMoreContainer.addClass("hide");
                _this.domElements.currentHotMessage.removeClass("brief-content");
                _this.domElements.currentHotMessage.css('height', '');
            }
            else {
                _this.domElements.showLessContainer.addClass("hide");
                _this.domElements.currentHotMessage.removeClass("brief-content");
            }
        };
        this.setHotMessageElementStyleByMessageHeight = function (showFullMessage, messageHeight, maxMessageHeight) {
            if (showFullMessage) {
                if (messageHeight <= maxMessageHeight) {
                    _this.domElements.showLessContainer.addClass("hide");
                }
                else {
                    _this.domElements.showLessContainer.removeClass("hide");
                }
            }
            else {
                if (messageHeight <= maxMessageHeight) {
                    _this.domElements.showMoreContainer.addClass("hide");
                }
                else {
                    _this.domElements.currentHotMessage.addClass("brief-content").height(maxMessageHeight);
                    _this.domElements.showMoreContainer.removeClass("hide");
                }
            }
        };
        this.calculateHotMessageHeight = function (showFullMessage, maxMessageHeight) {
            var bodyBorderHeight = 2;
            var bodyPaddingHeight = 5 + bodyBorderHeight;
            var messageHeight = _this.domElements.currentHotMessage.height();
            _this.setHotMessageElementStyleByMessageHeight(showFullMessage, messageHeight, maxMessageHeight);
            if (maxMessageHeight < messageHeight) {
                messageHeight = showFullMessage ?
                    messageHeight + _this.domElements.showLessContainer.height() :
                    maxMessageHeight + _this.domElements.showMoreContainer.height();
            }
            messageHeight += bodyPaddingHeight;
            return messageHeight;
        };
        this.calculateHotMessagePopupHeight = function (showFullMessage) {
            _this.resetHotMessageElements(showFullMessage);
            var maxMessageHeight = 53;
            var messageHeight = _this.calculateHotMessageHeight(showFullMessage, maxMessageHeight);
            var headerHeight = 10;
            var popUpHeight = headerHeight + messageHeight;
            _this.domElements.userMessageModal.height(popUpHeight);
        };
        this.showMoreHotMessage = function () {
            _this.calculateHotMessagePopupHeight(true);
        };
        this.showLessHotMessage = function () {
            _this.calculateHotMessagePopupHeight(false);
        };
        this.getMessages = function () {
            _this.$.ajax({
                type: "POST",
                url: _this.urls.getMessages,
                success: _this.onSuccessGetMessages
            });
        };
        this.onSuccessGetMessages = function (rawResults) {
            var results = rawResults, movebetsMessage = results.MoveBetsInformation.Content || "";
            _this.showPublicMessage(results);
            _this.domElements.moveBetsMessage.html(movebetsMessage);
            _this.showPrivateMessage(results);
            _this.applyScroller();
            setTimeout(_this.getMessages, 1 * 60 * 1000);
        };
        this.showPublicMessage = function (results) {
            var publicMessage = results.PublicMessage ? results.PublicMessage : 0;
            // Process Public Message: show new public message and its link to message form
            var url = _this.urls.mainAppPublicMessageForm;
            if (publicMessage.Id
                && publicMessage.Id !== _this.publicMessageId) {
                _this.domElements.publicMessage
                    .html(publicMessage.Content + '. ')
                    .attr('messageType', publicMessage.MessageType);
                _this.publicMessageId = publicMessage.Id;
                if (_this.publicMessageId !== 0) {
                    _this.appendMessageToPopup(publicMessage.Content);
                }
            }
            else {
                var spanMoveBets = _this.$("#span-movebets");
                if (spanMoveBets.length) {
                    spanMoveBets.remove();
                }
            }
        };
        this.showPrivateMessage = function (results) {
            var privateMessage = results.PrivateMessage ? results.PrivateMessage : 0;
            // Process Private Message: show new private message
            var url = _this.urls.mainAppPrivateMessageForm;
            if (privateMessage.Id
                && _this.privateMessageId !== privateMessage.Id) {
                //// Append Private Message to Marquee with green color format.
                _this.domElements.privateMessage.html("<font id='private-message-content' color=\"green\">Private message: " +
                    privateMessage.Content +
                    "</font>")
                    .attr('messageType', privateMessage.MessageType);
                if (_this.privateMessageId !== 0) {
                    _this.appendMessageToPopup(privateMessage.Content);
                }
            }
        };
        this.appendMessageToPopup = function (message) {
            _this.domElements.contentMessage.append('<p class="not-shown-message"></p>');
            _this.domElements.contentMessage.find("p:last").html(message);
        };
        this.applyScroller = function () {
            if (_this.scrollerLeft) {
                return;
            }
            _this.scrollerLeft = 100;
            _this.scrollerWidth = _this.domElements.scroller.width();
            _this.scroller = _this.domElements.scroller[0];
            _this.privateMsg = _this.domElements.privateMessage[0];
            _this.publicMsg = _this.domElements.publicMessage[0];
            _this.privateMsg.onmouseover = _this.publicMsg.onmouseover = _this.domElements.moveBetsMessage[0].onmouseover = function () {
                _this.pauseMarquee();
            };
            _this.privateMsg.onmouseout = _this.publicMsg.onmouseout = _this.domElements.moveBetsMessage[0].onmouseout = function () {
                _this.playMarquee();
            };
            var textNews = _this.domElements.marqueeMessage[0];
            _this.maskWidth = textNews.offsetWidth;
            _this.scrollMessage();
        };
        this.playMarquee = function () {
            if (_this.scrollerPaused) {
                _this.scrollerPaused = false;
                _this.scrollMessageTimeout = setTimeout(_this.scrollMessage, 20);
            }
            else {
                _this.scrollerPaused = false;
            }
        };
        this.pauseMarquee = function () {
            clearTimeout(_this.scrollMessageTimeout);
            _this.scrollMessageTimeout = 0;
            _this.scrollerPaused = true;
        };
        this.scrollMessage = function () {
            if (_this.scrollerPaused) {
                return;
            }
            if (_this.scrollerLeft + _this.scrollerWidth < -5) {
                _this.scrollerLeft = _this.maskWidth + 5;
                _this.scrollerWidth = _this.domElements.scroller.width();
            }
            _this.scroller.style.left = (_this.scrollerLeft--) + 'px';
            _this.scrollMessageTimeout = setTimeout(_this.scrollMessage, 20);
        };
        this.cacheElements = function () {
            _this.domElements.privateMessage = _this.$('#private-message');
            _this.domElements.publicMessage = _this.$('#public-message');
            _this.domElements.moveBetsMessage = _this.$('#movebets-message');
            _this.domElements.marqueeMessage = _this.$('#marqueeMessage');
            _this.domElements.hotMessages = _this.$('#userMessageModalBody');
            _this.domElements.contentMessage = _this.$('#msg-content');
            _this.domElements.userMessageModal = _this.$('#userMessageModal');
            _this.domElements.currentHotMessage = _this.domElements.hotMessages.find('p.current-hot-message');
            _this.domElements.scroller = _this.$('#scroller');
            _this.domElements.showMoreContainer = _this.$(".showmore-container");
            _this.domElements.showLessContainer = _this.$(".showless-container");
        };
        this.registerEvents = function () {
            _this.domElements.privateMessage.on('click', _this.viewLiveMessage);
            _this.domElements.publicMessage.on('click', _this.viewLiveMessage);
            _this.domElements.moveBetsMessage.on('click', _this.viewMoveBetsMessage);
            _this.domElements.showMoreContainer.on('click', _this.showMoreHotMessage);
            _this.domElements.showLessContainer.on('click', _this.showLessHotMessage);
        };
        this.$ = $;
    }
    return HeaderMessage;
}());
$(function () {
    if (alpha.isScreenLower768) {
        return;
    }
    var headerMessage = new HeaderMessage($);
    headerMessage.init();
});
//# sourceMappingURL=header-msg.js.map