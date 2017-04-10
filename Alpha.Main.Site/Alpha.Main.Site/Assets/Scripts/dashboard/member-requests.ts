class MemberRequests extends DashboardBase {
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        super($);
        this.topWindow = topWindow;
    }

    init = () => {
        let totalRequest = this.$('#totalRequest').val();
        this.$('.new-request').html("(" + totalRequest + ")");
        this.registerEvents();
    };

    registerEvents = () => {
        let self = this;
        this.$('#memberrequest-tab').on('click', function () {
            let position = self.$(this).position();
            self.$(".scroll-tabs").scrollLeft(position.left);
        });

        this.$('#icon-help').on('click', this.showGuideline);
    };
    showGuideline = () => {
        let title = this.$('#PlayForFunGuide').val();
        let url = this.$('#GuidelineUrl').val();
        this.topWindow.popupManager.open(url, title, 600, 900);
    };
}

$(() => {
    const memberRequests = new MemberRequests($, window.top);
    memberRequests.loadInfo('#member-requests', memberRequests.init);
})