class DataProccessingNotification {
    $: JQueryStatic;
    $progressBox: JQuery;
    $dataNotificationModal: JQuery;
    protected progressBoxCollapseTimeout = 5000;//5s
    protected getMessageTimeout = 60000;//1min
    protected getMoveBetMessageUrl = alpha.urls.messageRootPath + "MarqueeMessage/MoveBet";
    startPosition: JQueryCoordinates;
    documentClick: boolean;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.cacheDOMElements();
        this.registerEvents();
        this.getMessage();
    }

    cacheDOMElements(): void {
        this.$progressBox = this.$("#progress-box");
        this.$dataNotificationModal = this.$("#data-processing-popup");
    }

    getMessage = () => {
        this.$.ajax({
            type: "GET",
            url: this.getMoveBetMessageUrl,
            success: this.onGetMessageSuccess
        });
    };

    onGetMessageSuccess = (data) => {
        if (data && data.Content) {
            this.$dataNotificationModal.find(".modal-body").html(data.Content);

            if (this.$progressBox.hasClass("hide")) {
                this.showProgressBox();
            }
        } else {
            this.$progressBox.addClass("hide");
        }

        setTimeout(this.getMessage, this.getMessageTimeout);
    };

    registerEvents(): void {
        this.$progressBox.click(this.onProgressBoxClick);

        this.$progressBox.on("touchstart", () => {
            this.startPosition = this.$progressBox.position();
        });

        this.$progressBox.on("touchend", (event) => {
            const endPosition = this.$progressBox.position();
            if (Math.abs(this.startPosition.top - endPosition.top) < 3
                && Math.abs(this.startPosition.left - endPosition.left) < 3) {
                this.onProgressBoxClick();
            }
        });
    }

    showProgressBox(): void {
        this.$progressBox
            .removeAttr("style")
            .removeClass("collapsed")
            .removeClass("hide")
            .draggable({ disabled: true });

        setTimeout(() => {
            this.$progressBox
                .addClass("collapsed")
                .draggable({ disabled: false });
        }, this.progressBoxCollapseTimeout);
    }

    onProgressBoxClick = () => {
        if (this.$progressBox.hasClass("collapsed")) {
            this.$dataNotificationModal.modal("show");
        }
    };
}

$(document).ready(() => {
    if (alpha.isScreenLower768) {
        const dataProcessingNotification = new DataProccessingNotification($);
        dataProcessingNotification.init();
    }
});
