/// <reference path="../core.ts" />
var Message = (function () {
    function Message() {
        var _this = this;
        this.registerEvent = function () {
            _this.onClickTab('.public-tab');
            _this.onClickTab('.special-tab');
            _this.onClickTab('.private-tab');
            _this.initDateTimePicker('#fromDateContainer');
            _this.initDateTimePicker('#toDateContainer');
        };
        this.onClickTab = function (tabName) {
            var thisTab = $(tabName);
            thisTab.click(function () {
                var ajaxSetting = {
                    url: alpha.urls.mainRootPath + '/Message/Index',
                    data: { messageType: thisTab.attr('id') },
                    method: 'GET',
                    success: function (responseHtml) {
                        setTimeout(function () {
                            var newDoc = document.open("text/html", "replace");
                            newDoc.write(responseHtml);
                            newDoc.close();
                        }, 300);
                    }
                };
                $.ajax(ajaxSetting);
            });
        };
        this.initDateTimePicker = function (id) {
            $(id).datetimepicker({
                allowInputToggle: true,
                format: 'MM/DD/YYYY'
            });
        };
    }
    return Message;
}());
$(document).ready(function () {
    var message = new Message();
    message.registerEvent();
});
//# sourceMappingURL=index.js.map