class MemberRequests extends DashboardBase {
    init = () => {
        let totalRequest = this.$('#totalRequest').val();
        this.$('.new-request').html("(" + totalRequest + ")");
        this.registerEvents();
    };

    registerEvents = () => {
        let self = this;
        this.$('#memberrequest-tab').on('click', function() {
            let position = self.$(this).position();
            self.$(".scroll-tabs").scrollLeft(position.left);
        });
    };
}

$(() => {
    const memberRequests = new MemberRequests($);
    memberRequests.loadInfo('#member-requests', memberRequests.init);
})