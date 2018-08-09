class ChatBotGuide {
    $: JQueryStatic;

    $playReportExample: JQuery;
    $closeReportExample: JQuery;
    $reportDemo: JQuery;
    $reportBody: JQuery;
    $reportCommands: JQuery;
    $reportRefresh: JQuery;
    $reportClose: JQuery;

    $playNotificationExample: JQuery;
    $closeNotificationExample: JQuery;
    $notificationDemo: JQuery;
    $notificationBody: JQuery;
    $notificationCommands: JQuery;
    $notificationRefresh: JQuery;
    $notificationClose: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElement();
        this.registerEvents();
    }

    cacheDOMElement = () => {
        this.$playReportExample = this.$("#playReportExample");
        this.$closeReportExample = this.$("#closeReportExample");
        this.$reportDemo = this.$("#chatbotReport");
        this.$reportBody = this.$("#reportBody");
        this.$reportCommands = this.$("#reportCommands");
        this.$reportRefresh = this.$("#reportRefresh");
        this.$reportClose = this.$("#reportClose");

        this.$playNotificationExample = this.$("#playNotificationExample");
        this.$closeNotificationExample = this.$("#closeNotificationExample");
        this.$notificationDemo = this.$("#chatbotNotification");
        this.$notificationBody = this.$("#notificationBody");
        this.$notificationCommands = this.$("#notificationCommands");
        this.$notificationRefresh = this.$("#notificationRefresh");
        this.$notificationClose = this.$("#notificationClose");
    }

    registerEvents = () => {
        this.$playReportExample.click(this.showReportExample);
        this.$closeReportExample.click(this.closeReportExample);
        this.$reportClose.click(this.closeReportExample);
        this.$reportRefresh.click(this.replayReportExample);

        this.$playNotificationExample.click(this.showNotificationExample);
        this.$closeReportExample.click(this.closeNotificationExample);
        this.$notificationClose.click(this.closeNotificationExample);
        this.$notificationRefresh.click(this.replayNotificationExample);
    }

    showReportExample = () => {
        var self = this;
        this.$reportDemo.slideToggle('fast', function () {
            self.$playReportExample.hide();
            self.$closeReportExample.show();
            self.showMessageChat(self.$reportCommands, self.$reportBody)
        });
    }

    closeReportExample = () => {
        var self = this;
        this.$reportDemo.slideToggle('fast', function () {
            self.$closeReportExample.hide();
            self.$playReportExample.show();
            self.$reportCommands.find(".command").each(function () {
                self.$(this).hide();
            })
        });
    }

    replayReportExample = () => {
        var self = this;
        this.$reportCommands.find(".command").each(function () {
            self.$(this).hide();
        });
        this.showMessageChat(this.$reportCommands, this.$reportBody);
    }

    showNotificationExample = () => {
        var self = this;
        this.$notificationDemo.slideToggle('fast', function () {
            self.$playNotificationExample.hide();
            self.$closeNotificationExample.show();
            self.showMessageChat(self.$notificationCommands, self.$notificationBody)
        });
    }

    closeNotificationExample = () => {
        var self = this;
        this.$notificationDemo.slideToggle('fast', function () {
            self.$playNotificationExample.hide();
            self.$closeNotificationExample.show();
            self.$notificationCommands.find(".command").each(function () {
                self.$(this).hide();
            })
        });
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