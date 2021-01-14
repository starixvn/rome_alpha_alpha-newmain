var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MemberRequests = /** @class */ (function (_super) {
    __extends(MemberRequests, _super);
    function MemberRequests($, topWindow) {
        var _this = _super.call(this, $) || this;
        _this.init = function () {
            var totalRequest = _this.$('#totalRequest').val();
            _this.$('.new-request').html("(" + totalRequest + ")");
            _this.registerEvents();
        };
        _this.registerEvents = function () {
            var self = _this;
            _this.$('#memberrequest-tab').on('click', function () {
                var position = self.$(this).position();
                self.$(".scroll-tabs").scrollLeft(position.left);
            });
            _this.$('.icon-information-outline').on('click', _this.showGuideline);
        };
        _this.showGuideline = function () {
            var title = _this.$('#PlayForFunGuide').val();
            var url = _this.$('#GuidelineUrl').val();
            _this.topWindow.popupManager.open(url, title, 900, 600, "popup-guidelines");
        };
        _this.topWindow = topWindow;
        return _this;
    }
    return MemberRequests;
}(DashboardBase));
$(function () {
    var memberRequests = new MemberRequests($, window.top);
    memberRequests.loadInfo('#member-requests', memberRequests.init);
});
//# sourceMappingURL=member-requests.js.map