class ChatBotGuide {
    $: JQueryStatic;

    $reportDemo: JQuery;
    $reportBody: JQuery;
    $reportCommands: JQuery;
    $reportRefresh: JQuery;

    $notificationDemo: JQuery;
    $notificationBody: JQuery;
    $notificationCommands: JQuery;
    $notificationRefresh: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElement();
        this.registerEvents();
        this.showReportExample();
        this.showNotificationExample();
    }

    cacheDOMElement = () => {
        this.$reportDemo = this.$("#chatbotReport");
        this.$reportBody = this.$("#reportBody");
        this.$reportCommands = this.$("#reportCommands");
        this.$reportRefresh = this.$("#reportRefresh");

        this.$notificationDemo = this.$("#chatbotNotification");
        this.$notificationBody = this.$("#notificationBody");
        this.$notificationCommands = this.$("#notificationCommands");
        this.$notificationRefresh = this.$("#notificationRefresh");
    }

    registerEvents = () => {
        this.$reportRefresh.click(this.replayReportExample);

        this.$notificationRefresh.click(this.replayNotificationExample);
    }

    showReportExample = () => {
        this.showMessageChat(this.$reportCommands, this.$reportBody)
    }

    replayReportExample = () => {
        var self = this;
        this.$reportCommands.find(".command").each(function () {
            self.$(this).hide();
        });
        this.showMessageChat(this.$reportCommands, this.$reportBody);
    }

    showNotificationExample = () => {
        this.showMessageChat(this.$notificationCommands, this.$notificationBody);
    }

    replayNotificationExample = () => {
        var self = this;
        this.$notificationCommands.find(".command").each(function () {
            self.$(this).hide();
        });
        this.showMessageChat(this.$notificationCommands, this.$notificationBody);
    }

    showMessageChat = ($listCommand: JQuery, $chatbotBody: JQuery) => {
        var messageNumbers = $listCommand.find(".command").length;
        var currentHeight = $chatbotBody.prop("scrollHeight");

        for (var i = 1; i <= messageNumbers; i++) {
            this.showMessage($listCommand, i, currentHeight);
        }
    }

    showMessage = ($listCommands: JQuery, order, currentHeight) => {
        var firstMessage = 1;
        var self = this;
        setTimeout(function (order) {
            if (order === firstMessage) {
                setTimeout(function () {
                    self.messageWithDataInput($listCommands, order);
                }, 400);
            } else {
                $listCommands.parent(".chatbot-body").delay(1000).animate({
                    scrollTop: currentHeight
                }, 1200, function () {
                    self.messageWithDataInput($listCommands, order);
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