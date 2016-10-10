class BalanceInfo {
    registerEvent = () => {
        this.onSwipeInBalanceInfoMobileView();
    }

    private onSwipeInBalanceInfoMobileView = () => {
        const balance = document.getElementById('balance'),
            balanceTab = document.getElementById('balance-tab'),
            stats = document.getElementById('stats'),
            statsTab = document.getElementById('stats-tab');

        let balanceHM = new Hammer(balance);

        balanceHM.on('swipeleft', () => {
            balance.classList.remove('active');
            balanceTab.classList.remove('active');

            stats.classList.add('active');
            statsTab.classList.add('active');
        });
    }
}