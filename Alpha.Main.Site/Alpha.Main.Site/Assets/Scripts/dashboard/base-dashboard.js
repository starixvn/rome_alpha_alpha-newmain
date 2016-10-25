var Dashboard;
(function (Dashboard) {
    var BaseDashBoard = (function () {
        function BaseDashBoard($) {
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
                    success: function (responseHtml) {
                        component.html(responseHtml);
                        handleOnSuccessLoading();
                    }
                };
                return ajaxSetting;
            };
            this.$ = $;
        }
        return BaseDashBoard;
    }());
    Dashboard.BaseDashBoard = BaseDashBoard;
})(Dashboard || (Dashboard = {}));
//# sourceMappingURL=base-dashboard.js.map