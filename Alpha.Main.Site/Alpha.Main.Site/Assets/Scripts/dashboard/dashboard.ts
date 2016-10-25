module Dashboard {
    $(document).ready(() => {
        const balanceInfo = new BalanceInfo($);
        balanceInfo.registerEvent();

        const stats = new Statistics($);
        stats.loadInfo('#stats', stats.registerEvent);

        const notice = new NoticeMessage($);
        notice.loadInfo('#notice', notice.registerEvent);
    });
}