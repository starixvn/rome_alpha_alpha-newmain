class MemberRequests extends DashboardBase {
    registerEvents = () => { }
}

$(() => {
    const memberRequests = new MemberRequests($);
    memberRequests.loadInfo('#member-requests', memberRequests.registerEvents);
})