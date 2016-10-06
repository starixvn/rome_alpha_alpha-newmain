/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />
var Dashboard = (function () {
    function Dashboard() {
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
            new Statistics();
        };
        this.onCompleteLoadNotice = function () {
            new NoticeMessage();
        };
        this.loadInfo('#stats', this.onCompleteLoadStats);
        this.loadInfo('#notice', this.onCompleteLoadNotice);
    }
    ;
    return Dashboard;
}());
$(document).ready(function () {
    new Dashboard();
});
//# sourceMappingURL=dashboard.js.map