/// <reference path="./balance-info.ts" />
/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />
var Dashboard = (function () {
    function Dashboard() {
        var _this = this;
        this.startLoading = function () {
            var balanceInfo = new BalanceInfo();
            balanceInfo.registerEvent();
            _this.loadInfo('#stats', _this.onCompleteLoadStats);
            _this.loadInfo('#notice', _this.onCompleteLoadNotice);
        };
        this.loadInfo = function (id, onCompleteLoading) {
            var $component = $(id), url = $component.attr('action-url');
            if (url !== '' || typeof url !== 'undefined') {
                $.get(url, function (responseHtml) {
                    $component.html(responseHtml);
                    onCompleteLoading();
                });
            }
        };
        this.onCompleteLoadStats = function () {
            var stats = new Statistics();
            stats.registerEvent();
        };
        this.onCompleteLoadNotice = function () {
            var notice = new NoticeMessage();
            notice.registerEvent();
        };
    }
    return Dashboard;
}());
$(document).ready(function () {
    var dashboard = new Dashboard();
    dashboard.startLoading();
});
//# sourceMappingURL=dashboard.js.map