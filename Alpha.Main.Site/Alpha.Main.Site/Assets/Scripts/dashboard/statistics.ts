class Statistics extends DashboardBase {
    registerEvents = () => {
        this.onToggleDropdown('#listTopWinningMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listTopTurnoverMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listNewMembers', this.loadDropdownInfo);

        this.registerTooltip();
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

    private registerTooltip = () => {
        this.$('.tooltip-stats-member').tooltip({ html: true });

        this.hideTooltipOnTouchOutInMobile('tooltip-stats-member');
    }

    private hideTooltipOnTouchOutInMobile = (className: string) => {
        const $tooltip = this.$('.' + className);

        this.$(window.document).on('touchend', (event) => {
            const target = $(event.target);

            if (target.hasClass(className) || target.parent().hasClass(className)) {
                $tooltip.tooltip('show');
            }
            else {
                $tooltip.tooltip('hide');
            }
        });
    }
}

$(() => {
    const stats = new Statistics($);
    stats.loadInfo('#stats', stats.registerEvents);
})