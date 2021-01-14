/// <reference path="../core.ts" />
var Kickout = /** @class */ (function () {
    function Kickout() {
        var _this = this;
        this.init = function () {
            _this.message = document.getElementById('message') != null
                ? document.getElementById('message').value
                : "Oops... you are here because of a system error, please notify the administrators.";
            alert(_this.message);
            alpha.core.signOut();
        };
    }
    return Kickout;
}());
$(function () {
    var kickOut = new Kickout();
    kickOut.init();
});
//# sourceMappingURL=kick-out.js.map