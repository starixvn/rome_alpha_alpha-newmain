var BaseChatBot = /** @class */ (function () {
    function BaseChatBot($, topWindow) {
        var _this = this;
        this.expiredTimeKey = "ChatbotVerificationExpiredTime";
        this.baseResources = {
            reportExample: document.getElementById('ReportExample').value,
            notificationExample: document.getElementById('NotificationExample').value,
            actionExample: document.getElementById('ActionExample').value
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.safelyParseJSON = function (json) {
            var parsed;
            try {
                parsed = JSON.parse(json);
            }
            catch (e) { }
            return parsed;
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.updateExpiredTime = function () {
            var currentTime = new Date();
            var expiredTime = currentTime.getTime() + (3 * 60000); //3 minutes
            localStorage.setItem(_this.expiredTimeKey, expiredTime.toString());
        };
        this.cacheDOMAndRegisterFunctionExample = function (getLanguageFunc) {
            _this.$openReportExample = _this.$("#openReportExample");
            _this.$openReportExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.reportExample, "ChatBot/ReportExample", 400, getLanguageFunc());
            });
            _this.$openNotificationExample = _this.$("#openNotificationExample");
            _this.$openNotificationExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.notificationExample, "ChatBot/NotificationExample", 336, getLanguageFunc());
            });
            _this.$openActionExample = _this.$("#openActionExample");
            _this.$openActionExample.on('click touchend', function () {
                _this.openExample(_this.baseResources.actionExample, "ChatBot/ActionExample", 400, getLanguageFunc());
            });
        };
        this.openExample = function (title, url, height, language) {
            var src = alpha.urls.mainRootPath + url + "?language=" + language;
            _this.topWindow.bootbox.dialog({
                title: title,
                message: '<iframe height="' + height + 'px" width="100%" src="' + src + '"></iframe>'
            });
            var dialog = _this.topWindow.$(".bootbox .modal-dialog");
            dialog.width(400);
            dialog.css({
                left: _this.topWindow.$(_this.topWindow.window).width() / 2 - dialog.width() / 2,
                top: 100
            });
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return BaseChatBot;
}());
//# sourceMappingURL=base-chatbot.js.map