var BalanceInfo = (function () {
    function BalanceInfo() {
        var _this = this;
        this.registerEvent = function () {
            _this.onSwipeInBalanceInfoMobileView();
        };
        this.onSwipeInBalanceInfoMobileView = function () {
            var balance = document.getElementById('balance'), balanceTab = document.getElementById('balance-tab'), stats = document.getElementById('stats'), statsTab = document.getElementById('stats-tab');
            var balanceHM = new Hammer(balance);
            balanceHM.on('swipeleft', function () {
                balance.classList.remove('active');
                balanceTab.classList.remove('active');
                stats.classList.add('active');
                statsTab.classList.add('active');
            });
        };
    }
    return BalanceInfo;
}());
//# sourceMappingURL=balance-info.js.map