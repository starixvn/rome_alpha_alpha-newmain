/// <reference path="./balance-info.ts" />
/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />

class Dashboard {
    startLoading = () => {
        const balanceInfo = new BalanceInfo();
        balanceInfo.registerEvent();

        this.loadInfo('#stats', this.onCompleteLoadStats);
        this.loadInfo('#notice', this.onCompleteLoadNotice);
    }

    private loadInfo = (id: string, onCompleteLoading: any) => {
        let $component = $(id),
            url = $component.attr('action-url');

        if (url !== '' || typeof url !== 'undefined') {
            $.get(url,
                (responseHtml) => {
                    $component.html(responseHtml);
                    onCompleteLoading();
                });
        }
    }

    private onCompleteLoadStats = () => {
        const stats = new Statistics();
        stats.registerEvent();
    }

    private onCompleteLoadNotice = () => {
        const notice = new NoticeMessage();
        notice.registerEvent();
    }
}

$(document).ready(() => {
    const dashboard = new Dashboard();
    dashboard.startLoading();
});