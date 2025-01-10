declare interface Window {
    headerNav: any;
}

class WhiteLablePage {
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$("#lnk-livechat").click(() => this.showLiveChat());

    };
    showLiveChat = () => {
        top.window.headerNav.showLiveChat();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "WhiteLabel/BannerChat",
            data: {},
            success: function () { }
        });
    }
}

$(() => {
    var whiteLabelPage = new WhiteLablePage($);
    whiteLabelPage.init();
})