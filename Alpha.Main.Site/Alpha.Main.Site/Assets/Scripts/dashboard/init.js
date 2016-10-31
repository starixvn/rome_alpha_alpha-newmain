var Dashboard;
(function (Dashboard) {
    $(document).ready(function () {
        var balanceInfo = new Dashboard.BalanceInfo($);
        balanceInfo.registerEvent();
        var stats = new Dashboard.Statistics($);
        stats.loadInfo('#stats', stats.registerEvent);
        var notice = new Dashboard.NoticeMessage($);
        notice.loadInfo('#notice', notice.registerEvent);
    });
})(Dashboard || (Dashboard = {}));
//# sourceMappingURL=init.js.map