var DataProccessingNotification = /** @class */ (function () {
    function DataProccessingNotification($) {
        var _this = this;
        this.progressBoxCollapseTimeout = 5000; //5s
        this.getMessageTimeout = 60000; //1min
        this.getMoveBetMessageUrl = alpha.urls.messageRootPath + "MarqueeMessage/MoveBet";
        this.getMessage = function () {
            _this.$.ajax({
                type: "GET",
                url: _this.getMoveBetMessageUrl,
                success: _this.onGetMessageSuccess
            });
        };
        this.onGetMessageSuccess = function (data) {
            if (data && data.Content) {
                _this.$dataNotificationModal.find(".modal-body").html(data.Content);
                if (_this.$progressBox.hasClass("hide")) {
                    _this.showProgressBox();
                }
            }
            else {
                _this.$progressBox.addClass("hide");
            }
            setTimeout(_this.getMessage, _this.getMessageTimeout);
        };
        this.onProgressBoxClick = function () {
            _this.$dataNotificationModal.modal("show");
        };
        this.$ = $;
    }
    DataProccessingNotification.prototype.init = function () {
        this.cacheDOMElements();
        this.registerEvents();
        this.getMessage();
    };
    DataProccessingNotification.prototype.cacheDOMElements = function () {
        this.$progressBox = this.$("#progress-box");
        this.$dataNotificationModal = this.$("#data-processing-popup");
    };
    DataProccessingNotification.prototype.registerEvents = function () {
        var _this = this;
        this.$progressBox.on("touchstart", function () {
            _this.startPosition = _this.$progressBox.position();
        });
        this.$progressBox.on("touchend", function (event) {
            event.preventDefault();
            var endPosition = _this.$progressBox.position();
            if (Math.abs(_this.startPosition.top - endPosition.top) < 3
                && Math.abs(_this.startPosition.left - endPosition.left) < 3) {
                _this.onProgressBoxClick();
            }
        });
    };
    DataProccessingNotification.prototype.showProgressBox = function () {
        var _this = this;
        this.$progressBox
            .removeAttr("style")
            .removeClass("collapsed")
            .removeClass("hide")
            .draggable({ disabled: true });
        setTimeout(function () {
            _this.$progressBox
                .addClass("collapsed")
                .draggable({ disabled: false, containment: "#content-wrapper" });
        }, this.progressBoxCollapseTimeout);
    };
    return DataProccessingNotification;
}());
$(document).ready(function () {
    if (alpha.isScreenLower768) {
        var dataProcessingNotification = new DataProccessingNotification($);
        dataProcessingNotification.init();
    }
});
//# sourceMappingURL=data-proccessing-notification.js.map