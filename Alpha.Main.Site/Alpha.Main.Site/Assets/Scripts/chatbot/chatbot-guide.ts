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
        var delay = 0;
        for (var i = 1; i <= messageNumbers; i++) {
            this.showMessage(i, delay);
            delay = delay + 2000;
        }

        var interval = setInterval(() => {
            clearInterval(interval);
            this.$btnPlaying.addClass("hide");
            this.$btnReplay.removeClass("hide");
        }, 15000)
    }

    showMessage = (order, delay) => {
        var firstMessage = 1;
        var self = this;

        setTimeout(function (order, delay) {
            self.messageWithDataInput(self.$listCommand, order, delay);
        }, (1000), order, delay);
    }

    messageWithDataInput = ($listCommands: JQuery, order, delay) => {
        var chatContainer = $listCommands.find(".command:nth-child(" + order + ")");
        var reportText = null;
        setTimeout(() => {
            if (this.$('#reportText') && chatContainer.find('.user-input')) {
                reportText = this.$('#reportText');
                var chatContent = chatContainer.find('.user-input').text();
                reportText.val(chatContent);
            }
        }, delay);

        setTimeout(() => {
            if (this.$('#reportText')) {
                reportText = this.$('#reportText');
                reportText.val('');
            }

            chatContainer.show("drop", {
                direction: "down",
                easing: 'easeInOutExpo'
            }, 1000);

            this.$chatbotBody.delay(1000).animate({
                scrollTop: this.$chatbotBody.get(0).scrollHeight
            }, 1200);
        }, delay + 1000);
    }
}

$(function () {
    var chatbotGuide = new ChatBotGuide($);
    chatbotGuide.init();
});