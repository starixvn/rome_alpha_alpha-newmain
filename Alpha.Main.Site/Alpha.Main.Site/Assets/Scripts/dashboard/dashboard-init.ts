class DashboardInit {
    constructor($: JQueryStatic) {
        $(document).ready(() => {
            const stats = new Statistics($);
            stats.loadInfo('#stats', stats.registerEvent);

            const notice = new NoticeMessage($);
            notice.loadInfo('#notice', notice.registerEvent);
        });
    }
};

new DashboardInit($);