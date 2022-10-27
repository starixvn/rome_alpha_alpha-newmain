class PopupAdvertisement {
    private $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    private getMyWatchlist() {
        this.$.get(
            alpha.urls.mainRootPath + "Advertisement/MyWatchlist",
            (data) => {
                if (data.length) {
                    this.$("#advertisement-modal-body").html(data);
                    this.$("#advertisement-modal")
                        .modal({ show: true, backdrop: false })
                        .tinyDraggable({ exclude: "button.icon.icon-close.close, h3, p, img, button, label, input[type='checkbox']" });
                }
            });
    }

    init() {
        const userLevel = $("#userLevel");
        userLevel.length && userLevel.val() === "agent" && this.getMyWatchlist();
    }
}

$(function () {
    new PopupAdvertisement($).init();
});
