module Dashboard {
    export class Statistics extends Base {
        registerEvent = () => {
            this.handleTooltip();
            this.onToggleDropdown('#listTopWinningMembers', this.loadDropdownInfo);
            this.onToggleDropdown('#listTopTurnoverMembers', this.loadDropdownInfo);
            this.onToggleDropdown('#listNewMembers', this.loadDropdownInfo);
        }

        private handleTooltip = () => {
            const thisTooltip = this.$('[data-toggle="tooltip"]');

            thisTooltip.tooltip({ html: true });

            $('body').on('touchend', () => {
                thisTooltip.tooltip('hide');
                thisTooltip.tooltip({ html: true });
            });
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
            if (url !== '' || typeof url !== 'undefined') {
                this.$.get(url, (responseHtml) => {
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

            this.$('#stats').hammer().on("swiperight", () => {
                stats.classList.remove('active');
                statsTab.classList.remove('active');

                balance.classList.add('active');
                balanceTab.classList.add('active');
            });

            this.$('#stats').hammer().on("swipeleft", () => {
                stats.classList.remove('active');
                statsTab.classList.remove('active');

                notice.classList.add('active');
                noticeTab.classList.add('active');
            });
        }
    }
}