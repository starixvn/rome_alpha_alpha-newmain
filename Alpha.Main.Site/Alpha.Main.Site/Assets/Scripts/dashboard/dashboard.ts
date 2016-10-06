/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />

class Dashboard {
    constructor() {
        this.loadInfo('#stats', this.onCompleteLoadStats);
        this.loadInfo('#notice', this.onCompleteLoadNotice);
    }

    private loadInfo = (id: string, onCompleteLoading: any): void => {
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

    private onCompleteLoadStats = (): void => {
        new Statistics();
    }

    private onCompleteLoadNotice = (): void => {
        new NoticeMessage();
    }
}

$(document).ready(() => {
    new Dashboard();
});