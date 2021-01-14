var DashboardBase = /** @class */ (function () {
    function DashboardBase($) {
        var _this = this;
        this.loadInfo = function (id, handleOnSuccessLoading) {
            var $component = _this.$(id), url = $component.attr('action-url');
            if (url !== '' && typeof url !== 'undefined') {
                var ajaxSettings = _this.createAjaxSettings(url, 'GET', $component, handleOnSuccessLoading);
                _this.$.ajax(ajaxSettings);
            }
        };
        this.createAjaxSettings = function (url, method, component, handleOnSuccessLoading) {
            var ajaxSetting = {
                method: method,
                url: url,
                cache: false,
                success: function (responseHtml) {
                    component.html(responseHtml);
                    handleOnSuccessLoading();
                }
            };
            return ajaxSetting;
        };
        this.$ = $;
    }
    return DashboardBase;
}());
//# sourceMappingURL=dashboard.js.map