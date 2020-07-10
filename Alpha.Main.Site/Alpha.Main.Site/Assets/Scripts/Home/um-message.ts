class UMMessage {
    $: JQueryStatic;
    $umMessageBox: JQuery;
    $umMessagePopup: JQuery;
    $umMessagePopupBody: JQuery;
    umMessageData: Array<number> = [];
    getUMMessagesTimeout = 1000 * 60; // 1min
    getUMMessagesUrl = alpha.urls.mainRootPath + "UMMessage/Get";
    doNotShowAgainKey = "UMMessageDoNotShowAgain";

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public init() {
        this.cacheDOMElements();
        this.registerEvents();
        this.getUMMessages();
    }

    private cacheDOMElements() {
        this.$umMessageBox = this.$("#um-message-box");
        this.$umMessagePopup = this.$("#um-message-popup");
        this.$umMessagePopupBody = this.$umMessagePopup.find(".modal-body");
    }

    private registerEvents() {
        let startPosition: JQueryCoordinates;

        this.$umMessageBox.on("mousedown touchstart", () => {
            startPosition = this.$umMessageBox.position();
        });

        this.$umMessageBox.on("mouseup touchend", (event) => {
            event.preventDefault();
            const endPosition = this.$umMessageBox.position();

            if (Math.abs(startPosition.top - endPosition.top) < 3
                && Math.abs(startPosition.left - endPosition.left) < 3) {
                this.$umMessagePopup.modal("show");
            }
        });

        this.$umMessageBox.draggable({ disabled: false, containment: "body" });

        const chkDontShowAgain = this.$umMessagePopup.find("#chb-um");
        chkDontShowAgain.change(() => {
            this.setDontShowAgainLocalStorage(chkDontShowAgain.is(":checked"));
        });

        this.$umMessagePopup.on("show.bs.modal", () => {
            chkDontShowAgain.prop("checked", this.getDontShowAgainLocalStorage());
        });
    }

    private getUMMessages() {
        const self = this;

        this.$.get(this.getUMMessagesUrl, function (data) {
            self.getUMMessagesSuccessfully(data);
        }).always(function () {
            setTimeout(function () { self.getUMMessages(); }, self.getUMMessagesTimeout);
        });
    }

    private getUMMessagesSuccessfully(data) {
        if (data.length) {
            const umMessage = this.extractUMMessage(data);
            this.$umMessagePopupBody.html(umMessage.message)

            if (umMessage.hasNewMessage) {
                this.getDontShowAgainLocalStorage() || this.$umMessagePopup.modal("show");
                this.$umMessageBox.removeClass("hide");
            }
        } else {
            this.umMessageData.length = 0;
            this.$umMessagePopupBody.html("");
            this.$umMessagePopup.modal("hide");
            this.$umMessageBox.addClass("hide");
        }
    }

    private extractUMMessage(data): any {
        let idArray: Array<number> = [];
        let messageArray: Array<string> = [];
        let hasNewMessage = false;

        for (var i = 0, max = data.length; i < max; i++) {
            const datum = data[i];
            idArray.push(datum.Id);
            messageArray.push(datum.Message);

            if (this.umMessageData.indexOf(datum.Id) === -1) {
                hasNewMessage = true;
            }
        }

        this.umMessageData = idArray;

        return {
            hasNewMessage: hasNewMessage,
            message: messageArray.join("<br/><br/>")
        };
    }

    private setDontShowAgainLocalStorage(value: boolean) {
        alpha.localStorage.set(this.doNotShowAgainKey, value ? "1" : "0");
    }

    private getDontShowAgainLocalStorage(): boolean {
        return alpha.localStorage.get(this.doNotShowAgainKey) === "1";
    }
}

$(function () {
    new UMMessage($).init();
});
