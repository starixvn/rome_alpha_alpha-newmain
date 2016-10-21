class BalanceInfo extends Dashboard {
    registerEvent = () => {
        this.onSwipeInBalanceInfoMobileView();
    }

    private onSwipeInBalanceInfoMobileView = () => {
        const balance = document.getElementById('balance'),
            balanceTab = document.getElementById('balance-tab'),
            stats = document.getElementById('stats'),
            statsTab = document.getElementById('stats-tab');

        this.$('#balance').hammer().on('swipeleft', () => {
            balance.classList.remove('active');
            balanceTab.classList.remove('active');

            stats.classList.add('active');
            statsTab.classList.add('active');
        });
    }
}