var ChatBotGuide = /** @class */ (function () {
    function ChatBotGuide($) {
        var _this = this;
        this.init = function () {
            _this.cacheDOMElement();
            _this.registerEvents();
            _this.showMessageChat();
        };
        this.cacheDOMElement = function () {
            _this.$chatbotLayout = _this.$("#chatbotLayout");
            _this.$chatbotBody = _this.$("#chatbotBody");
            _this.$listCommand = _this.$("#listCommand");
            _this.$btnReplay = _this.$("#btnReplay");
            _this.$btnPlaying = _this.$("#btnPlaying");
        };
        this.registerEvents = function () {
            _this.$btnReplay.click(_this.replayExample);
        };
        this.replayExample = function () {
            _this.$btnReplay.addClass("hide");
            _this.$btnPlaying.removeClass("hide");
            var self = _this;
            _this.$listCommand.find(".command").each(function () {
                self.$(this).hide();
            });
            _this.showMessageChat();
        };
        this.showMessageChat = function () {
            var messageNumbers = _this.$listCommand.find(".command").length;
            var delay = 0;
            for (var i = 1; i <= messageNumbers; i++) {
                _this.showMessage(i, delay);
                delay = delay + 2000;
            }
            var interval = setInterval(function () {
                clearInterval(interval);
                _this.$btnPlaying.addClass("hide");
                _this.$btnReplay.removeClass("hide");
            }, 15000);
        };
        this.showMessage = function (order, delay) {
            var firstMessage = 1;
            var self = _this;
            setTimeout(function (order, delay) {
                self.messageWithDataInput(self.$listCommand, order, delay);
            }, (1000), order, delay);
        };
        this.messageWithDataInput = function ($listCommands, order, delay) {
            var chatContainer = $listCommands.find(".command:nth-child(" + order + ")");
            var reportText = null;
            setTimeout(function () {
                if (_this.$('#reportText') && chatContainer.find('.user-input')) {
                    reportText = _this.$('#reportText');
                    var chatContent = chatContainer.find('.user-input').text();
                    reportText.val(chatContent);
                }
            }, delay);
            setTimeout(function () {
                if (_this.$('#reportText')) {
                    reportText = _this.$('#reportText');
                    reportText.val('');
                }
                chatContainer.show("drop", {
                    direction: "down",
                    easing: 'easeInOutExpo'
                }, 1000);
                _this.$chatbotBody.delay(1000).animate({
                    scrollTop: _this.$chatbotBody.get(0).scrollHeight
                }, 1200);
            }, delay + 1000);
        };
        this.$ = $;
    }
    return ChatBotGuide;
}());
$(function () {
    var chatbotGuide = new ChatBotGuide($);
    chatbotGuide.init();
});
//# sourceMappingURL=chatbot-guide.js.map