declare var toastr: Toastr;

class ToastMessage {
    getMessagesUrl = alpha.urls.messageRootPath + "ToastMessage/Get";
    markreadMessagesUrl = alpha.urls.messageRootPath + "ToastMessage/MarkRead";
    getMessagesTimeout = 1000 * 60 * 5; // 5 minute

    $: JQueryStatic;
    topWindow: any;
    $here: string;
    $minAgo: string;
    $minsAgo: string;
    $hoursAgo: string;
    $daysAgo: string;

    monitorURL: string;
    showingToasts: object;
    toastId: number;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public init() {
        this.$minAgo = this.$("#minAgo").val();
        this.$minsAgo = this.$("#minsAgo").val();
        this.$hoursAgo = this.$("#hoursAgo").val();
        this.$daysAgo = this.$("#daysAgo").val();
        this.$here = this.$("#Here").val().toUpperCase();
        this.monitorURL = alpha.urls.transferRootPath + "AutoTransfer/Monitoring?Status=Failed";
        this.showingToasts = {};
        this.toastId = 0;

        this.getMessages();
        setInterval(() => { this.updateMinsDiff(); }, 10000);
    }

    private getMessages() {
        const self = this;

        this.$.get(this.getMessagesUrl, { LastToastId: this.toastId }, function (data) {
            self.renderMessages(data);
        }).always(function () {
            setTimeout(function () { self.getMessages(); }, self.getMessagesTimeout);
        });
    }

    private renderMessages(data) {
        if (!data || data.length === 0) {
            return;
        }

        var message = data[0];
        this.toastId = message.ToastId;
        this.renderMessage(message);
    }

    private renderMessage(message) {
        if (this.showingToasts[message.ToastId]) {
            return;
        }

        Object.keys(this.showingToasts).forEach(k => {
            var t = this.showingToasts[k];
            toastr.clear(t);
        });

        var self = this;
        var optionsOverride = {
            tapToDismiss: false,
            closeOnHover: false,
            timeOut: 0,
            positionClass: 'toast-bottom-right',
            closeButton: true,
            onCloseClick: function () { self.closeMessage(this, message.ToastId); }
        };

        var title = '<span>' + message.Title + '</span> <span class="mins-diff" data-org-time="' + message.CreatedTimeStr + '">' + this.getMinDiffs(message.CreatedTimeStr) + '</span>';
        var content = this.formatContent(message);
        var t1;

        switch (message.TypeId) {
            case 3:
                t1 = toastr.warning(content, title, optionsOverride);
                break;
            case 4:
                t1 = toastr.error(content, title, optionsOverride);
                break;
        }

        var displayInfo = this.getDisplayInfo();
        t1.css(displayInfo);

        this.showingToasts[message.ToastId] = t1;
    }

    private closeMessage(elm, id) {
        this.showingToasts[id] = null;
        const self = this;

        this.$.post(this.markreadMessagesUrl, { id: id }, function (data) {
            if (data.ErrCode) {
                self.topWindow.bootboxAlert(data.ErrMsg);
            }
        }).always(function () {
        });
    }

    private formatContent(message) {
        var type = message.SiteFunctionId;
        var content = message.Body;
        var format = this['format_' + type];
        return format ? format.call(this, content) : content;
    }

    private format_1(content) {
        return content.replace("{0}", "<a target='main' href='" + this.monitorURL + "'>" + this.$here + "</a>");
    }

    private getDisplayInfo() {
        var feedback = this.$("#feedback-function");
        if (feedback.length && feedback.hasClass('hide')) {
            return {
                bottom: 0,
                width: 350
            }
        }

        return {
            bottom: feedback.height(),
        };
    }

    public updatePosition() {
        var displayInfo = this.getDisplayInfo();

        Object.keys(this.showingToasts).forEach(k => {
            var t = this.showingToasts[k];
            t.css(displayInfo);
        });
    }

    public updateMinsDiff() {
        var $minsDiff = this.$(".mins-diff");
        $minsDiff.each((_, e) => {
            var $e = this.$(e);

            var createdTime = $e.attr("data-org-time");
            var txtDiff = this.getMinDiffs(createdTime);
            if (txtDiff !== $e.html()) {
                $e.html(txtDiff);
            }
        });
    }

    private getMinDiffs(createdTime) {
        var now = new Date();
        var currentTime = now.getTime();
        var orgTime = new Date(createdTime).getTime();

        var diffs = Math.floor((currentTime - orgTime) / 60000 - 4 * 60 + now.getTimezoneOffset());
        var txtDiff = this.getAgoText(diffs);
        return txtDiff;
    }

    private getAgoText(minDiffs) {
        if (minDiffs <= 1) {
            return this.$minAgo;
        }

        if (minDiffs < 60) {
            return minDiffs + " " + this.$minsAgo;
        }
        if (minDiffs < 24 * 60) {
            return Math.floor(minDiffs / 60) + " " + this.$hoursAgo;
        }

        return Math.floor(minDiffs / 60 / 24) + " " + this.$daysAgo;
    }
}

const toastMessage = new ToastMessage($, window.top);

$(function () {
    toastMessage.init();
});
