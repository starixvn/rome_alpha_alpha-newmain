class Statistics extends DashboardBase {
    registerEvents = () => {
        this.$('[data-toggle="tooltip"]').tooltip({
            html: true,
            trigger: 'hover'
        });

        this.onToggleDropdown('#listTopWinningMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listTopTurnoverMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listNewMembers', this.loadDropdownInfo);
    }

    private onToggleDropdown = (id: string, loadInfo: (url: string, dataElement: JQuery) => void) => {
        let $this = this.$(id),
            dataElement = $this.next(),
            url = $this.attr('action-url');

        $this.click(() => {
            if (dataElement.is(':hidden')) {
                loadInfo(url, dataElement);
            }
        });
    }

    private loadDropdownInfo = (url: string, dataElement: JQuery): void => {
        if (url !== '' && typeof url !== 'undefined') {
            this.$.ajax({
                method: 'GET',
                url: url,
                success: (responseHtml) => dataElement.html(responseHtml)
            });
        }
    }
}

$(() => {
    const stats = new Statistics($);
    stats.loadInfo('#stats', stats.registerEvents);
})