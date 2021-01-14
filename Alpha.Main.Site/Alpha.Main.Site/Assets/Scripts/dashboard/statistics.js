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
var Statistics = /** @class */ (function (_super) {
    __extends(Statistics, _super);
    function Statistics() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.registerEvents = function () {
            _this.onToggleDropdown('#listTopWinningMembers', _this.loadDropdownInfo);
            _this.onToggleDropdown('#listTopTurnoverMembers', _this.loadDropdownInfo);
            _this.onToggleDropdown('#listNewMembers', _this.loadDropdownInfo);
            _this.registerTooltip();
        };
        _this.onToggleDropdown = function (id, loadInfo) {
            var $this = _this.$(id), dataElement = $this.next(), url = $this.attr('action-url');
            $this.on('click', function () {
                if (dataElement.is(':hidden')) {
                    loadInfo(url, dataElement);
                }
            });
        };
        _this.loadDropdownInfo = function (url, dataElement) {
            if (url !== '' && typeof url !== 'undefined') {
                _this.$.ajax({
                    method: 'GET',
                    url: url,
                    cache: false,
                    success: function (responseHtml) { return dataElement.html(responseHtml); }
                });
            }
        };
        _this.registerTooltip = function () {
            _this.$('[data-toggle="tooltip"]').tooltip({ html: true });
            _this.hideTooltipOnTouchOutInMobile();
        };
        _this.hideTooltipOnTouchOutInMobile = function () {
            var $tooltip = _this.$('[data-toggle="tooltip"]');
            _this.$(window.document).on('touchend', function (event) {
                var target = $(event.target);
                $tooltip.tooltip('hide');
                if (target.attr('data-toggle') === 'tooltip') {
                    target.tooltip('show');
                }
                else if (target.parent().attr('data-toggle') === 'tooltip') {
                    target.parent().tooltip('show');
                }
            });
        };
        return _this;
    }
    return Statistics;
}(DashboardBase));
$(function () {
    var stats = new Statistics($);
    stats.loadInfo('#stats', stats.registerEvents);
});
//# sourceMappingURL=statistics.js.map