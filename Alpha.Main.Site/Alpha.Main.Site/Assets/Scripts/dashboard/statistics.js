var Statistics = (function () {
    function Statistics() {
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
        $('[data-toggle="tooltip"]').tooltip({ html: true });
        this.onToggleDropdown('#listTopWinningMembers', this.loadInfo);
        this.onToggleDropdown('#listTopTurnoverMembers', this.loadInfo);
        this.onToggleDropdown('#listNewMembers', this.loadInfo);
    }
    return Statistics;
}());
//# sourceMappingURL=statistics.js.map