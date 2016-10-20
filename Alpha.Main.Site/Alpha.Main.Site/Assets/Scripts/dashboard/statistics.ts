class Statistics extends Dashboard {
    registerEvent = () => {
        $('[data-toggle="tooltip"]').tooltip({ html: true });

        this.onToggleDropdown('#listTopWinningMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listTopTurnoverMembers', this.loadDropdownInfo);
        this.onToggleDropdown('#listNewMembers', this.loadDropdownInfo);

        this.onSwipeInMobileView();
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

    private loadDropdownInfo = (url: string, dataElement: any): void => {
        if (url !== '' || typeof url !== 'undefined') {
            $.get(url, (responseHtml) => {
                dataElement.html(responseHtml);
            });
        }
    }

    private onSwipeInMobileView = () => {
        const balance = document.getElementById('balance'),
            balanceTab = document.getElementById('balance-tab'),
            stats = document.getElementById('stats'),
            statsTab = document.getElementById('stats-tab'),
            notice = document.getElementById('notice'),
            noticeTab = document.getElementById('notice-tab');

        let statsHM = new Hammer(stats);

        statsHM.on("swiperight", () => {
            stats.classList.remove('active');
            statsTab.classList.remove('active');

            balance.classList.add('active');
            balanceTab.classList.add('active');
        });

        statsHM.on("swipeleft", () => {
            stats.classList.remove('active');
            statsTab.classList.remove('active');

            notice.classList.add('active');
            noticeTab.classList.add('active');
        });
    }
}

const stats = new Statistics($);
stats.loadInfo('#stats', stats.registerEvent);