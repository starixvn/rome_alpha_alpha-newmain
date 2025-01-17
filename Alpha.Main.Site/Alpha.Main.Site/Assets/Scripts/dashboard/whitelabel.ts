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
        this.$(".lnk-skype").click(() => this.showSkype());
        this.$(".lnk-letstalk").click(() => this.showLetTalk());

    };
    showLiveChat = () => {
        // top.window.headerNav.showLiveChat();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "WhiteLabel/BannerChat?username=" + this.$("#Username").val(),
            data: {},
            success: function () { }
        });
    };
    showSkype = () => {
        window.open('skype:' + this.$("#SkypeId").val() + '?chat');
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "WhiteLabel/SkypeChat?username=" + this.$("#Username").val(),
            data: {},
            success: function () { }
        });
    };
    showLetTalk = () => {
    };
}

$(() => {
    var whiteLabelPage = new WhiteLablePage($);
    whiteLabelPage.init();
})