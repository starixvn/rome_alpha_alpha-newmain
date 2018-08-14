class ChatBotGuide {
    $: JQueryStatic;
    $chatbotLayout: JQuery;
    $chatbotBody: JQuery;
    $listCommand: JQuery;
    $btnReplay: JQuery;
    $btnPlaying: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElement();
        this.registerEvents();
        this.showMessageChat();
    }

    cacheDOMElement = () => {
        this.$chatbotLayout = this.$("#chatbotLayout");
        this.$chatbotBody = this.$("#chatbotBody");
        this.$listCommand = this.$("#listCommand");
        this.$btnReplay = this.$("#btnReplay");
        this.$btnPlaying = this.$("#btnPlaying");
    }

    registerEvents = () => {
        this.$btnReplay.click(this.replayExample);
    }

    replayExample = () => {
        this.$btnReplay.addClass("hide");
        this.$btnPlaying.removeClass("hide");
        var self = this;
        this.$listCommand.find(".command").each(function () {
            self.$(this).hide();
        });

        this.showMessageChat();
    }

    showMessageChat = () => {
        var messageNumbers = this.$listCommand.find(".command").length;
        var currentHeight = this.$chatbotBody.prop("scrollHeight");
        for (var i = 1; i <= messageNumbers; i++) {
            this.showMessage(i, currentHeight);
        }

        setInterval(() => {
            this.$btnPlaying.addClass("hide");
            this.$btnReplay.removeClass("hide");
        }, 15000)
    }

    showMessage = (order, currentHeight) => {
        var firstMessage = 1;
        var self = this;

        setTimeout(function (order) {
            if (order === firstMessage) {
                setTimeout(function () {
                    self.messageWithDataInput(self.$listCommand, order);
                }, 400);
            } else {
                self.$listCommand.parent(".chatbot-body").delay(1000).animate({
                    scrollTop: currentHeight
                }, 1200, function () {
                    self.messageWithDataInput(self.$listCommand, order);
                });
            }
        }, (10), order);
    }

    messageWithDataInput = ($listCommands: JQuery, order) => {
        var chatContainer = $listCommands.find(".command:nth-child(" + order + ")");
        var reportText = null;

        if (this.$('#reportText') && chatContainer.find('.user-input')) {
            reportText = this.$('#reportText');
            var chatContent = chatContainer.find('.user-input').text();
            reportText.val(chatContent);
        }

        setTimeout(function () {
            if (this.$('#reportText')) {
                reportText.val('');
            }

            chatContainer.show("drop", {
                direction: "down",
                easing: 'easeInOutExpo'
            }, 1000);
        }, 1000);
    }
}

$(function () {
    var chatbotGuide = new ChatBotGuide($);
    chatbotGuide.init();
});