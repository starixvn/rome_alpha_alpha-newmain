class PopupAdvertisement {
    private $: JQueryStatic;
    private dontShowAgainKey;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    private getMyWatchlist() {
        this.$.get(
            alpha.urls.mainRootPath + "Advertisement/MyWatchlist",
            (data) => {
                if (data.length) {
                    const popup = this.$("#advertisement-modal");
                    popup.find("#advertisement-modal-body").html(data);

                    popup.on("shown.bs.modal", () => this.bindMyWatchlistEvents(popup));
                    popup.modal({ show: true, backdrop: false })
                        .tinyDraggable({ exclude: "h3, ol, li, p, img, label, input[type='checkbox'], a, button" });
                }
            });
    }

    private bindMyWatchlistEvents(popup) {
        const chkDontShowAgain = popup.find("#chb-dontShow");
        chkDontShowAgain.change(() => this.setDontShowAgainLocalStorage(chkDontShowAgain.is(":checked")));

        popup.find("#btn-setup").click(() => {
            popup.modal("hide");
            this.activateSidebarMenu(this.$("#menu-MYWATCHLIST").find(".menu-opener"));
        });
    }

    private setDontShowAgainLocalStorage(value: boolean) {
        alpha.localStorage.set(this.dontShowAgainKey, value ? "1" : "0");
    }

    private getDontShowAgainLocalStorage(): boolean {
        return alpha.localStorage.get(this.dontShowAgainKey) === "1";
    }

    private activateSidebarMenu(menu) {
        menu.trigger("click");
        this.$(".sub-menu-item").removeClass("active");

        const mainIframe = this.$("#main");
        try {
            if (!alpha.core.isIe()) {
                const mainIframeContent = mainIframe.contents();
                mainIframeContent.find("head").append(this.$("#AssetCommonCssLink").val());
                mainIframeContent.find("body").empty().block();
            }
        } catch (e) { }

        mainIframe.attr("src", menu.attr("href")).attr("target", "main");
    }

    init() {
        this.dontShowAgainKey = [
            "AdvertisementDontShowAgainKey",
            this.$("#userId").val(),
            this.$("#subUserId").val()
        ].join("_");

        this.$("#userLevel").val() === "agent"
            && this.$("#subPageAccess").val().includes("[H]")
            && !this.getDontShowAgainLocalStorage()
            && this.getMyWatchlist();
    }
}

$(function () {
    new PopupAdvertisement($).init();
});
