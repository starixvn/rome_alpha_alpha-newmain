var Statistics = (function () {
    function Statistics() {
        var _this = this;
        this.registerEvent = function () {
            $('[data-toggle="tooltip"]').tooltip({ html: true });
            _this.onToggleDropdown('#listTopWinningMembers', _this.loadInfo);
            _this.onToggleDropdown('#listTopTurnoverMembers', _this.loadInfo);
            _this.onToggleDropdown('#listNewMembers', _this.loadInfo);
            _this.onSwipeInMobileView();
        };
        this.onToggleDropdown = function (id, loadInfo) {
            var $this = $(id), $dataElement = $this.next(), url = $this.attr('action-url');
            $this.click(function () {
                if ($dataElement.is(':hidden')) {
                    loadInfo(url, $dataElement);
                }
            });
        };
        this.loadInfo = function (url, dataElement) {
            if (url !== '' || typeof url !== 'undefined') {
                $.get(url, function (responseHtml) {
                    dataElement.html(responseHtml);
                });
            }
        };
        this.onSwipeInMobileView = function () {
            var balance = document.getElementById('balance'), balanceTab = document.getElementById('balance-tab'), stats = document.getElementById('stats'), statsTab = document.getElementById('stats-tab'), notice = document.getElementById('notice'), noticeTab = document.getElementById('notice-tab');
            var statsHM = new Hammer(stats);
            statsHM.on("swiperight", function () {
                stats.classList.remove('active');
                statsTab.classList.remove('active');
                balance.classList.add('active');
                balanceTab.classList.add('active');
            });
            statsHM.on("swipeleft", function () {
                stats.classList.remove('active');
                statsTab.classList.remove('active');
                notice.classList.add('active');
                noticeTab.classList.add('active');
            });
        };
    }
    return Statistics;
}());
