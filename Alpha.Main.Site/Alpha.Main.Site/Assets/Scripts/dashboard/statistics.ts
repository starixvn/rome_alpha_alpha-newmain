class Statistics {
    constructor() {
        $('[data-toggle="tooltip"]').tooltip({ html: true });

        this.onToggleDropdown('#listTopWinningMembers', this.loadInfo);
        this.onToggleDropdown('#listTopTurnoverMembers', this.loadInfo);
        this.onToggleDropdown('#listNewMembers', this.loadInfo);
    }

    private onToggleDropdown = (id: string, loadInfo: any) => {
        let $this = $(id),
            $dataElement = $this.next(),
            url = $this.attr('action-url');

        $this.click(() => {
            if ($dataElement.is(':hidden')) {
                loadInfo(url, $dataElement);
            }
        });
    }

    private loadInfo = (url: string, dataElement: any): void => {
        if (url !== '' || typeof url !== 'undefined') {
            $.get(url, (responseHtml) => {
                dataElement.html(responseHtml);
            });
        }
    }
}