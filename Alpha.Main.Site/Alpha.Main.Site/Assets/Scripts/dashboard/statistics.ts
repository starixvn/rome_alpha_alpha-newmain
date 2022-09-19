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

        $this.on('click', () => {
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
                cache: false,
                success: (responseHtml) => dataElement.html(responseHtml)
            });
        }
    }

    private registerTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip({ html: true });

        this.hideTooltipOnTouchOutInMobile();
    }

    private hideTooltipOnTouchOutInMobile = () => {
        const $tooltip = this.$('[data-toggle="tooltip"]');

        this.$(window.document).on('touchend', (event) => {
            const target = $(event.target);

            $tooltip.tooltip('hide');

            if (target.attr('data-toggle') === 'tooltip') {
                target.tooltip('show');
            }
            else if (target.parent().attr('data-toggle') === 'tooltip') {
                target.parent().tooltip('show');
            }
        });
    }
}

$(() => {
    const stats = new Statistics($);
    stats.loadInfo('#stats', stats.registerEvents);
})