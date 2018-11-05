class BaseChatBot {
    $: JQueryStatic;
    topWindow: any;
    expiredTimeKey = "ChatbotVerificationExpiredTime";
    $openReportExample: JQuery;
    $openNotificationExample: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    baseResources = {
        reportExample: (<HTMLInputElement>document.getElementById('ReportExample')).value,
        notificationExample: (<HTMLInputElement>document.getElementById('NotificationExample')).value
    }

    onComplete = () => {
        this.$.unblockUI();
    }

    safelyParseJSON = (json) => {
        var parsed;
        try {
            parsed = JSON.parse(json);
        } catch (e) { }

        return parsed;
    }

    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    }

    updateExpiredTime = () => {
        var currentTime = new Date();
        var expiredTime = currentTime.getTime() + (3 * 60000);//3 minutes
        localStorage.setItem(this.expiredTimeKey, expiredTime.toString());
    }

    cacheDOMAndRegisterFunctionExample = (getLanguageFunc: Function) => {
        this.$openReportExample = this.$("#openReportExample");
        this.$openReportExample.on('click touchend', () => {
            this.openExample(this.baseResources.reportExample, "ChatBot/ReportExample", 400, getLanguageFunc());
        });

        this.$openNotificationExample = this.$("#openNotificationExample");
        this.$openNotificationExample.on('click touchend', () => {
            this.openExample(this.baseResources.notificationExample, "ChatBot/NotificationExample", 336, getLanguageFunc());
        });
    }

    openExample = (title: string, url: string, height: number, language: string) => {
        var src = alpha.urls.mainRootPath + url + "?language=" + language;

        this.topWindow.bootbox.dialog({
            title: title,
            message: '<iframe height="' + height + 'px" width="100%" src="' + src + '"></iframe>'
        });

        var dialog = this.topWindow.$(".bootbox .modal-dialog");
        dialog.width(400);
        dialog.css({
            left: this.topWindow.$(this.topWindow.window).width() / 2 - dialog.width() / 2,
            top: 100
        });
    }
}